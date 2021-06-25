using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.GenericSupports.Base
{
    /// <summary>
    /// 泛型行索引器接口
    /// </summary>
    public interface IGenericLineIndexer : IFDSDKObjectBase
    {
        /// <summary>
        /// 根据索引访问对象成员
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        object this[int index] { get; set; }
    }

    /// <summary>
    /// 泛型行接口(2属性)
    /// </summary>
    public interface IGenericLine<T0, T1>
    {
        /// <summary>
        /// 泛型行的第1个属性
        /// </summary>
        T0 Param0 { get; set; }
        /// <summary>
        /// 泛型行的第2个属性
        /// </summary>
        T1 Param1 { get; set; }
    }

    /// <summary>
    /// 泛型行接口(3属性)
    /// </summary>
    public interface IGenericLine<T0, T1, T2>
    {
        /// <summary>
        /// 泛型行的第1个属性
        /// </summary>
        T0 Param0 { get; set; }
        /// <summary>
        /// 泛型行的第2个属性
        /// </summary>
        T1 Param1 { get; set; }
        /// <summary>
        /// 泛型行的第3个属性
        /// </summary>
        T2 Param2 { get; set; }
    }

    /// <summary>
    /// 泛型行接口(4属性)
    /// </summary>
    public interface IGenericLine<T0, T1, T2, T3>
    {
        /// <summary>
        /// 泛型行的第1个属性
        /// </summary>
        T0 Param0 { get; set; }
        /// <summary>
        /// 泛型行的第2个属性
        /// </summary>
        T1 Param1 { get; set; }
        /// <summary>
        /// 泛型行的第3个属性
        /// </summary>
        T2 Param2 { get; set; }
        /// <summary>
        /// 泛型行的第3个属性
        /// </summary>
        T3 Param3 { get; set; }
    }

    /// <summary>
    /// 泛型行接口(5属性)
    /// </summary>
    public interface IGenericLine<T0, T1, T2, T3, T4> 
    {
        /// <summary>
        /// 泛型行的第1个属性
        /// </summary>
        T0 Param0 { get; set; }
        /// <summary>
        /// 泛型行的第2个属性
        /// </summary>
        T1 Param1 { get; set; }
        /// <summary>
        /// 泛型行的第3个属性
        /// </summary>
        T2 Param2 { get; set; }
        /// <summary>
        /// 泛型行的第4个属性
        /// </summary>
        T3 Param3 { get; set; }
        /// <summary>
        /// 泛型行的第5个属性
        /// </summary>
        T4 Param4 { get; set; }
    }
}
