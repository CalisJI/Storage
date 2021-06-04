using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using ModBus_RTU;


namespace MODBUS_Sample
{
    
    public partial class Form1 : Form
    {
        ModBus_RTU.ModBus_RS485 _modBus_RS485 = new ModBus_RTU.ModBus_RS485();
        private Timer Timer = new Timer();
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)

        {
           
            Timer.Interval = 100;
            Timer.Tick += Timer_Tick;
            string[] portName = SerialPort.GetPortNames();
            foreach (var name in portName)
            {
                comboBox2.Items.Add(name);
            }
            comboBox2.SelectedIndex = 0;
            Int32[] Baudrate = new Int32[] { 9600, 19200, 38400, 57600, 115200 };
            foreach (var item in Baudrate)
            {
                comboBox3.Items.Add(item);
            }
            comboBox3.SelectedIndex = 0;
            Parity[] parity = new Parity[] { };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Read_modbus.PerformClick();
        }

        private void Read_modbus_Click(object sender, EventArgs e)
        {
            short[] value = new short[Convert.ToUInt16(tb_Register.Text)] ;
           bool read = _modBus_RS485.SendFc3(Convert.ToByte(tb_salveID.Text),Convert.ToUInt16(tb_starAddress.Text),Convert.ToUInt16(tb_Register.Text),ref value);
            if (read) 
            {
                MethodInvoker inv = delegate
                {
                    textBox5.Text = string.Empty;
                    string donv = string.Empty;
                    //Int16 result = Convert.ToInt16(value[i]);
                    double chia = 0;
                    for (int i = 0; i < Convert.ToUInt16(tb_Register.Text); i++)
                    {
                        Int16 result = Convert.ToInt16(value[i]);
                        switch (i)
                        {
                            case 0:
                                chia = (double)result / (double)10;
                                donv = chia.ToString() + " kWh";
                                break;
                            case 2:
                                chia = (double)result / (double)100;
                                donv = chia.ToString() + " Volt";
                                break;
                            case 3:
                                chia = (double)result / (double)1000;
                                donv = chia.ToString() + " Ampe";
                                break;
                            case 5:
                                chia = (double)result / (double)1000;
                                donv = chia.ToString() + " kW";
                                break;
                            case 7:
                                chia = (double)result / (double)100;
                                donv = chia.ToString() + " Hz";
                                break;
                            default:
                                donv = "0";
                                break;
                                //textBox5.AppendText(result.ToString() + Environment.NewLine);
                             
                        }
                        textBox5.AppendText(donv.ToString() + Environment.NewLine);

                    }

                };this.Invoke(inv);
               // MessageBox.Show(_modBus_RS485.Modbus_status);
            }
           // else MessageBox.Show(_modBus_RS485.Modbus_status);
        }

        private void Connect_btn_Click(object sender, EventArgs e)
        {
            string portNam = comboBox2.Text;
            if (Connect_btn.Text == "Connect") 
            {
                bool Open = _modBus_RS485.Opened(portNam, 9600, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
                if (Open)
                {
                    Connect_btn.Text = "Disconnect";
                    Timer.Enabled = true;
                    Timer.Start();
                    MessageBox.Show(_modBus_RS485.Modbus_status);
                }
                else MessageBox.Show(_modBus_RS485.Modbus_status);
            }
            else 
            {
                bool Close = _modBus_RS485.Closed();
                if (Close) 
                {
                    Timer.Stop();
                    Timer.Enabled = false;
                    Connect_btn.Text = "Connect";
                    MessageBox.Show(_modBus_RS485.Modbus_status);
                }
                else 
                {
                    MessageBox.Show(_modBus_RS485.Modbus_status);
                }
               
            }
           
        }

        private void Write_modbus_Click(object sender, EventArgs e)
        {
            short[] value = new short[Convert.ToUInt16(tb_Register.Text)] ;
            for (int i = 0; i < Convert.ToUInt16(tb_Register.Text); i++)
            {
                value[i] = Convert.ToInt16(tb_message.Text);
            }
            bool read = _modBus_RS485.SendFc16(Convert.ToByte(tb_salveID.Text)
                , Convert.ToUInt16(tb_starAddress.Text)
                , Convert.ToUInt16(tb_Register.Text),  value);
            if (read)
            {
                
                //MethodInvoker inv = delegate
                //{
                //    textBox5.Text = string.Empty;
                //    for (int i = 0; i < Convert.ToUInt16(tb_Register.Text); i++)
                //    {
                //        Int16 result = Convert.ToInt16(value[i]);
                //        textBox5.AppendText(result.ToString() + Environment.NewLine);
                //    }
                //    //textBox5.Text = value.ToString();
                //}; this.Invoke(inv);
                MessageBox.Show(_modBus_RS485.Modbus_status);
            }
            else MessageBox.Show(_modBus_RS485.Modbus_status);
        }
    }
}
