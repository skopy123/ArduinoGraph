using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace ArduinoGraph {
    class BMP180Example : DataTransformTool {
        public BMP180Example(Chart outputChart): base(outputChart)  {
            chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisX.Title = "Time";
            chart.ChartAreas[0].AxisX.Interval = 0.001;
            chart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;

            //temperature axis settings
            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisY.Title = "Temperature [°C]";
            chart.ChartAreas[0].AxisY.Minimum = 15;
            chart.ChartAreas[0].AxisY.Maximum = 35;
            chart.ChartAreas[0].AxisY.MajorGrid.Interval = 2;
            chart.ChartAreas[0].AxisY.MinorGrid.Interval = 1;
            chart.ChartAreas[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart.ChartAreas[0].AxisY.MinorGrid.Enabled = true;

            //pressure axis settings
            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisY2.Title = "Pressure [hPa]";
            chart.ChartAreas[0].AxisY2.IsStartedFromZero = false;
            chart.ChartAreas[0].AxisY2.Minimum = 950;
            chart.ChartAreas[0].AxisY2.Maximum = 1050;
            chart.ChartAreas[0].AxisY2.MajorGrid.Interval = 10;

            chart.Series.Add("temperature");
            chart.Series["temperature"].Color = Color.Red;
            chart.Series["temperature"].ChartType = SeriesChartType.Line;
            chart.Series["temperature"].MarkerStyle = MarkerStyle.Cross;
            chart.Series["temperature"].MarkerSize = 5;
            chart.Series["temperature"].YAxisType = AxisType.Primary;


            chart.Series.Add("pressure");
            chart.Series["pressure"].Color = Color.Blue;
            chart.Series["pressure"].ChartType = SeriesChartType.Line;
            chart.Series["pressure"].MarkerStyle = MarkerStyle.Cross;
            chart.Series["pressure"].MarkerSize = 5;
            chart.Series["pressure"].YAxisType = AxisType.Secondary;
        }

        public override void ProcessMessage(string s) {
            // process message from arduino here
            DateTime dt = DateTime.Now;
            string[] values = s.Split(';');
            if (values.Count() > 1) {
                double d = Convert.ToDouble(values[0]);
                d = d / 100;
                chart.Series["temperature"].Points.AddXY(dt.ToOADate(), d);
                d = Convert.ToDouble(values[2]);
                d = d / 100;
                chart.Series["pressure"].Points.AddXY(dt.ToOADate(), d);
            }
        }
    }
}
