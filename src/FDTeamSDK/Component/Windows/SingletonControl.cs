using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FDSDK.Component.Windows
{
    /// <summary>
    /// 控件单例
    /// </summary>
    public abstract class SingletonControl<T> : Control, IFDCustomComponent where T : class,new()
    {
        /// <summary>
        /// 该控件的实例
        /// </summary>
        protected static T Instance = null;

        /// <summary>
        /// 获取该控件的唯一实例
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            if (Instance == null)
                Instance = new T();
            return Instance;
        }

        /// <summary>
        /// 单例保护
        /// </summary>
        protected void SingleProtect()
        {
            this.Paint += (sender, e) =>
            {
                if (Instance != null && this.IsDisposed == false)
                    this.Dispose();
            };
        }
    }
}
