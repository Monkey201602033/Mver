using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using HslCommunication.ModBus;
namespace WindowsFormsApp1
{
    public  class CommunicationPLC
    {
        SiemensS7Net m_objSiemensS7;
        ModbusTcpNet m_objModbus;
        OperateResult m_connect;
        public CommunicationPLC()
        {

        }
        // 单例模式    
        private static CommunicationPLC m_CommunicationPLCSingleton = null;
        private static object Singleton_Lock = new object();
        public static CommunicationPLC CreateInstance()
        {
            // 双锁
            if (m_CommunicationPLCSingleton == null) 
            {
                lock (Singleton_Lock)
                {
                    Console.WriteLine("路过。");
                    if (m_CommunicationPLCSingleton == null)
                    {
                        Console.WriteLine("被创建。");
                    m_CommunicationPLCSingleton = new CommunicationPLC();
                    }
                }
            }
            return m_CommunicationPLCSingleton;
        }
        // 正常委托的函数写在类外，用于绑定委托，等待触发
        public void ConnectSiemens1500(string _sNameIP)
        {
            m_objSiemensS7 = new SiemensS7Net(SiemensPLCS.S1500, _sNameIP);
        }
        public void ConnectSiemens1200(string _sNameIP)
        {
            m_objSiemensS7 = new SiemensS7Net(SiemensPLCS.S1200, _sNameIP);
        }
        public void ConnectSiemens200(string _sNameIP)
        {
            m_objSiemensS7 = new SiemensS7Net(SiemensPLCS.S200, _sNameIP);
        }
        public void ConnectSiemens200Smart(string _sNameIP)
        {
            m_objSiemensS7 = new SiemensS7Net(SiemensPLCS.S200Smart, _sNameIP);
        }
        public void ConnectModBus(string _sNameIP)
        {
            m_objModbus = new ModbusTcpNet(_sNameIP, 502, 0x01);
        }
        public bool GetConnectionStatue()
        {
            if(null == m_connect || !m_connect.IsSuccess)
            {
                return false;
            }
            else
            {
                return m_connect.IsSuccess;
            }
            
        }
        public bool OpenConnectionSiemensS7()
        {
            m_connect = m_objSiemensS7.ConnectServer();    // PLC长连接
            if (m_connect.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void CloseConnectionSiemensS7()
        {
            // 断开连接，也就是关闭了长连接，如果再去请求数据，就变成了短连接,
            // 连接服务器，也可以放在窗口的Load方法中，一般建议使用长连接，速度更快，
            // 又是线程安全的（调用下面的方法就是使用了长连接，如果不连接直接读取数据，那就是短连接）
            if (null != m_objSiemensS7)
            {
                m_objSiemensS7.ConnectClose();
            }
        }
        public bool OpenConnectionModbus()
        {
            m_connect = m_objModbus.ConnectServer();    // PLC长连接
            if (m_connect.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void CloseConnectionModbus()
        {
            // 断开连接，也就是关闭了长连接，如果再去请求数据，就变成了短连接,
            // 连接服务器，也可以放在窗口的Load方法中，一般建议使用长连接，速度更快，
            // 又是线程安全的（调用下面的方法就是使用了长连接，如果不连接直接读取数据，那就是短连接）
            if (null != m_objModbus)
            {
                m_objModbus.ConnectClose();
            }
        }
        public bool WriteByteSiemensS7(string _address, byte _data)
        {
            if(null != m_objSiemensS7)
            {
                OperateResult result = m_objSiemensS7.Write(_address, _data);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        public bool WriteBoolSiemensS7(string _address, bool _data)
        {
            if (null != m_objSiemensS7)
            {
                OperateResult result = m_objSiemensS7.Write(_address, _data);
                if(result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool WriteIntSiemensS7(string _address, int _data)
        {
            if (null != m_objSiemensS7)
            {
                OperateResult result = m_objSiemensS7.Write(_address, _data);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool WriteFloatSiemensS7(string _address, float _data)
        {
            if (null != m_objSiemensS7)
            {
                OperateResult result = m_objSiemensS7.Write(_address, _data);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool WriteDoubleSiemensS7(string _address, double _data)
        {
            if (null != m_objSiemensS7)
            {
                OperateResult result = m_objSiemensS7.Write(_address, _data);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public byte ReadByteSiemensS7(string _address)
        {
            byte data = 0;
            if (null != m_objSiemensS7)
            {
                data = m_objSiemensS7.ReadByte(_address).Content;
            }
            return data;
        }
        public bool ReadBoolSiemensS7(string _address)
        {
            bool data = false;
            if (null != m_objSiemensS7)
            {
                data = m_objSiemensS7.ReadBool(_address).Content;
            }
            return data;
        }
        public int ReadIntSiemensS7(string _address)
        {
            int data = -1;
            if (null != m_objSiemensS7)
            {
                data = m_objSiemensS7.ReadInt32(_address).Content;
            }
            return data;
        }
        public float ReadFloatSiemensS7(string _address)
        {
            float data = -1;
            if (null != m_objSiemensS7)
            {
                data = m_objSiemensS7.ReadFloat(_address).Content;
            }
            return data;
        }
        public double ReadDoubleSiemensS7(string _address)
        {
            double data = -1;
            if (null != m_objSiemensS7)
            {
                data = m_objSiemensS7.ReadDouble(_address).Content;
            }
            return data;
        }
        public bool WriteIntModbus(string _address, int _data)
        {
            if(null != m_objModbus)
            {
                OperateResult result = m_objModbus.Write(_address, _data);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool WriteBoolModbus(string _address, bool _data)
        {
            if (null != m_objModbus)
            {
                OperateResult result = m_objModbus.Write(_address, _data);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool WriteFloatModbus(string _address, float _data)
        {
            if (null != m_objModbus)
            {
                OperateResult result = m_objModbus.Write(_address, _data);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool WriteDoubleModbus(string _address, double _data)
        {
            if (null != m_objModbus)
            {
                OperateResult result = m_objModbus.Write(_address, _data);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public int ReadIntModbus(string _address)
        {
            int data = 0;
            if (null != m_objModbus)
            {
                data = m_objModbus.ReadInt32(_address).Content;
            }
            return data;
        }
        public bool ReadBoolModbus(string _address)
        {
            bool data = false;
            if (null != m_objModbus)
            {
                data = m_objModbus.ReadBool(_address).Content;
            }
            return data;
        }
        public float ReadFloatModbus(string _address)
        {
            float data = 0;
            if (null != m_objModbus)
            {
                data = m_objModbus.ReadFloat(_address).Content;
            }
            return data;
        }
        public double ReadDoubleModbus(string _address)
        {
            double data = 0.0;
            if (null != m_objModbus)
            {
                data = m_objModbus.ReadDouble(_address).Content;
            }
            return data;
        }
    }
}
