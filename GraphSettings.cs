using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace ArduinoGraph {
    public abstract class DataTransformTool {
        protected Chart chart;

        public DataTransformTool(Chart outputChart) {
            chart = outputChart;
        }

        public abstract void ProcessMessage(string s);
    }
}
