using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
namespace WindowsFormsApp1
{
    // Delegate[] delegates = d1.GetInvocationList(); // 多播委托，dl是委托列表
    public delegate void ConnectionPLCDelegate(string _sNameIP); 
    public delegate void WriteDataPLCDelegate(string _address, string _data);
    public delegate string ReadDataPLCDelegate(string _address);
    public partial class CommunicationPLCWindow : Form
    {
        CommunicationPLC m_objCommPLC;
        string m_sNamePLC;
        string m_sNameIP;
        string m_sNamePort;
        public CommunicationPLCWindow(CommunicationPLC _objCommPLC)
        {
            // PLC连接
            m_objCommPLC = _objCommPLC;
            InitializeComponent();
            comboBox1.Items.Add("S7-s1500");
            comboBox1.Items.Add("S7-s1200");
            comboBox1.Items.Add("S7-s200");
            comboBox1.Items.Add("S7-s200 smart");
            comboBox1.Items.Add("ModbusNet");
            // 数据读取
            comboBox2.Items.Add("bool");
            comboBox2.Items.Add("byte");
            comboBox2.Items.Add("int");
            comboBox2.Items.Add("float");
            comboBox2.Items.Add("double");
            // 数据写入
            comboBox3.Items.Add("bool");
            comboBox3.Items.Add("byte");
            comboBox3.Items.Add("int");
            comboBox3.Items.Add("float");
            comboBox3.Items.Add("double");
        }
        public void ConnectPLCs(string _sNameIP, ConnectionPLCDelegate _connectionPLC)
        {
            _connectionPLC(_sNameIP);
        }
        public void WriteDataPLC(string _address, string _data, WriteDataPLCDelegate _writeDataPLCDelegate)
        {
            _writeDataPLCDelegate(_address, _data);
        }
        /*
        public byte ReadDataPLC(string _address, ReadDataPLCDelegate _readDataPLCDelegate)
        {
            byte data = _readDataPLCDelegate(_address);
            return data;
        }
        */
        private void button1_Click(object sender, EventArgs e)
        {
            m_sNamePLC = comboBox1.Text;
            m_sNameIP = textBox1.Text;
            m_sNamePort = textBox2.Text;
            if("" == m_sNamePLC)
            {
                MessageBox.Show("请选择PLC型号！");
                return;
            }
            if ("" == m_sNameIP)
            {
                MessageBox.Show("请输入IP地址！");
                return;
            }
            if ("" == m_sNamePort)
            {
                MessageBox.Show("请输入端口号！");
                return;
            }
            switch (m_sNamePLC)
            {
                case "S7-s1500":
                    ConnectPLCs(m_sNameIP, m_objCommPLC.ConnectSiemens1500);
                    m_objCommPLC.OpenConnectionSiemensS7();
                    break;
                case "S7-s1200":
                    ConnectPLCs(m_sNameIP, m_objCommPLC.ConnectSiemens1200);
                    m_objCommPLC.OpenConnectionSiemensS7();
                    break;
                case "S7-s200":
                    ConnectPLCs(m_sNameIP, m_objCommPLC.ConnectSiemens200);
                    m_objCommPLC.OpenConnectionSiemensS7();
                    break;
                case "S7-s200 smart":
                    ConnectPLCs(m_sNameIP, m_objCommPLC.ConnectSiemens200Smart);
                    m_objCommPLC.OpenConnectionSiemensS7();
                    break;
                case "ModbusNet":
                    ConnectPLCs(m_sNameIP, m_objCommPLC.ConnectModBus);
                    m_objCommPLC.OpenConnectionModbus();
                    break;
            }
            if(!m_objCommPLC.GetConnectionStatue())
            {
                MessageBox.Show("PLC连接失败！");
            }
            else
            {
                MessageBox.Show("PLC连接成功！");
            }
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            m_objCommPLC.CloseConnectionSiemensS7();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_objCommPLC.WriteIntModbus("10", 123);
        }

        private void button4_Click(object sender, EventArgs e)
        {
           int ss = m_objCommPLC.ReadIntModbus("10");
            int a = 11;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string address = textBox3.Text;
            string data = textBox4.Text;
            string typeData = comboBox2.Text;
            switch (typeData)
            {
                case "bool":
                    if (m_sNamePLC.Contains("S7"))
                    {
                        m_objCommPLC.WriteBoolSiemensS7(address, bool.Parse(data));
                    }
                    else if (m_sNamePLC.Contains("ModbusNet"))
                    {
                        m_objCommPLC.WriteBoolModbus(address, bool.Parse(data));
                    }
                    break;
                case "byte":
                    if (m_sNamePLC.Contains("S7"))
                    {
                        m_objCommPLC.WriteByteSiemensS7(address, byte.Parse(data));
                    }
                    else if(m_sNamePLC.Contains("ModbusNet"))
                    {
                        m_objCommPLC.WriteIntModbus(address, int.Parse(data));
                    }
                    
                    break;
                case "int":
                    if (m_sNamePLC.Contains("S7"))
                    {
                        m_objCommPLC.WriteIntSiemensS7(address, int.Parse(data));
                    }
                    else if (m_sNamePLC.Contains("ModbusNet"))
                    {
                        m_objCommPLC.WriteIntModbus(address, int.Parse(data));
                    }
                    break;
                case "float":
                    if (m_sNamePLC.Contains("S7"))
                    {
                        m_objCommPLC.WriteFloatSiemensS7(address, float.Parse(data));
                    }
                    else if (m_sNamePLC.Contains("ModbusNet"))
                    {
                        m_objCommPLC.WriteFloatModbus(address, float.Parse(data));
                    }
                    break;
                case "double":
                    if (m_sNamePLC.Contains("S7"))
                    {
                        m_objCommPLC.WriteDoubleSiemensS7(address, double.Parse(data));
                    }
                    else if (m_sNamePLC.Contains("ModbusNet"))
                    {
                        m_objCommPLC.WriteDoubleModbus(address, double.Parse(data));
                    }
                    break;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string address = textBox6.Text;
            
            string typeData = comboBox3.Text;
            switch (typeData)
            {
                case "bool":
                    if (m_sNamePLC.Contains("S7"))
                    {
                        bool data = m_objCommPLC.ReadBoolSiemensS7(address);
                        textBox5.Text = data.ToString();
                    }
                    else if (m_sNamePLC.Contains("ModbusNet"))
                    {
                        bool data = m_objCommPLC.ReadBoolModbus(address);
                        textBox5.Text = data.ToString();
                    }
                    break;
                case "byte":

                    if (m_sNamePLC.Contains("S7"))
                    {
                        byte data = m_objCommPLC.ReadByteSiemensS7(address);
                        textBox5.Text = data.ToString();
                    }
                    else if (m_sNamePLC.Contains("ModbusNet"))
                    {
                        int data = m_objCommPLC.ReadIntModbus(address);
                        textBox5.Text = data.ToString();
                    }
                     
                    break;
                case "int":
                    if (m_sNamePLC.Contains("S7"))
                    {
                        int data = m_objCommPLC.ReadIntSiemensS7(address);
                        textBox5.Text = data.ToString();
                    }
                    else if (m_sNamePLC.Contains("ModbusNet"))
                    {
                        int data = m_objCommPLC.ReadIntModbus(address);
                        textBox5.Text = data.ToString();
                    }
                    break;
                case "float":
                    if (m_sNamePLC.Contains("S7"))
                    {
                        float data = m_objCommPLC.ReadFloatSiemensS7(address);
                        textBox5.Text = data.ToString();
                    }
                    else if (m_sNamePLC.Contains("ModbusNet"))
                    {
                        float data = m_objCommPLC.ReadFloatModbus(address);
                        textBox5.Text = data.ToString();
                    }
                    break;
                case "double":
                    if (m_sNamePLC.Contains("S7"))
                    {
                        double data = m_objCommPLC.ReadDoubleSiemensS7(address);
                        textBox5.Text = data.ToString();
                    }
                    else if (m_sNamePLC.Contains("ModbusNet"))
                    {
                        double data = m_objCommPLC.ReadDoubleModbus(address);
                        textBox5.Text = data.ToString();
                    }
                    break;
            }
        }
    }
}
