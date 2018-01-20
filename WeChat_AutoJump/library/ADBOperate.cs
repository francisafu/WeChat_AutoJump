using System;
using System.Diagnostics;
using System.Drawing;

namespace WeChat_AutoJump.library
{
    /// <summary>
    /// ADB操作类
    /// </summary>
    class ADBOperate
    {
        /// <summary>
        /// 类初始化
        /// </summary>
        private Process adbProcess;
        public ADBOperate()
        {
            adbProcess = new Process();
            string fileDirectory = Environment.CurrentDirectory + "\\ADB\\adb.exe";
            adbProcess.StartInfo.UseShellExecute = false;
            adbProcess.StartInfo.CreateNoWindow = true;
            adbProcess.StartInfo.FileName = fileDirectory;
            adbProcess.StartInfo.RedirectStandardError = true;
            adbProcess.StartInfo.RedirectStandardInput = true;
            adbProcess.StartInfo.RedirectStandardOutput = true;
        }

        /// <summary>
        /// ADB Shell运行收到的命令
        /// </summary>
        /// <param name="command">命令内容</param>
        /// <returns>返回字符串</returns>
        private string AdbShell(string command)
        {
            adbProcess.StartInfo.Arguments = command;
            adbProcess.Start();
            string result = adbProcess.StandardOutput.ReadToEnd();
            adbProcess.Close();
            return result;
        }

        /// <summary>
        /// 截图命令
        /// </summary>
        public void ScreenCap()
        {
            AdbShell("shell screencap -p /sdcard/autojump.png");
            AdbShell("pull /sdcard/autojump.png .");
        }

        /// <summary>
        /// 滑屏命令
        /// </summary>
        /// <param name="p">触点坐标</param>
        /// <param name="duration">按压时间</param>
        public void SwipeScreen(Point p, double duration)
        {
            Random ran = new Random();
            AdbShell(string.Format("shell input swipe {0} {1} {0} {1} {2}",
                p.X + ran.Next(1, 10),
                p.Y + ran.Next(1, 10),
                Convert.ToInt32(duration)));
        }

        /// <summary>
        /// 获取设备连接状态
        /// </summary>
        /// <returns>布尔值是否连接</returns>
        public bool DeviceStatus()
        {
            string status = AdbShell("shell getprop ro.product.model");
            if (status.Contains("no device") || string.IsNullOrWhiteSpace(status))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
