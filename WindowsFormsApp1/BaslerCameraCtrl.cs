using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;
using System.Drawing;
using System.Drawing.Imaging;
namespace WindowsFormsApp1
{
    class BaslerCameraCtrl
    {
        //相机连接的个数
        List<ICameraInfo> m_allCameras = null;
        Dictionary<string, Camera> m_mapCameras = new Dictionary<string, Camera>();
        //委托+事件 = 回调函数，用于传递相机抓取的图像
        public delegate void CameraImage(Bitmap bmp);
        public event CameraImage CameraImageEvent;

        //basler里用于将相机采集的图像转换成位图
        PixelDataConverter pxConvert = new PixelDataConverter();

        //控制相机采集图像的过程
        bool GrabOver = false;

        public int GetNumCamera()
        {
            int CameraNumber = CameraFinder.Enumerate().Count;
            return CameraNumber;
        }
        //相机初始化
        public int CameraInit()
        {
            m_allCameras = CameraFinder.Enumerate();
            for(int i = 0; i < m_allCameras.Count; i++)
            {
                Camera camera = new Camera(m_allCameras[i]);
                
                //自由运行模式
                camera.CameraOpened += Configuration.AcquireContinuous;

                //断开连接事件
                camera.ConnectionLost += Camera_ConnectionLost;

                //抓取开始事件
                camera.StreamGrabber.GrabStarted += StreamGrabber_GrabStarted;

                //添加抓取图片事件
                camera.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;

                //结束抓取事件
                camera.StreamGrabber.GrabStopped += StreamGrabber_GrabStopped;

                //打开相机
                camera.Open();
                string ss = m_allCameras[i][CameraInfoKey.SerialNumber];
                m_mapCameras.Add(m_allCameras[i][CameraInfoKey.SerialNumber], camera);
                
            }
            
            return m_allCameras.Count;
        }
        private void StreamGrabber_GrabStarted(object sender, EventArgs e)
        {
            GrabOver = true;
        }
        public void ImageGrabbedEvent(string _modelName)
        {
            if (m_mapCameras[_modelName] != null)
            {
                m_mapCameras[_modelName].StreamGrabber.GrabStarted += StreamGrabber_GrabStarted;
            }
        }
        private void StreamGrabber_ImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            IGrabResult grabResult = e.GrabResult;
            if (grabResult.IsValid)
            {
                if (GrabOver)
                    CameraImageEvent(GrabResult2Bmp(grabResult));
            }
        }

        private void StreamGrabber_GrabStopped(object sender, GrabStopEventArgs e)
        {
            GrabOver = false;
        }

        private void Camera_ConnectionLost(object sender, EventArgs e)
        {
            // 一个相机断开，所有都销毁，后面改成对应相机销毁
            foreach (var item in m_mapCameras)
            {
                item.Value.StreamGrabber.Stop();
                DestroyCamera(item.Key);
            }    
        }
        /// <summary>
        /// 触发相机拍照
        /// </summary>
        public void OneShot(string _modelName)
        {
            if (m_mapCameras[_modelName] != null)
            {
                if (!m_mapCameras[_modelName].StreamGrabber.IsGrabbing)  // 防止当前正在采集，再次触发报错
                {
                    m_mapCameras[_modelName].Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame);
                    m_mapCameras[_modelName].StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                }
            }
        }

        public void KeepShot(string _modelName)
        {
            if (m_mapCameras[_modelName] != null)
            {
                m_mapCameras[_modelName].Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                m_mapCameras[_modelName].StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
        }

        public void Stop(string _modelName)
        {
            if (m_mapCameras[_modelName] != null)
            {
                m_mapCameras[_modelName].StreamGrabber.Stop();
            }
        }

        //将相机抓取到的图像转换成Bitmap位图
        Bitmap GrabResult2Bmp(IGrabResult grabResult)
        {
            Bitmap b = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
            BitmapData bmpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, b.PixelFormat);
            pxConvert.OutputPixelFormat = PixelType.BGRA8packed;
            IntPtr bmpIntpr = bmpData.Scan0;
            pxConvert.Convert(bmpIntpr, bmpData.Stride * b.Height, grabResult);
            b.UnlockBits(bmpData);
            return b;
        }

        //释放相机
        public void DestroyCamera(string _modelName)
        {
            if (m_mapCameras[_modelName] != null)
            {
                m_mapCameras[_modelName].Close();
                m_mapCameras[_modelName].Dispose();
                m_mapCameras[_modelName] = null;
            }
        }
        public int setExposureTime(string _modelName, long ExposureTimeNum)//设置曝光时间us
        {
            try
            {
                m_mapCameras[_modelName].Parameters[PLCamera.ExposureTimeAbs].SetValue(ExposureTimeNum);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

    }
}

