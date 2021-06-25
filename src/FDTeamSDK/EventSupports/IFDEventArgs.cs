using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.EventSupports
{
    /// <summary>
    /// 事件接口
    /// </summary>
    /// <typeparam name="TItem">事件携带对象类型</typeparam>
    public interface IFDEventArgs<TItem> : IFDSDKObjectBase
    {
        /// <summary>
        /// 事件携带对象
        /// </summary>
        TItem Item { get; set; }

        /// <summary>
        /// 事件携带对象类型
        /// </summary>
        Enum ItemType { get; set; }

        /// <summary>
        /// 用户定义的事件操作类型
        /// </summary>
        Enum OperatorType { get; set; }
    }
}
