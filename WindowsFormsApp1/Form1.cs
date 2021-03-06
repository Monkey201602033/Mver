using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvCamCtrl.NET;
using System.Threading;
using System.Windows.Forms;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using WindowsFormsApp1;
using OpenCvSharp;
using GxIAPINET;
using System.Threading;
namespace WindowsFormsApp1
{
    delegate void myDelegateShowText(string ss);
    public partial class Form1 : Form
    {                                 
        CommunicationPLC m_objCommPLC;                     // PLC通信
        DHCameraCtrls m_objDHCameras;
        BaslerCameraCtrl m_objBaslerCamera;
        ImageProcessing m_objImageProcessing;
     
        bool m_bHeartBeat = true;                          // 心跳
        int m_nNumCamera;
        string[] m_sDeviceName = new string[10];
        bool m_bCaptureImageDH = true;
        bool m_bCaptureImageBasler = true;
        string m_appPath = System.Windows.Forms.Application.StartupPath;
        public Form1()
        {
            InitializeComponent();

            m_objCommPLC = CommunicationPLC.CreateInstance();                                    // 通信初始化

            // 图像处理初始化
            m_objImageProcessing = new ImageProcessing();                                
            m_objImageProcessing.CameraSendImageEvent += PictureBoxShowEvent;                   // 注册显示图像事件
            m_objImageProcessing.CaptureStatueEvent += GetCaptureStatueEvent;                   // 图像采集完成事件 

            // basler相机初始化
            m_objBaslerCamera = new BaslerCameraCtrl();
            m_nNumCamera = m_objBaslerCamera.CameraInit();                                    
            m_objBaslerCamera.CameraImageEvent += m_objImageProcessing.CameraProcessBasler;     // basler相机响应事件，所有相机事件触发同一事件函数
            
            // 大恒相机初始化
            m_objDHCameras = new DHCameraCtrls();
            m_objDHCameras.opencamera();
            InitCameras();                                                                      // 初始化所有相机，将事件图像处理事件加入委托

            // 状态栏初始化
            InitCameraStatue(m_nNumCamera);
            InitPLCStatue(0);
            ChangeStateBarColor(false, toolStripStatusLabel8);
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
        public void InitCameras()
        {
            m_objDHCameras.RegisterCallBackCaptureImage(m_objImageProcessing.CameraProcessDH);  // 注册相机回调
        }
      
        void AutoRun()
        {
            int i = 0;
            while (true)
            {
                string ss = Convert.ToString(i) + "\n";
                //this.Invoke(new Action(() =>
                //{ SetString2RichTextBox(ss); }
                //));
                i++;
                byte byteCaptureImageBasler = m_objCommPLC.ReadByteSiemensS7("DB10.22");
                
                byte byteCaptureImageDH = m_objCommPLC.ReadByteSiemensS7("DB100.10");
                if (11 == byteCaptureImageDH)
                {
                    // 触发相机拍照
                    m_objDHCameras.CaptureImage();
                    m_bCaptureImageDH = false;
                }
                
                if (11 == byteCaptureImageBasler)
                {
                    // 触发相机拍照,使用map和配置文件实现具体哪个信号对应哪个相机
                    m_objBaslerCamera.OneShot("21163847");
                    m_bCaptureImageBasler = false;
                }
                Thread.Sleep(50); // 每秒读取一次
            }
        }
        private void 自动运行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_objBaslerCamera.setExposureTime("21163847", 1000);
            m_objDHCameras.SetExposureTime(1000);
            bool statue = m_objCommPLC.GetConnectionStatue();
            if (statue)
            {
                Thread t = new Thread(new ThreadStart(AutoRun));
                t.Start();
            }
            else
            {
                MessageBox.Show("PLC未连接！");
            }
        }

        private void BaslerEventInit(int _numCamera)
        {
            for(int i = 0; i < _numCamera; i++)
            {

            }
        }

        private void PictureBoxShow(Mat _image, PictureBox objPictureBox)
        {
            Bitmap bitMap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_image);
            objPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            objPictureBox.Image = bitMap;
        }
        private void GetCaptureStatueEvent(bool _bCaptureImage, string _typeCamera)
        {
            switch(_typeCamera)
            {
                case "DH":
                    m_bCaptureImageDH = _bCaptureImage;
                    break;
                case "Basler":
                    m_bCaptureImageBasler = _bCaptureImage;
                    break;
            }
           
        }
        // 事件响应函数
        public void PictureBoxShowEvent(Mat _image, int _idPictureBox)
        {
            if (1== _idPictureBox)
            {
                PictureBoxShow(_image, this.pictureBox1);
            }
            else if(2 == _idPictureBox)
            {
                PictureBoxShow(_image, this.pictureBox2);
            }
            else if (3 == _idPictureBox)
            {
                PictureBoxShow(_image, this.pictureBox3);
            }
            else if (4 == _idPictureBox)
            {
                PictureBoxShow(_image, this.pictureBox4);
            }
        }
        private void ChangeStateBarColor(bool state, ToolStripStatusLabel toolStripStatusLabel)
        {
            if(state)
            {
                toolStripStatusLabel.Image = Image.FromFile(m_appPath + @"\green.png");
            }
            else
            {
                toolStripStatusLabel.Image = Image.FromFile(m_appPath + @"\red.png");
            }
            
        }
        private void ChangeRunStateColor(bool state, PictureBox objPictureBox)
        {
            string m_appPath = System.Windows.Forms.Application.StartupPath;
            if (state)
            {
                objPictureBox.Image = Image.FromFile(m_appPath + @"\OK.png");
            }
            else
            {
                objPictureBox.Image = Image.FromFile(m_appPath + @"\NG.png");
            }

        }
        private void SetString2RichTextBox(string ss)
        {
            this.richTextBox1.AppendText(ss);
        }
        private void InitCameraStatue(int nNum)
        {
            if(1 == nNum)
            {
                ChangeStateBarColor(true, toolStripStatusLabel1);
            }
            else if (2 == nNum)
            {
                ChangeStateBarColor(true, toolStripStatusLabel1);
                ChangeStateBarColor(true, toolStripStatusLabel2);
            }
            else if (3 == nNum)
            {
                ChangeStateBarColor(true, toolStripStatusLabel1);
                ChangeStateBarColor(true, toolStripStatusLabel2);
                ChangeStateBarColor(true, toolStripStatusLabel3);
            }
            else if (4 == nNum)
            {
                ChangeStateBarColor(true, toolStripStatusLabel1);
                ChangeStateBarColor(true, toolStripStatusLabel2);
                ChangeStateBarColor(true, toolStripStatusLabel3);
                ChangeStateBarColor(true, toolStripStatusLabel4);
            }
        }
        private void InitPLCStatue(int nNum)
        {
            if (1 == nNum)
            {
                ChangeStateBarColor(true, toolStripStatusLabel5);
            }
            else if (2 == nNum)
            {
                ChangeStateBarColor(true, toolStripStatusLabel5);
                ChangeStateBarColor(true, toolStripStatusLabel6);
            }
            else if (3 == nNum)
            {
                ChangeStateBarColor(true, toolStripStatusLabel5);
                ChangeStateBarColor(true, toolStripStatusLabel6);
                ChangeStateBarColor(true, toolStripStatusLabel7);
            }
            else if (4 == nNum)
            {
                ChangeStateBarColor(true, toolStripStatusLabel5);
                ChangeStateBarColor(true, toolStripStatusLabel6);
                ChangeStateBarColor(true, toolStripStatusLabel7);
                ChangeStateBarColor(true, toolStripStatusLabel9);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommunicationPLCWindow objCommunicationPLC = new CommunicationPLCWindow(m_objCommPLC);
            objCommunicationPLC.SendPLCStateEvent += InitPLCStatue;
            objCommunicationPLC.Show();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            /*
            this.pictureBox1.Height = this.Height / 2 - 5;
            this.pictureBox2.Height = this.Height / 2 - 5;
            this.pictureBox3.Height = this.Height / 2 - 5;
            this.pictureBox4.Height = this.Height / 2 - 5;
            this.panel1.Height = this.Height - 5;
            this.panel2.Height = this.Height - 5;
            this.panel1.Width = this.Width / 3 * 2 - 5;
            this.panel2.Width = this.Width / 3 - 5;
            */
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void 点标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NinePointCalibration objNinePointCalibration = new NinePointCalibration();
            objNinePointCalibration.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if(m_bHeartBeat)
            {
                m_objCommPLC.WriteByteSiemensS7("DB100.13", 10);
                toolStripStatusLabel8.Image = Image.FromFile(m_appPath + @"\red.png");
                m_bHeartBeat = false;
            }
            else
            {
                byte data = m_objCommPLC.ReadByteSiemensS7("DB100.13");
                if(11 == data)
                {
                    toolStripStatusLabel8.Image = Image.FromFile(m_appPath + @"\green.png");
                    m_bHeartBeat = true;
                }
                
            }
            // Thread.Sleep(1000);
        }

        private void toolStripStatusLabel8_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel10_Click(object sender, EventArgs e)
        {

        }

        private void 图像分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageAnalysis objImageAnalysis = new ImageAnalysis();
            objImageAnalysis.Show();
        }

        private void 相机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 触发ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_objCommPLC.WriteByteSiemensS7("DB10.22", 11);
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_objCommPLC.WriteByteSiemensS7("DB10.22", 10);
        }



        /*
        static void ImageCallbackCam1(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            Console.WriteLine("Get one frame: Width[" + Convert.ToString(pFrameInfo.nWidth) + "] , Height[" + Convert.ToString(pFrameInfo.nHeight)
                                + "] , FrameNum[" + Convert.ToString(pFrameInfo.nFrameNum) + "]");
        }
        static void ImageCallbackCam2(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            Console.WriteLine("Get one frame: Width[" + Convert.ToString(pFrameInfo.nWidth) + "] , Height[" + Convert.ToString(pFrameInfo.nHeight)
                                + "] , FrameNum[" + Convert.ToString(pFrameInfo.nFrameNum) + "]");
        }
        */
    }
}
