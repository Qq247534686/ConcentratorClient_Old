using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// 打开记事本
    /// </summary>
    public class NotePadUtil
    {
        #region 记事本API
        /// <summary>
        /// 传递消息给记事本
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);

        /// <summary>
        /// 查找句柄
        /// </summary>
        /// <param name="hwndParent"></param>
        /// <param name="hwndChildAfter"></param>
        /// <param name="lpszClass"></param>
        /// <param name="lpszWindow"></param>
        /// <returns></returns>
        [DllImport("User32.DLL")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// 记事本需要的常量
        /// </summary>
        public const uint WM_SETTEXT = 0x000C;

        #endregion
        /// <summary>
        /// 不创建文件打开记事本显示自定义内容
        /// </summary>
        /// <param name="txtValue">内容</param>
        public static void OpenNotepad(string txtValue)
        {
            System.Diagnostics.Process Proc;
            try
            {
                // 新建进程
                Proc = new System.Diagnostics.Process();
                Proc.StartInfo.FileName = "notepad.exe";
                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardInput = true;
                Proc.StartInfo.RedirectStandardOutput = true;
                Proc.Start();
            }
            catch (Exception msg)
            {
                Proc = null;
               Log.LogWrite(msg);
            }
            if (Proc != null)
            {
                // 调用 API, 传递数据
                while (Proc.MainWindowHandle == IntPtr.Zero)
                {
                    Proc.Refresh();
                }
                IntPtr vHandle = FindWindowEx(Proc.MainWindowHandle, IntPtr.Zero, "Edit", null);
                // 传递数据给记事本
                SendMessage(vHandle, WM_SETTEXT, 0, txtValue);
            }

        }
        /// <summary>
        /// 使用记事本打开指定路径所在的文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void OpenNotepadFile(string filePath)
        {
            Process.Start("notepad", filePath);
        }
    }
}
