using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.EventSupports
{
    /// <summary>
    /// 不支持单例模式的基础事件类
    /// </summary>
    public class FDEventBasicComponent<T> : IFDEventComponent<T>
    {
        /// <summary>
        /// 自定义事件被触发
        /// </summary>
        public event FDCustomEventHandler<T> EventTrigger = null;

        /// <summary>
        /// 发送触发消息（触发事件用 可重写）
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemType"></param>
        /// <param name="opType"></param>
        protected virtual void SendAlertMessage(T item, Enum itemType = null, Enum opType = null)
        {
            if(EventTrigger != null)
            {
                IFDEventArgs<T> args = new FDCustomEventArgs<T>();
                args.Item = item;
                args.ItemType = itemType;
                args.OperatorType = opType;
                EventTrigger(this, args);
            }
        }
    }
}
