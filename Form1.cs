using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO.Ports;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
            Form1.check1 = this.checkBox1;
            Form1.check2 = this.checkBox2;
        }

        static CheckBox check1;
        static CheckBox check2;


        private void timer1_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("a");
        }

        private SerialPort mySerialPort;


        private void button1_Click(object sender, EventArgs e)
        {
            mySerialPort = new SerialPort(comboBox1.SelectedItem.ToString());

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            mySerialPort.Open();

            button1.Enabled = false;
        }

        private static void DataReceivedHandler(
                    object sender,
                    SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            if (Form1.check1.Checked)
            {
                indata = Regex.Replace(indata, @"[^\u0000-\u007F|\r|\n]", string.Empty);
            }
            if (Form1.check2.Checked)
            {
                indata = indata.Replace("\r", "");
                indata = indata.Replace("\n", "");
            }
            SendKeys.SendWait(indata);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mySerialPort != null && mySerialPort.IsOpen)
            {
                mySerialPort.Close();
            }
        }
    }
}
