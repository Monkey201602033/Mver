using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GxIAPINET;
using OpenCvSharp;
using System.Drawing;
using System.Drawing.Imaging;
namespace WindowsFormsApp1
{
    public delegate void SendImageDelegate(Mat _image, int _idPictureBox);
    public delegate void SendCaptureStatueDelegate(bool _bCaptureImage, string _typeCamera);
    class ImageProcessing
    {
        CommunicationPLC m_objCommPLC;
        DHCameraCtrl m_objDHCamera;
        bool m_bCaptureImage = false;
        
        public ImageProcessing()
        {
            m_objCommPLC = CommunicationPLC.CreateInstance();   // PLC通信为单例模式
            m_objDHCamera = new DHCameraCtrl();
            
        }
        public event SendImageDelegate CameraSendImageEvent;    // 定义事件
        public event SendCaptureStatueDelegate CaptureStatueEvent;
        public void CameraProcessDH(object objUserParam, IFrameData objIFrameData)
        {
            m_objCommPLC.WriteByteSiemensS7("DB100.11", 11);
            m_objCommPLC.WriteByteSiemensS7("DB100.10", 10);
            // 图像处理
            int width = (int)objIFrameData.GetWidth();
            int height = (int)objIFrameData.GetHeight();
            Mat src = m_objDHCamera.IFrameData2Mat(objIFrameData, width, height);
            Mat dst = src.Clone();
            Cv2.Threshold(src, dst, 150, 255, ThresholdTypes.Binary);
            CameraSendImageEvent(src, 1);          // 给主程序发送图片
            CameraSendImageEvent(dst, 2);
            m_objCommPLC.WriteByteSiemensS7("DB100.12", 11);
            
            CaptureStatueEvent(true, "DH");
        }
        public void CameraProcessBasler(Bitmap bmp)
        {
            m_objCommPLC.WriteByteSiemensS7("DB10.11", 11);
            m_objCommPLC.WriteByteSiemensS7("DB10.10", 10);
            // 图像处理
            OpenCvSharp.Mat mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp);
            CameraSendImageEvent(mat, 3);          // 给主程序发送图片
            CameraSendImageEvent(mat, 4);
            m_objCommPLC.WriteByteSiemensS7("DB10.12", 11);

            CaptureStatueEvent(true, "Basler");
        }
        public void CameraProcessTwo(object objUserParam, IFrameData objIFrameData)
        {

        }
        public void CameraProcessThree(object objUserParam, IFrameData objIFrameData)
        {

        }
        public void CameraProcessFour(object objUserParam, IFrameData objIFrameData)
        {

        }
    }
}
