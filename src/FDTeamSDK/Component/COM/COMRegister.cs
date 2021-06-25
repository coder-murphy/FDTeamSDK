using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace FDSDK.Component
{
    /// <summary>
    /// COM组件注册器
    /// </summary>
    public class COMRegister : IFDCustomComponent
    {
        /// <summary>
        /// 注册一个组件
        /// </summary>
        public bool RegistCOM(string path)
        {
            string strcmd = string.Format(regsvr32Str, path);
            if(ExecuteCommand(strcmd))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 反注册组件
        /// </summary>
        public bool UnRegister(string file)
        {
            // 断言检查
            Debug.Assert(string.IsNullOrEmpty(file) == false);
            string fileFullName = @"\"+ file + @"\";
            // 检查方法,查找注册表是否存在标识符
            Process process = Process.Start("regsvr32",fileFullName + "/s /u");
            if (process != null && process.HasExited)
            {
                int exitcode = process.ExitCode;
                if (exitcode == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断这个COM组件是否注册过
        /// </summary>
        public bool IsCOMRegistered(string clsid)
        {
            // 断言检查
            Debug.Assert(string.IsNullOrEmpty(clsid) == false);
            bool result = false;
            string key = string.Format(@"CLSID\{0}", clsid);
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(key);
            if (regKey != null) result = true;
            return result;
        }



        private string regsvr32Str = "regsvr32 -s{0}";

        private bool ExecuteCommand(string strCmd)
        {
            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = true,
                Arguments = "/c" + strCmd
            };
            Process myProcess = new Process
            {
                StartInfo = info
            };
            bool res = myProcess.Start();
            myProcess.Close();
            return res;
        }
    }
}
