using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
namespace WindowsFormsApp1
{
    class IniFileIO
    {
        public IniFileIO()
        {

        }

        // 单例模式    
        private static IniFileIO m_IniFileIO = null;
        private static object Singleton_Lock = new object();
        public static IniFileIO CreateInstance()
        {
            // 双锁
            if (m_IniFileIO == null)
            {
                lock (Singleton_Lock)
                {
                    if (m_IniFileIO == null)
                    {
                        m_IniFileIO = new IniFileIO();
                    }
                }
            }
            return m_IniFileIO;
        }
        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);


        #endregion

        #region 读Ini文件

        public  string ReadIniData(string Section, string Key, string iniFilePath)
        {
            iniFilePath = "D:\\winformProj\\WindowsFormsApp1\\WindowsFormsApp1\\bin\\Debug\\ImageProcessIni\\CameraOne.ini";
            //if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, "", temp, 1024, iniFilePath);
                return temp.ToString();
            }
            //else
            {
                //return String.Empty;
            }
        }

        #endregion

        #region 写Ini文件

        public  bool WriteIniData(string Section, string Key, string Value, string iniFilePath)
        {
            iniFilePath = "D:\\winformProj\\WindowsFormsApp1\\WindowsFormsApp1\\bin\\Debug\\ImageProcessIni\\CameraOne.ini";
            //if (File.Exists(iniFilePath))
            {
                long OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //else
            {
                //return false;
            }
        }

        #endregion
    }
}

