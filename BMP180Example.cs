using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace ArduinoGraph {
    class BMP180Example : DataTransformTool {

        DateTime start;

        public BMP180Example(Chart outputChart): base(outputChart)  {
            start = DateTime.Now;
            chart.ChartAreas[0].AxisX.Title = "Time [s]";
            chart.ChartAreas[0].AxisX.MajorGrid.Interval = 5;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "F0";

            //temperature axis settings
            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisY.Title = "Temperature [°C]";
            chart.ChartAreas[0].AxisY.Minimum = 15;
            chart.ChartAreas[0].AxisY.Maximum = 35;
            chart.ChartAreas[0].AxisY.MajorGrid.Interval = (chart.ChartAreas[0].AxisY.Maximum - chart.ChartAreas[0].AxisY.Minimum) / 10;
            chart.ChartAreas[0].AxisY.MinorGrid.Interval = (chart.ChartAreas[0].AxisY.Maximum - chart.ChartAreas[0].AxisY.Minimum) / 20;
            chart.ChartAreas[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart.ChartAreas[0].AxisY.MinorGrid.Enabled = true;

            //pressure axis settings
            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisY2.Title = "Pressure [hPa]";
            chart.ChartAreas[0].AxisY2.IsStartedFromZero = false;
            chart.ChartAreas[0].AxisY2.Minimum = 980;
            chart.ChartAreas[0].AxisY2.Maximum = 1030;
            chart.ChartAreas[0].AxisY2.MajorGrid.Interval = (chart.ChartAreas[0].AxisY2.Maximum - chart.ChartAreas[0].AxisY2.Minimum) / 10;

            chart.Series.Add("temperature");
            chart.Series["temperature"].Color = Color.Red;
            chart.Series["temperature"].ChartType = SeriesChartType.Line;
            chart.Series["temperature"].MarkerStyle = MarkerStyle.Cross;
            chart.Series["temperature"].MarkerSize = 5;
            chart.Series["temperature"].YAxisType = AxisType.Primary; // use AxisY


            chart.Series.Add("pressure");
            chart.Series["pressure"].Color = Color.Blue;
            chart.Series["pressure"].ChartType = SeriesChartType.Line;
            chart.Series["pressure"].MarkerStyle = MarkerStyle.Cross;
            chart.Series["pressure"].MarkerSize = 5;
            chart.Series["pressure"].YAxisType = AxisType.Secondary; // use AxisY2
        }

        /*
         * input data sended by mcu look like:
         23;101274;
         24;101272;
         * one row is one sample, ";" is used as delimiter, ProcessMessage function ignore initial Log messages if not contains at least 2 delimiters
         */
        public override void ProcessMessage(string s) {
            try {
                // process message from arduino here
                double x = (DateTime.Now - start).TotalSeconds;
                string[] values = s.Split(';');
                if (values.Count() > 1) {
                    double d = Convert.ToDouble(values[0]);
                    chart.Series["temperature"].Points.AddXY(x, d);
                    d = Convert.ToDouble(values[1]);
                    d = d / 100;
                    chart.Series["pressure"].Points.AddXY(x, d);
                }
                chart.ChartAreas[0].AxisX.Minimum = x - 30;
            }
            catch { 
                // message cannot be procesed
            }
        }

    }
}
