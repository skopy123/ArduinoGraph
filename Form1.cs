using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;

namespace ArduinoGraph {

    public partial class Form1 : Form {

        ThreadedSerialPort tsp;
        StringBuilder Log;
        DataTransformTool dtt;

        public Form1() {
            InitializeComponent();
            for (int i = 1; i < 10; i++) {
                comboBox1.Items.Add("COM"+i.ToString());
            }
            comboBox1.SelectedIndex = 0;
            chart1.Series.Clear();
            Log = new StringBuilder();
            dtt = new BMP180Example(chart1);
        }

        private void AddToLog(string s) {
            Log.AppendLine(s);
            textBox1.Text = Log.ToString();
        }


        private void button1_Click(object sender, EventArgs e) {
            if (tsp == null){
                tsp = new ThreadedSerialPort((string)comboBox1.SelectedItem,9600);
                tsp.OnLineRecieved += OnLineRecievedEventHandler;
            }
            if(!tsp.IsOpen) {
                if (tsp.Open()) {
                    btnOpenCom.Text = "Close COM";
                    AddToLog("com open");
                }
            }
            else{
                tsp.Close();
                comboBox1.Enabled = true;
                AddToLog("com closed");
                btnOpenCom.Text = "Open COM";
            }
        }

        // event OnLineRecived raised by communication thread, this EventHandler also run in communication thread. UI components can not be accessed from this eventhandler.
        private void OnLineRecievedEventHandler(string s){
            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate { ProcessMessage(s); }); // run ProcessMessage function in UI thread
            }
            else {
                ProcessMessage(s);
            }
        }

        //This function run in UI thread, you can manipulate with UI commponents in this function
        private void ProcessMessage(string s) {
            AddToLog(s);
            dtt.ProcessMessage(s);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (tsp != null) {
                tsp.Close();
            }
        }

     
    }

}
