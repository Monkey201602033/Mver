using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using OpenCvSharp;
namespace WindowsFormsApp1
{
    public partial class ImageAnalysis : Form
    {
        Mat m_ShowImage = new Mat();
        bool m_PressCtrl = false;
        double m_scale = 1.2;
        public ImageAnalysis()
        {
            InitializeComponent();
            this.pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {

        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            double scale = 1.2;
            if (m_scale < 6.0 && e.Delta > 0)
            {
                m_scale = m_scale * scale;
            }
            else if(m_scale > 0.1 && e.Delta < 0)
            {
                m_scale = m_scale / scale;
            }

            Mat dst = m_ShowImage.Clone();
            if (e.Delta > 0 && m_PressCtrl)
            {
                //OpenCvSharp.Size size = new OpenCvSharp.Size(m_ShowImage.Rows * step, m_ShowImage.Cols * step);
                OpenCvSharp.Size size = new OpenCvSharp.Size(0, 0);
                double step = m_scale;
                if (m_scale > 1.0)
                {
                    step = System.Math.Round(m_scale);
                }
                Cv2.Resize(m_ShowImage, dst, size, step, step, InterpolationFlags.Nearest);
                Cv2.ImWrite("D:\\1.png", dst);
                pictureBox1.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(dst);
            }
            else if(e.Delta < 0 && m_PressCtrl)
            {
                OpenCvSharp.Size size = new OpenCvSharp.Size(0, 0);
                double step = m_scale;
                if (m_scale > 1.0)
                {
                    step = System.Math.Round(m_scale);
                }
                Cv2.Resize(m_ShowImage, dst, size, step, step, InterpolationFlags.Nearest);
                pictureBox1.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(dst);
            }
        }
        public static Bitmap FileToBitmap(string fileName)
        {
            try
            {
                // 打开文件  
                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                // 读取文件的 byte[]  
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
                // 把 byte[] 转换成 Stream  
                Stream stream = new MemoryStream(bytes);

                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始  
                stream.Seek(0, SeekOrigin.Begin);

                MemoryStream mstream = null;
                try
                {
                    mstream = new MemoryStream(bytes);
                    return new Bitmap((Image)new Bitmap(stream));
                }
                catch (ArgumentNullException ex)
                {
                    return null;
                }
                catch (ArgumentException ex)
                {
                    return null;
                }
                finally
                {
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private void 打开图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title =
                "请选择要插入的图片";
                ofd.Filter =
                    "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|" +
                    "Windows Bitmap(*.bmp)|*.bmp|" +
                    "Windows Icon(*.ico)|*.ico|" +
                    "Graphics Interchange Format (*.gif)|(*.gif)|" +
                    "JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|" +
                    "Portable Network Graphics (*.png)|*.png|" +
                    "Tag Image File Format (*.tif)|*.tif;*.tiff";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    m_ShowImage = Cv2.ImRead(ofd.FileName, ImreadModes.AnyColor);
                    pictureBox1.Image = FileToBitmap(ofd.FileName);
                }
            }
        }

        private void ImageAnalysis_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control)
            {
                m_PressCtrl = true;
            }
            else
            {
                m_PressCtrl = false;
            }
            
        }

        private void 打开图片ToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseHover_1(object sender, EventArgs e)
        {

        }
        private void ResizeNew(Mat _src, Mat _dst, double _scale)
        {
            int scale = (int)_scale;
            OpenCvSharp.Size size= new OpenCvSharp.Size(_src.Cols * _scale, _src.Rows * _scale);
            Mat dst = new Mat(size, MatType.CV_8U);
            for (int nHeight = 0; nHeight < _src.Rows; nHeight++)
            {
                for (int nWidth = 0; nWidth < _src.Cols; nWidth++)
                {
                    byte p = _src.At<byte>(nHeight, nWidth);

                    for (int i = 0; i < scale; i++)
                    {
                        for (int j = 0; j < scale; j++)
                        {
                            _dst.At<byte>(nHeight * scale + i, nWidth * scale + j) = p;
                        }
                    }
                }
            }
            _dst = dst.Clone();
        }
    }
}
