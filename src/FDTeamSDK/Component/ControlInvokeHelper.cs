using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FDSDK.Component
{
    /// <summary>
    /// 控件助手
    /// </summary>
    public class ControlInvokeHelper : IFDCustomComponent
    {
        #region Invoke
        /// <summary>
        /// 延迟执行委托
        /// </summary>
        public void RunWaitInvoke(Action action, Control srcControl, int millsecond)
        {
            Task task = new Task(() =>
            {
                Wait(millsecond);
                srcControl.Invoke(action);
                action();
            });
            task.Start();
        }

        /// <summary>
        /// 延迟执行委托
        /// </summary>
        public TResult RunWaitInvoke<TResult>(Func<TResult> func, Control srcControl, int millsecond)
            where TResult : class
        {
            Task<TResult> task = new Task<TResult>(() =>
            {
                GenericDelegate<TResult> act = new GenericDelegate<TResult>(() => { return func(); });
                Wait(millsecond);
                var iResult = srcControl.BeginInvoke(act, null);
                return srcControl.EndInvoke(iResult) as TResult;
            });
            return task.Result;
        }

        /// <summary>
        /// 延迟执行委托
        /// </summary>
        public TResult RunWaitInvoke<TParam, TResult>(Func<TParam, TResult> func, Control srcControl, int millsecond, TParam param)
            where TResult : class
        {
            Task<TResult> task = new Task<TResult>(() =>
            {
                GenericDelegate<TResult> act = new GenericDelegate<TResult>(() => { return func(param); });
                Wait(millsecond);
                var iResult = srcControl.BeginInvoke(act, null);
                return srcControl.EndInvoke(iResult) as TResult;
            });
            return task.Result;
        }
        #endregion

        /// <summary>
        /// 线程暂停
        /// </summary>
        public void Wait(int millseconds)
        {
            Thread.Sleep(millseconds);
        }

        /// <summary>
        /// 泛型委托
        /// </summary>
        /// <returns></returns>
        private delegate T GenericDelegate<T>();
        /// <summary>
        /// 常规委托
        /// </summary>
        private delegate void NormalDelegate();
    }
}
