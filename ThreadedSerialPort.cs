using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;


namespace ArduinoGraph {
    public class ThreadedSerialPort {
        public ThreadedSerialPort(string portName, int baudRate) {
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.ReadTimeout = 500;
            serialPort.NewLine = "\r\n";
        }

        public bool Open() {
            if (!serialPort.IsOpen) {
                serialPort.Open();
            }
            if (serialPort.IsOpen) {
                comThread = new Thread(this.ReadLoop);
                comThread.Start();
                return true;
            }
            return false;
        }

        public void Close() {
            serialPort.Close();
            comThread.Abort();
        }

        public bool IsOpen { get { return serialPort.IsOpen; } }

        public delegate void LineRecieved(string s);
        public event LineRecieved OnLineRecieved;

        private Thread comThread;
        private SerialPort serialPort;

        // this function should run in communication thread. if it run in own thread (other than UI thread) UI will not freeze during waiting on data recive;  
        private void ReadLoop() {
            string s = "";
            while (serialPort.IsOpen) {
                try {
                    s = serialPort.ReadLine();
                }
                catch (TimeoutException e) {
                    continue;
                }
                catch (System.IO.IOException e) {
                    break;
                }
                OnLineRecieved(s);
            }
        }
    }

}
