using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DBLib.Windows
{
    public class WinHelper
    {

        /// <summary>
        /// winapi 用于找到句柄线程ID,即PID
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int processID);

        /// <summary>
        /// 根据进程ID杀死进程
        /// </summary>
        /// <param name="processID"></param>
        /// <returns></returns>
        public static bool KillProcessByPID(int processID)
        {
            try
            {
                Process p = Process.GetProcessById(processID);
                p.Kill();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 根据[进程名称]结束进程
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool KillProcessByName(string name)
        {
            try
            {
                Process[] ps = Process.GetProcesses();
                foreach (Process item in ps)
                {
                    if (item.ProcessName.ToLower() == name.ToLower())
                    {
                        item.Kill();
                    }
                }
                return true;
            }
            catch { return false; }
        }
    }
}
