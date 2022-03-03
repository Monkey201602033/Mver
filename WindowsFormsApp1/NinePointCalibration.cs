using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
namespace WindowsFormsApp1
{
    public partial class NinePointCalibration : Form
    {
        IniFileIO m_objIniFileIO;
        OpenCvSharp.Point2f pointPixel1 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointPixel2 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointPixel3 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointPixel4 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointPixel5 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointPixel6 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointPixel7 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointPixel8 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointPixel9 = new OpenCvSharp.Point2f();

        OpenCvSharp.Point2f pointMechanism1 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointMechanism2 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointMechanism3 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointMechanism4 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointMechanism5 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointMechanism6 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointMechanism7 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointMechanism8 = new OpenCvSharp.Point2f();
        OpenCvSharp.Point2f pointMechanism9 = new OpenCvSharp.Point2f();
        public NinePointCalibration()
        {
            m_objIniFileIO = IniFileIO.CreateInstance();
            
            InitializeComponent();
            ReadIni();
        }

        private Mat GetMatPiexl2Mechanism(List<OpenCvSharp.Point2f> _listPiexlPoints, List<OpenCvSharp.Point2f> _listMechanismPoints)
        {
            Mat convert = Cv2.GetAffineTransform(_listPiexlPoints, _listMechanismPoints);
            return convert;
        }
        private void ReadIni()
        {
            string iniFilePath = System.Windows.Forms.Application.StartupPath + "\\ImageProcessIni\\CameraOne.ini";
            pointPixel1.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel1X", iniFilePath));
            pointPixel2.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel1Y", iniFilePath));
            pointPixel2.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel2X", iniFilePath));
            pointPixel2.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel2Y", iniFilePath));
            pointPixel3.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel3X", iniFilePath));
            pointPixel3.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel3Y", iniFilePath));
            pointPixel4.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel4X", iniFilePath));
            pointPixel4.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel4Y", iniFilePath));
            pointPixel5.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel5X", iniFilePath));
            pointPixel5.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel5Y", iniFilePath));
            pointPixel6.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel6X", iniFilePath));
            pointPixel6.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel6Y", iniFilePath));
            pointPixel7.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel7X", iniFilePath));
            pointPixel7.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel7Y", iniFilePath));
            pointPixel8.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel8X", iniFilePath));
            pointPixel8.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel8Y", iniFilePath));
            pointPixel9.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel9X", iniFilePath));
            pointPixel9.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointPixel9Y", iniFilePath));

            pointMechanism1.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism1X", iniFilePath));
            pointMechanism2.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism1Y", iniFilePath));
            pointMechanism2.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism2X", iniFilePath));
            pointMechanism2.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism2Y", iniFilePath));
            pointMechanism3.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism3X", iniFilePath));
            pointMechanism3.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism3Y", iniFilePath));
            pointMechanism4.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism4X", iniFilePath));
            pointMechanism4.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism4Y", iniFilePath));
            pointMechanism5.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism5X", iniFilePath));
            pointMechanism5.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism5Y", iniFilePath));
            pointMechanism6.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism6X", iniFilePath));
            pointMechanism6.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism6Y", iniFilePath));
            pointMechanism7.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism7X", iniFilePath));
            pointMechanism7.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism7Y", iniFilePath));
            pointMechanism8.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism8X", iniFilePath));
            pointMechanism8.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism8Y", iniFilePath));
            pointMechanism9.X = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism9X", iniFilePath));
            pointMechanism9.Y = Convert.ToSingle(m_objIniFileIO.ReadIniData("NineCalibration", "pointMechanism9Y", iniFilePath));

            this.textBox1.Text = pointPixel1.X.ToString();
            this.textBox2.Text = pointPixel1.Y.ToString();
            this.textBox4.Text = pointPixel2.X.ToString();
            this.textBox3.Text = pointPixel2.Y.ToString();
            this.textBox6.Text = pointPixel3.X.ToString();
            this.textBox5.Text = pointPixel3.Y.ToString();
            this.textBox8.Text = pointPixel4.X.ToString();
            this.textBox7.Text = pointPixel4.Y.ToString();
            this.textBox10.Text = pointPixel5.X.ToString();
            this.textBox9.Text = pointPixel5.Y.ToString();
            this.textBox12.Text = pointPixel6.X.ToString();
            this.textBox11.Text = pointPixel6.Y.ToString();
            this.textBox14.Text = pointPixel7.X.ToString();
            this.textBox13.Text = pointPixel7.Y.ToString();
            this.textBox16.Text = pointPixel8.X.ToString();
            this.textBox15.Text = pointPixel8.Y.ToString();
            this.textBox45.Text = pointPixel9.X.ToString();
            this.textBox44.Text = pointPixel9.Y.ToString();

            this.textBox32.Text = pointMechanism1.X.ToString();
            this.textBox31.Text = pointMechanism1.Y.ToString();
            this.textBox30.Text = pointMechanism2.X.ToString();
            this.textBox29.Text = pointMechanism2.Y.ToString();
            this.textBox28.Text = pointMechanism3.X.ToString();
            this.textBox27.Text = pointMechanism3.Y.ToString();
            this.textBox26.Text = pointMechanism4.X.ToString();
            this.textBox25.Text = pointMechanism4.Y.ToString();
            this.textBox24.Text = pointMechanism5.X.ToString();
            this.textBox23.Text = pointMechanism5.Y.ToString();
            this.textBox22.Text = pointMechanism6.X.ToString();
            this.textBox21.Text = pointMechanism6.Y.ToString();
            this.textBox20.Text = pointMechanism7.X.ToString();
            this.textBox19.Text = pointMechanism7.Y.ToString();
            this.textBox18.Text = pointMechanism8.X.ToString();
            this.textBox17.Text = pointMechanism8.Y.ToString();
            this.textBox43.Text = pointMechanism9.X.ToString();
            this.textBox42.Text = pointMechanism9.Y.ToString();
        }
        private void WriteIni()
        {
            string iniFilePath = System.Windows.Forms.Application.StartupPath + "\\ImageProcessIni\\CameraOne.ini";
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel1X", pointPixel1.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel1Y", pointPixel1.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel2X", pointPixel2.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel2Y", pointPixel2.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel3X", pointPixel3.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel3Y", pointPixel3.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel4X", pointPixel4.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel4Y", pointPixel4.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel5X", pointPixel5.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel5Y", pointPixel5.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel6X", pointPixel6.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel6Y", pointPixel6.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel7X", pointPixel7.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel7Y", pointPixel7.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel8X", pointPixel8.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel8Y", pointPixel8.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel9X", pointPixel9.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointPixel9Y", pointPixel9.Y.ToString(), iniFilePath);

            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism1X", pointMechanism1.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism1Y", pointMechanism1.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism2X", pointMechanism2.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism2Y", pointMechanism2.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism3X", pointMechanism3.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism3Y", pointMechanism3.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism4X", pointMechanism4.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism4Y", pointMechanism4.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism5X", pointMechanism5.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism5Y", pointMechanism5.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism6X", pointMechanism6.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism6Y", pointMechanism6.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism7X", pointMechanism7.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism7Y", pointMechanism7.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism8X", pointMechanism8.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism8Y", pointMechanism8.Y.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism9X", pointMechanism9.X.ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "pointMechanism9Y", pointMechanism9.Y.ToString(), iniFilePath);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pointPixel1.X = Convert.ToSingle(this.textBox1.Text);
            pointPixel1.Y = Convert.ToSingle(this.textBox2.Text);
            pointPixel2.X = Convert.ToSingle(this.textBox4.Text);
            pointPixel2.Y = Convert.ToSingle(this.textBox3.Text);
            pointPixel3.X = Convert.ToSingle(this.textBox6.Text);
            pointPixel3.Y = Convert.ToSingle(this.textBox5.Text);
            pointPixel4.X = Convert.ToSingle(this.textBox8.Text);
            pointPixel4.Y = Convert.ToSingle(this.textBox7.Text);
            pointPixel5.X = Convert.ToSingle(this.textBox10.Text);
            pointPixel5.Y = Convert.ToSingle(this.textBox9.Text);
            pointPixel6.X = Convert.ToSingle(this.textBox12.Text);
            pointPixel6.Y = Convert.ToSingle(this.textBox11.Text);
            pointPixel7.X = Convert.ToSingle(this.textBox14.Text);
            pointPixel7.Y = Convert.ToSingle(this.textBox13.Text);
            pointPixel8.X = Convert.ToSingle(this.textBox16.Text);
            pointPixel8.Y = Convert.ToSingle(this.textBox15.Text);
            pointPixel9.X = Convert.ToSingle(this.textBox45.Text);
            pointPixel9.Y = Convert.ToSingle(this.textBox44.Text);

            pointMechanism1.X = Convert.ToSingle(this.textBox32.Text);
            pointMechanism1.Y = Convert.ToSingle(this.textBox31.Text);
            pointMechanism2.X = Convert.ToSingle(this.textBox30.Text);
            pointMechanism2.Y = Convert.ToSingle(this.textBox29.Text);
            pointMechanism3.X = Convert.ToSingle(this.textBox28.Text);
            pointMechanism3.Y = Convert.ToSingle(this.textBox27.Text);
            pointMechanism4.X = Convert.ToSingle(this.textBox26.Text);
            pointMechanism4.Y = Convert.ToSingle(this.textBox25.Text);
            pointMechanism5.X = Convert.ToSingle(this.textBox24.Text);
            pointMechanism5.Y = Convert.ToSingle(this.textBox23.Text);
            pointMechanism6.X = Convert.ToSingle(this.textBox22.Text);
            pointMechanism6.Y = Convert.ToSingle(this.textBox21.Text);
            pointMechanism7.X = Convert.ToSingle(this.textBox20.Text);
            pointMechanism7.Y = Convert.ToSingle(this.textBox19.Text);
            pointMechanism8.X = Convert.ToSingle(this.textBox18.Text);
            pointMechanism8.Y = Convert.ToSingle(this.textBox17.Text);
            pointMechanism9.X = Convert.ToSingle(this.textBox43.Text);
            pointMechanism9.Y = Convert.ToSingle(this.textBox42.Text);

            List<OpenCvSharp.Point2f> listPiexlPoints = new List<OpenCvSharp.Point2f>();
            List<OpenCvSharp.Point2f> listMechanismPoints = new List<OpenCvSharp.Point2f>();
            listPiexlPoints.Add(pointPixel1);
            listPiexlPoints.Add(pointPixel2);
            listPiexlPoints.Add(pointPixel3);
            listPiexlPoints.Add(pointPixel4);
            listPiexlPoints.Add(pointPixel5);
            listPiexlPoints.Add(pointPixel6);
            listPiexlPoints.Add(pointPixel7);
            listPiexlPoints.Add(pointPixel8);
            listPiexlPoints.Add(pointPixel9);

            listMechanismPoints.Add(pointMechanism1);
            listMechanismPoints.Add(pointMechanism2);
            listMechanismPoints.Add(pointMechanism3);
            listMechanismPoints.Add(pointMechanism4);
            listMechanismPoints.Add(pointMechanism5);
            listMechanismPoints.Add(pointMechanism6);
            listMechanismPoints.Add(pointMechanism7);
            listMechanismPoints.Add(pointMechanism8);
            listMechanismPoints.Add(pointMechanism9);

            Mat convertPixel2Mechanism = GetMatPiexl2Mechanism(listPiexlPoints, listMechanismPoints);
            this.textBox33.Text = convertPixel2Mechanism.Get<float>(0, 0).ToString();
            this.textBox34.Text = convertPixel2Mechanism.Get<float>(1, 0).ToString();
            this.textBox35.Text = convertPixel2Mechanism.Get<float>(2, 0).ToString();
            this.textBox38.Text = convertPixel2Mechanism.Get<float>(0, 1).ToString();
            this.textBox37.Text = convertPixel2Mechanism.Get<float>(1, 1).ToString();
            this.textBox36.Text = convertPixel2Mechanism.Get<float>(2, 1).ToString();
            this.textBox41.Text = convertPixel2Mechanism.Get<float>(0, 2).ToString();
            this.textBox40.Text = convertPixel2Mechanism.Get<float>(1, 2).ToString();
            this.textBox39.Text = convertPixel2Mechanism.Get<float>(2, 2).ToString();
            WriteIni();
            string iniFilePath = System.Windows.Forms.Application.StartupPath + "\\ImageProcessIni\\CameraOne.ini";
            m_objIniFileIO.WriteIniData("NineCalibration", "MatrixX0Y0", convertPixel2Mechanism.Get<float>(0, 0).ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "MatrixX1Y0", convertPixel2Mechanism.Get<float>(1, 0).ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "MatrixX2Y0", convertPixel2Mechanism.Get<float>(2, 0).ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "MatrixX0Y1", convertPixel2Mechanism.Get<float>(0, 1).ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "MatrixX1Y1", convertPixel2Mechanism.Get<float>(1, 1).ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "MatrixX2Y1", convertPixel2Mechanism.Get<float>(2, 1).ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "MatrixX0Y2", convertPixel2Mechanism.Get<float>(0, 2).ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "MatrixX1Y2", convertPixel2Mechanism.Get<float>(1, 2).ToString(), iniFilePath);
            m_objIniFileIO.WriteIniData("NineCalibration", "MatrixX2Y2", convertPixel2Mechanism.Get<float>(2, 2).ToString(), iniFilePath);
        }
    }
}
