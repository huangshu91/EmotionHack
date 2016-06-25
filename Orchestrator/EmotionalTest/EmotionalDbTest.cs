using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmotionalTest
{
    [TestClass]
    public class EmotionalDbTest
    {
        private string _connection = @"Server=tcp:emotiondb.database.windows.net,1433;Database=EmotionHackPPE;User ID=emotionhack@emotiondb;Password=@NYTH!NG123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}
