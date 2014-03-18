using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using ArduinoGraphModuleInterface;

namespace ArduinoGraph {
    class AnalogReadExamples : IArduinoGraphModule {
        protected Chart chart;

        public void SetupChart(Chart c){
            chart = c;
            chart.Series.Add("value");
            chart.Series["value"].Color = Color.Red;
            chart.Series["value"].ChartType = SeriesChartType.Line;
        }

        public void ProcessMessage(string s) {
            try {
                // process message from arduino here
                chart.Series["value"].Points.AddY(Convert.ToDouble(s));
            }
            catch { 
                // message cannot be procesed
            }
        }
    }
}

