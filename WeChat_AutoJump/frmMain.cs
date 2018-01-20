using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using WeChat_AutoJump.library;


namespace WeChat_AutoJump
{
    public partial class frmMain : Form
    {
        private string screencapPath = @"autojump.png";
        private double ratio;
        private enum Status
        {
            Start,Stop
        }

        private Status status = Status.Start;

        public frmMain()
        {
            InitializeComponent();
            imgTimer.Interval = 5000;
            imgTimer.AutoReset = true;
            imgTimer.Elapsed += new ElapsedEventHandler(imgTimer_Tick);
            KeyDown += new KeyEventHandler(frmMain_KeyDown);
            ratio = Convert.ToDouble(this.tbxRatio.Text);
        }

        #region Button Click Event

        private void btnAuto_Click(object sender, EventArgs e)
        {
            imgTimer.Start();
            btnAuto.Enabled = false;
            btnStep.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            ADBOperate adbOperate = new ADBOperate();
            adbOperate.ScreenCap();
            Point targetpoint = new Point();
            double distance = 0, duration;
            ImageProcessor imgprocess = new ImageProcessor();
            imgprocess.ImageScan(screencapPath, ref targetpoint, ref distance);
            if (btnStep.Text == "分析(&A)")
            {
                picBox.Image = RenderPicture(screencapPath, targetpoint).ToBitmap();
                btnStep.Text = "跳跃(&A)";
            }
            else
            {
                duration = distance * ratio > 200 ? distance * ratio : 200;
                adbOperate.SwipeScreen(targetpoint, duration);
                btnStep.Text = "分析(&A)";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            status = Status.Stop;
            btnAuto.Enabled = true;
            btnStep.Enabled = true;
            btnStop.Enabled = false;
        }

        #endregion

        /// <summary>
        /// 设备连接状态判定
        /// </summary>
        private void timerStatus_Tick(object sender, EventArgs e)
        {
            ADBOperate adbo = new ADBOperate();
            if (adbo.DeviceStatus())
            {
                lblStatus.Text = "状态：已就绪";
                timerStatus.Interval = 2000;
            }
            else
            {
                lblStatus.Text = "状态：未就绪";
                timerStatus.Interval = 1000;
            }
        }

        /// <summary>
        /// 渲染图片函数
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="p">目标点坐标</param>
        /// <returns>渲染后图像</returns>
        private Image<Rgb, Byte> RenderPicture(string sourcePath,Point p)
        {
            ImageProcessor imgrender = new ImageProcessor();
            Image<Rgb, Byte> img = imgrender.SetROIArea(sourcePath);
            MCvScalar scalar = new MCvScalar(255);
            Rectangle rect = new Rectangle(p.X, p.Y, 3, 3);
            CvInvoke.Rectangle(img, rect, scalar, 5, LineType.EightConnected);
            return img;
        }

        /// <summary>
        /// 连续跳跃定时器
        /// </summary>
        private System.Timers.Timer imgTimer = new System.Timers.Timer();
        private void imgTimer_Tick(object sender, ElapsedEventArgs e)
        {
            ADBOperate adbOperate = new ADBOperate();
            adbOperate.ScreenCap();
            Point targetpoint = new Point();
            double distance = 0, duration;
            ImageProcessor imgprocess = new ImageProcessor();
            imgprocess.ImageScan(screencapPath, ref targetpoint, ref distance);
            picBox.Image = this.RenderPicture(screencapPath, targetpoint).ToBitmap();
            duration = distance * ratio > 200 ? distance * ratio : 200;
            adbOperate.SwipeScreen(targetpoint, duration);
            if (status == Status.Stop)
            {
                status = Status.Start;
                imgTimer.Stop();
            }
        }

        /// <summary>
        /// 按键事件
        /// </summary>
        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                btnStep_Click(this, EventArgs.Empty);
            }
        }
    }
}
