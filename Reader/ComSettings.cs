using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Reader
{
    public partial class ComSettings : Form
    {
        private SerialPort _Port;

        public SerialPort Port
        {
            get { return _Port; }
            set 
            { 
                _Port = value;

                Init();
            }
        }


        public ComSettings()
        {
            InitializeComponent();

            comboBoxPorts.Items.Clear();
            comboBoxPorts.Items.AddRange(SerialPort.GetPortNames());
        }

        private void Init()
        {
            comboBoxPorts.Text = _Port.PortName;
            comboBoxStopbits.SelectedIndex = (int)_Port.StopBits;
            comboBoxBaudrate.Text = _Port.BaudRate.ToString();
            comboBoxParity.SelectedIndex = (int)_Port.Parity;
            comboBoxHandshake.SelectedIndex = (int)_Port.Handshake;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_Port.IsOpen)
                {
                    _Port.Close();
                }

                _Port.PortName = comboBoxPorts.Text;
                _Port.StopBits = (StopBits)comboBoxStopbits.SelectedIndex;
                _Port.BaudRate = int.Parse(comboBoxBaudrate.Text);
                _Port.Parity = (Parity)comboBoxParity.SelectedIndex;
                _Port.Handshake = (Handshake)comboBoxHandshake.SelectedIndex;

                _Port.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to update the setting to the port. The message:\n\n" + ex.Message, 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
