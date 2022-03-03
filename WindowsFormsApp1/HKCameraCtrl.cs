using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvCamCtrl.NET;
using OpenCvSharp;
using System.Runtime.InteropServices;
namespace WindowsFormsApp1
{
    class HKCameraCtrl
    {
        public static MyCamera.cbOutputExdelegate ImageCallback;
        void ImageCallbackCam1(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            Console.WriteLine("Get one frame: Width[" + Convert.ToString(pFrameInfo.nWidth) + "] , Height[" + Convert.ToString(pFrameInfo.nHeight)
                                + "] , FrameNum[" + Convert.ToString(pFrameInfo.nFrameNum) + "]");
        }
        void ImageCallbackCam2(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            Console.WriteLine("Get one frame: Width[" + Convert.ToString(pFrameInfo.nWidth) + "] , Height[" + Convert.ToString(pFrameInfo.nHeight)
                                + "] , FrameNum[" + Convert.ToString(pFrameInfo.nFrameNum) + "]");
        }
        MyCamera.MV_CC_DEVICE_INFO_LIST m_stDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();   // 相机列表
        private MyCamera m_MyCamera = new MyCamera();
        MyCamera.MV_FRAME_OUT_INFO_EX m_stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();
        /// <summary>
        /// 枚举设备
        /// </summary>
        /// <returns></returns>
        public int EnumDevices(string[] sDeviceName)
        {
            m_stDeviceList.nDeviceNum = 0;
            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_stDeviceList);
            if (0 != nRet)
            {
                return -1;  // 枚举设备失败
            }
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)   // 网口相机
                {
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    if (gigeInfo.chUserDefinedName != "")
                    {
                        // cbDeviceList.Items.Add("GEV: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")");
                        sDeviceName[i] = "";  // 获得相机名称
                    }
                    else
                    {
                        // cbDeviceList.Items.Add("GEV: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                        sDeviceName[i] = "";  // 获得相机名称
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)     // USB相机
                {
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        // cbDeviceList.Items.Add("U3V: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")");
                        sDeviceName[i] = "";  // 获得相机名称
                    }
                    else
                    {
                        // cbDeviceList.Items.Add("U3V: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")");
                        sDeviceName[i] = "";  // 获得相机名称
                    }
                }
            }
                
            return (int)m_stDeviceList.nDeviceNum;
        }

        public int OpenDevice(int nCam)   // 打开的相机序号
        {
            if (m_stDeviceList.nDeviceNum == 0)
            {
                return -1;
            }
            MyCamera.MV_CC_DEVICE_INFO device =
                (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[nCam],
                                                              typeof(MyCamera.MV_CC_DEVICE_INFO));
            // ch:打开设备 | en:Open device
            if (null == m_MyCamera)    // 设备为空，再打开一次
            {
                m_MyCamera = new MyCamera();
                if (null == m_MyCamera)
                {
                    return -1;
                }
            }

            int nRet = m_MyCamera.MV_CC_CreateDevice_NET(ref device);
            if (MyCamera.MV_OK != nRet)
            {
                return -1;
            }

            nRet = m_MyCamera.MV_CC_OpenDevice_NET();
            if (MyCamera.MV_OK != nRet)
            {
                m_MyCamera.MV_CC_DestroyDevice_NET();
                return -1;
            }

            // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
            if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
            {
                int nPacketSize = m_MyCamera.MV_CC_GetOptimalPacketSize_NET();
                if (nPacketSize > 0)
                {
                    nRet = m_MyCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                    if (nRet != MyCamera.MV_OK)
                    {
                        // ShowErrorMsg("Set Packet Size failed!", nRet);
                    }
                }
                else
                {
                    // ShowErrorMsg("Get Packet Size failed!", nPacketSize);
                }
            }
            // 1. 内触发（OFF）：连续采集，单帧采集
            // 2. 外触发（ON）：软触发，硬触发
            m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);   // 软触发
            m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);               // 外触发


            // ch:注册回调函数 | en:Register image callback
            if(1 == nCam)
            {
                ImageCallback = new MyCamera.cbOutputExdelegate(ImageCallbackCam1);
            }
            else
            {
                ImageCallback = new MyCamera.cbOutputExdelegate(ImageCallbackCam2);
            }
            nRet = m_MyCamera.MV_CC_RegisterImageCallBackEx_NET(ImageCallback, IntPtr.Zero);
            if (MyCamera.MV_OK != nRet)
            {
                Console.WriteLine("Register image callback failed!");
                return -1;
            }

            return 0;
        }
        public void CloseDevice()
        {
            m_MyCamera.MV_CC_CloseDevice_NET();
            m_MyCamera.MV_CC_DestroyDevice_NET();
        }

        public int CaptureImage(Mat _image)
        {
            // ch:触发命令 | en:Trigger command
            int nRet = m_MyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
            if (MyCamera.MV_OK != nRet)
            {
                return -1;
            }
            return 0;
        }

    }
}
