using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.EventSupports
{
    /// <summary>
    /// 事件组件泛型接口
    /// </summary>
    public interface IFDEventComponent<T> : IFDCustomComponent
    {
        /// <summary>
        /// 事件响应
        /// </summary>
        event FDCustomEventHandler<T> EventTrigger;
    }

    /// <summary>
    /// 事件组件非泛型接口
    /// </summary>
    public interface IFDEventComponent : IFDCustomComponent
    {
        /// <summary>
        /// 事件响应
        /// </summary>
        event FDCustomEventHandler EventTrigger;
    }

    /// <summary>
    /// 支持泛型自定义事件对象
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void FDCustomEventHandler<T>(object sender, IFDEventArgs<T> args);

    /// <summary>
    /// 不支持泛型的自定义事件对象
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void FDCustomEventHandler(object sender, IFDEventArgs<object> args);
}
