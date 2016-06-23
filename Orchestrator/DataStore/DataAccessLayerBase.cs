using System;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataStore
{
    /// <summary>
    /// Provides generally useful methods for invoking queries against SQL Server.  Can be used as part of the implementation
    /// of a data access layer.
    /// </summary>
    public class DataAccessLayerBase : IDisposable
    {
        public string ConnectionString { get; set; }
        protected SqlConnection _Connection = null;
        protected SqlCommand _Command;
        protected SqlTransaction _Transaction;
        protected int? _ExecutionTimeout = null;
        bool _IsDisposed = false;


        /// <summary>
        /// Initializes the connection and command objects, and opens the connection.
        /// </summary>
        public void Open()
        {
            _Connection = new SqlConnection(ConnectionString);
            _Connection.Open();
            _Command = _Connection.CreateCommand();
            if (_ExecutionTimeout.HasValue)
            {
                _Command.CommandTimeout = _ExecutionTimeout.Value;
            }
        }

        private static readonly int[] RetryDelay = new int[] { 2, 30, 60, 90 };
        private static readonly int RetryCount = RetryDelay.Length;

        /// <summary>
        /// See WithDataLayer with the type parameter T below for details.  This is similar but returns no value.
        /// </summary>
        /// <param name="connectionString">Which database to access</param>
        /// <param name="protectedAction">See below</param>
        public static void WithDataLayer(string connectionString, Action<DataAccessLayerBase> protectedAction)
        {
            WithDataLayer<int>(connectionString, (dataLayer) =>
            {
                protectedAction(dataLayer);
                return 0;
            });
        }

        /// <summary>
        /// Provides retry functionality when SQL Exceptions occurs, and ensures that the data access layer is cleaned up
        /// </summary>
        /// <typeparam name="T">The result returned by WithDataLayer from the protected method provided</typeparam>
        /// <param name="connectionString">Which database to access</param>
        /// <param name="protectedFunction">A method or lambda expression invoked which will be retried if SQL Exceptions occur</param>
        /// <returns>What the protected method returns</returns>
        public static T WithDataLayer<T>(string connectionString, Func<DataAccessLayerBase, T> protectedFunction)
        {
            DataAccessLayerBase dataLayer = new DataAccessLayerBase();
            dataLayer.ConnectionString = connectionString;
            return dataLayer.WithDataLayer<T, DataAccessLayerBase>(protectedFunction);
        }

        /// <summary>
        /// Provides retry functionality when SQL Exceptions occurs, and ensures that the data access layer is cleaned up
        /// </summary>
        /// <typeparam name="TResult">The result returned by WithDataLayer from the protected method provided</typeparam>
        /// <typeparam name="TDataAccess">The class to use for the data access layer.  Typically this is either DataAccessLayerBase or a subclass of it. </typeparam>
        /// <param name="protectedFunction">A method or lambda expression invoked which will be retried if SQL Exceptions occur</param>
        /// <returns>What the protected method returns</returns>
        public TResult WithDataLayer<TResult, TDataAccess>(Func<TDataAccess, TResult> protectedFunction) where TDataAccess : DataAccessLayerBase
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new Exception("Connection string must be non-null and non-whitespace.");
            }
            for (int retry = 0; retry < RetryCount; retry++)
            {
                try
                {
                    this.Open();

                    return protectedFunction(this as TDataAccess);
                }
                catch (SqlException)
                {
                    if (retry + 1 >= RetryCount)
                    {
                        throw;
                    }
                    //Retry with ever long delays between.  This allows us to
                    //recover quickly from fast resolving issues, while also giving time  
                    //to Azure SQL Database to recover in the case where a failover is required.
                    Thread.Sleep(1000 * RetryDelay[retry]);
                }
                finally
                {
                    this.Close();
                }
            }
            //It's not really possible to get to this point, but if you do something is terribly wrong!
            //This must be here to shut up the compiler error of not returning something from all paths.
            throw new ApplicationException("Not possible to get here in WithDataLayer.");
        }

        protected internal void ExecuteTransaction<T>(params Action<T>[] protectedActions) where T : DataAccessLayerBase
        {
            this.WithDataLayer<int, DataAccessLayerBase>(x =>
            {
                var transaction = this._Connection.BeginTransaction();

                this._Transaction = transaction;
                try
                {
                    foreach (var protectedAction in protectedActions)
                    {
                        protectedAction(this as T);
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    this._Transaction = null;
                    throw;
                }
                transaction.Commit();
                this._Transaction = null;
                return 0;
            });
        }

        /// <summary>
        /// The async version of the WithDataLayerAsync
        /// </summary>
        /// <typeparam name="TResult">The result returned by WithDataLayer from the protected method provided</typeparam>
        /// <typeparam name="TDataAccess">The class to use for the data access layer.  Typically this is either DataAccessLayerBase or a subclass of it. </typeparam>
        /// <param name="protectedFunction">A method or lambda expression invoked which will be retried if SQL Exceptions occur</param>
        /// <returns>What the protected method returns</returns>
        public async Task<TResult> WithDataLayerAsync<TResult, TDataAccess>(Func<TDataAccess, Task<TResult>> protectedFunction) where TDataAccess : DataAccessLayerBase
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new Exception("Connection string must be non-null and non-whitespace.");
            }

            bool isSuccessful;

            for (int retry = 0; retry < RetryCount; retry++)
            {
                try
                {
                    isSuccessful = true;

                    this.Open();

                    return await protectedFunction(this as TDataAccess);
                }
                catch (SqlException)
                {
                    isSuccessful = false;

                    if (retry + 1 >= RetryCount)
                    {
                        throw;
                    }
                }
                finally
                {
                    this.Close();
                }

                if (!isSuccessful)
                {
                    //Retry with ever long delays between.  This allows us to
                    //recover quickly from fast resolving issues, while also giving time  
                    //to Azure SQL Database to recover in the case where a failover is required.
                    await Task.Delay(1000 * RetryDelay[retry]);
                }
            }
            //It's not really possible to get to this point, but if you do something is terribly wrong!
            //This must be here to shut up the compiler error of not returning something from all paths.
            throw new ApplicationException("Not possible to get here in WithDataLayer.");
        }

        /// <summary>
        /// Executes a non-query type of command
        /// </summary>
        /// <param name="queryString">What action to take expressed as Tranact-SQL</param>
        /// <param name="isSproc"></param>
        /// <param name="parameters">A sequence of parameter names and values</param>
        public int ExecuteCommand(string queryString, bool isSproc, params object[] parameters)
        {
            PrepareCommand(queryString, isSproc, parameters);
            return _Command.ExecuteNonQuery();
        }

        /// <summary>
        /// TExecutes a non-query type of command asynchronously
        /// </summary>
        /// <param name="queryString">What action to take expressed as Transact-SQL</param>
        /// <param name="isSproc">Is stored procedure?</param>
        /// <param name="parameters">A sequence of parameter names and values</param>
        /// <returns></returns>
        public async Task ExecuteCommandAsync(string queryString, bool isSproc, params object[] parameters)
        {
            PrepareCommand(queryString, isSproc, parameters);
            await _Command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Executes a query which returns a single atomic value (not rows of values).
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryString">The transact-sql query to execute</param>
        /// <param name="isSproc">Whether to invoke a sproc or just execute the text as a query</param>
        /// <param name="parameters">A sequence of parameter names and values</param>
        /// <returns>A single value</returns>
        public T ExecuteScalar<T>(string queryString, bool isSproc, params object[] parameters)
        {
            PrepareCommand(queryString, isSproc, parameters);

            var result = _Command.ExecuteScalar();
            return (T)(result);
        }

        /// <summary>
        /// Executes a query which returns a single atomic value asynchronously (not rows of values).
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="queryString">The transact-sql query to execute</param>
        /// <param name="isSproc">Whether to invoke a sproc or just execute the text as a query</param>
        /// <param name="parameters">A sequence of parameter names and values</param>
        /// <returns>A single value</returns>
        public async Task<T> ExecuteScalarAsync<T>(string queryString, bool isSproc, params object[] parameters)
        {
            PrepareCommand(queryString, isSproc, parameters);
            var result = await _Command.ExecuteScalarAsync();
            return (T)(result);
        }

        /// <summary>
        /// Create and return a SqlDataReader for reading rows of a result set based on the specified query
        /// </summary>
        /// <param name="queryString">The transact-sql query to execute</param>
        /// <param name="isSproc">Whether to invoke a sproc or just execute the text as a query</param>
        /// <param name="parameters">A sequence of parameter names and values</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string queryString, bool isSproc, params object[] parameters)
        {
            PrepareCommand(queryString, isSproc, parameters);
            return _Command.ExecuteReader();
        }

        /// <summary>
        /// Create and return a SqlDataReader for reading rows of a result set based on the specified query asynchronously
        /// </summary>
        /// <param name="queryString">The transact-sql query to execute</param>
        /// <param name="isSproc">Whether to invoke a sproc or just execute the text as a query</param>
        /// <param name="parameters">A sequence of parameter names and values</param>
        /// <returns></returns>
        public async Task<SqlDataReader> ExecuteReaderAsync(string queryString, bool isSproc, params object[] parameters)
        {
            PrepareCommand(queryString, isSproc, parameters);
            var result = await _Command.ExecuteReaderAsync();
            return result;
        }

        /// <summary>
        /// Sets up the SqlCommand for invocation, handling things like parameters for the query.
        /// </summary>
        /// <param name="queryString">The transact-sql query to execute</param>
        /// <param name="isSproc">Whether to invoke a sproc or just execute the text as a query</param>
        /// <param name="parameters">A sequence of parameter names and values</param>
        protected void PrepareCommand(string queryString, bool isSproc, object[] parameters)
        {
            if (_Connection == null)
            {
                throw new Exception("Attempt to use a closed connection in the data access layer.");
            }

            _Command.Parameters.Clear();
            if (this._Transaction != null)
            {
                _Command.Transaction = this._Transaction;
            }
            if (parameters.Length % 2 != 0)
            {
                throw new ArgumentException("Must provide an even number of parameter arguments (pairs of variable name and value).", "parameters");
            }
            _Command.CommandText = queryString;
            _Command.CommandType = isSproc ? CommandType.StoredProcedure : CommandType.Text;
            SqlParameter parameter;

            for (int i = 0; i < parameters.Length - 1; i += 2)
            {
                if (parameters[i + 1] != null)
                {
                    parameter = new SqlParameter((string)parameters[i], parameters[i + 1]);
                }
                else
                {
                    parameter = new SqlParameter((string)parameters[i], DBNull.Value);
                }
                _Command.Parameters.Add(parameter);
            }

        }

        /// <summary>
        /// Fills DataTable with results of sql query
        /// </summary>
        /// <param name="queryString">SQL query string</param>
        /// <returns></returns>
        public DataTable FillDataTable(string queryString)
        {
            var adapter = new SqlDataAdapter(queryString, this._Connection);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        /// <summary>
        /// If the specified column of the specified row of the results is database null, return a C# null.  Otherwise
        /// pull the value from the column of the row and return that.
        /// </summary>
        /// <typeparam name="T">What kind of data to return</typeparam>
        /// <param name="statusRow">Which row of the result set to read</param>
        /// <param name="columnNumber">Which column to look at</param>
        /// <param name="rowValueReader">A function which returns the value of the column in the row and assumes there is no db null in that column</param>
        /// <returns>C# null or the value of the column in the row of the result set</returns>
        public T CoaleseRowValue<T>(SqlDataReader statusRow, int columnNumber, Func<T> rowValueReader) where T : class
        {
            return (statusRow.IsDBNull(columnNumber)) ? (T)null : rowValueReader();
        }

        /// <summary>
        /// Release any resources held by the class.  If overriden in a child class, be sure to call the base method!
        /// </summary>
        public virtual void Close()
        {
            if (_Connection != null)
            {
                _Connection.Close();
                _Connection = null;
            }
        }


        /// <summary>
        /// Release any resources held by the class if not already disposed.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (_IsDisposed)
                {
                    return;
                }

                Close();

                _IsDisposed = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
            
    }
}
