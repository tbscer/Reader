using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Xml.Linq;

namespace Reader
{
    public partial class Form1 : Form
    {
        private string _ConfigFile;
        private SerialPort _LKG500;

        private SerialPort _Scanner;

        public SerialPort BarcodeReader
        {
            get { return _Scanner; }
            set { _Scanner = value; }
        }


        public SerialPort LaserReader
        {
            get { return _LKG500; }
            set { _LKG500 = value; }
        }


        public Form1()
        {
            InitializeComponent();

            string x = "sss\n";

            _ConfigFile = Path.GetDirectoryName(Application.ExecutablePath);
            _ConfigFile = Path.Combine(_ConfigFile, "Configuration.xml");
        }

        private void Init()
        {
            _LKG500 = new SerialPort();
            _Scanner = new SerialPort();

            if (!File.Exists(_ConfigFile))
            {
                return;
            }

            using (FileStream fs = File.OpenRead(_ConfigFile))
            {
                XDocument doc = XDocument.Load(fs);

                XElement lk = doc.Descendants("comport").
                    Where(x => x.Attribute("name").Value == "LK-G5000").FirstOrDefault();
                if (lk != null)
                {
                    LoadPortSetting(_LKG500, lk);
                }

                XElement sc = doc.Descendants("comport").
                    Where(x => x.Attribute("name").Value == "Scanner").FirstOrDefault();
                if (sc != null)
                {
                    LoadPortSetting(_Scanner, sc);
                }

                fs.Close();
            }

            _LKG500.DataReceived += new SerialDataReceivedEventHandler(_LKG500_DataReceived);
            _Scanner.DataReceived += new SerialDataReceivedEventHandler(_Scanner_DataReceived);
            try
            {
                _LKG500.Open();
                _Scanner.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Try to open com port error:" + ex.Message);
            }
        }

        private void Save()
        {
            XDocument doc = XDocument.Load(_ConfigFile);
            var lk = doc.Descendants("comport");
            foreach (var item in lk)
            {
                if (item.Attribute("name").Value == "LK-G5000")
                {
                    SavePortSetting(_LKG500, item);
                }
                else
                {
                    SavePortSetting(_Scanner, item);
                }
            }

            doc.Save(_ConfigFile);
        }

        private void LoadPortSetting(SerialPort port, XElement setting)
        {
            port.PortName = setting.Attribute("port").Value;
            port.BaudRate = int.Parse(setting.Attribute("baudrate").Value);
            port.Parity = (Parity)Enum.Parse(typeof(Parity), setting.Attribute("parity").Value);
            port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), setting.Attribute("stopbits").Value);
            port.Handshake = (Handshake)Enum.Parse(typeof(Handshake), setting.Attribute("handshake").Value);
            port.DataBits = int.Parse(setting.Attribute("databits").Value);
        }

        private void SavePortSetting(SerialPort port, XElement setting)
        {
            setting.Attribute("port").Value = port.PortName;
            setting.Attribute("baudrate").Value = port.BaudRate.ToString();
            setting.Attribute("parity").Value = port.Parity.ToString();
            setting.Attribute("stopbits").Value = port.StopBits.ToString();
            setting.Attribute("handshake").Value = port.Handshake.ToString();
            setting.Attribute("databits").Value = port.DataBits.ToString();
        }

        private void lKG5000ComportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComSettings cs = new ComSettings();
            cs.Port = _LKG500;

            if (cs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void barcodeComportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        void _Scanner_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

        }

        void _LKG500_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof)
            {
                return;
            }

            string text = _LKG500.ReadExisting();
        }


        /*
        存储周期不是“同步输入”时
            1. 使用“数据存储设定”命令指定存储点编号与存储周期。 (第 5-22 页)
            2. 使用“存储 (OUT)”命令指定要存储的数据的目标 OUT。 (第 5-21 页)
            3. 使用“数据存储开始”命令开始数据存储。 (第 5-12 页)
            4. 使用“数据存储停止”命令停止数据存储。 (第 5-12 页)
            5. 使用“数据存储/数据输出”命令输出存储的数据。 (第 5-12 页)
         */
    }
}
