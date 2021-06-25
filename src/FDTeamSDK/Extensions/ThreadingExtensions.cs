using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace FDSDK.Extensions
{
    /// <summary>
    /// 线程拓展方法
    /// </summary>
    public static class ThreadingExtensions
    {
        /// <summary>
        /// 延时线程创建(秒为单位)
        /// </summary>
        /// <param name="work"></param>
        /// <param name="delay"></param>
        public static void RunDelayThread(Action work, float delay = 0f)
        {
            var mainTask = new Task(() =>
            {
                var task = new Task(work);
                delay = delay <= 0f ? 0f : delay;
                Thread.Sleep((int)(delay * 1000));
                task.Start();
            });
            mainTask.Start();
        }
    }
}
