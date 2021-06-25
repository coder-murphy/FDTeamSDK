using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.EventSupports
{
    /// <summary>
    /// 自定义事件数据
    /// </summary>
    public class FDCustomEventArgs<T> : EventArgs, IFDEventArgs<T>
    {
        /// <summary>
        /// 事件携带对象
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// 用户定义的事件携带对象类型
        /// </summary>
        public Enum ItemType { get; set; }

        /// <summary>
        /// 用户定义的事件操作类型
        /// </summary>
        public Enum OperatorType { get; set; }

        /// <summary>
        /// 创建一个事件对象
        /// </summary>
        /// <returns></returns>
        public static FDCustomEventArgs<T> CreateEvent(T item, Enum enuItemType, Enum opType)
        {
            FDCustomEventArgs<T> ev = new FDCustomEventArgs<T>();
            ev.Item = item;
            ev.ItemType = enuItemType;
            ev.OperatorType = opType;
            return ev;
        }
    }
}
