using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.Component
{
    /// <summary>
    /// 常规单例模式
    /// </summary>
    public class SingletonNormal<T> : IDisposable, IFDCustomComponent where T : class,new()
    {
        /// <summary>
        /// 该类的唯一实例
        /// </summary>
        protected static T Instance = null;

        /// <summary>
        /// 获取该类的唯一实例
        /// </summary>
        public static T GetInstance()
        {
            if (Instance == null)
                Instance = new T();
            return Instance;
        }

        /// <summary>
        /// 释放该对象
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 可重写销毁过程中方法
        /// </summary>
        protected virtual void Dispose(bool isDisposing)
        {
            if (IsDisposed) return;
            if(isDisposing)
            {
                // TODO:被销毁中执行的操作
            }
            _isDisposed = true;
        }

        private bool _isDisposed = false;

        /// <summary>
        /// 对象是否被GC释放
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public SingletonNormal()
        {
            SingleProtect();
        }

        /// <summary>
        /// 针对泛型公共构造的单例保护机制
        /// </summary>
        protected void SingleProtect()
        {
            if (Instance != null && IsDisposed == false)
            {
                Console.WriteLine("单例模式禁止创建多个实例");
                this.Dispose();
            }
                
        }
    }
}
