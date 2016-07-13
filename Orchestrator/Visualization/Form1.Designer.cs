namespace Visualization
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.combinedChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.angerChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.combinedChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.angerChart)).BeginInit();
            this.SuspendLayout();
            // 
            // combinedChart
            // 
            chartArea1.Name = "ChartArea1";
            this.combinedChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.combinedChart.Legends.Add(legend1);
            this.combinedChart.Location = new System.Drawing.Point(12, 12);
            this.combinedChart.Name = "combinedChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.combinedChart.Series.Add(series1);
            this.combinedChart.Size = new System.Drawing.Size(300, 300);
            this.combinedChart.TabIndex = 0;
            this.combinedChart.Text = "chart1";
            // 
            // angerChart
            // 
            chartArea2.Name = "ChartArea1";
            this.angerChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.angerChart.Legends.Add(legend2);
            this.angerChart.Location = new System.Drawing.Point(342, 12);
            this.angerChart.Name = "angerChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.angerChart.Series.Add(series2);
            this.angerChart.Size = new System.Drawing.Size(300, 300);
            this.angerChart.TabIndex = 1;
            this.angerChart.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 631);
            this.Controls.Add(this.angerChart);
            this.Controls.Add(this.combinedChart);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.combinedChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.angerChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart combinedChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart angerChart;
    }
}

