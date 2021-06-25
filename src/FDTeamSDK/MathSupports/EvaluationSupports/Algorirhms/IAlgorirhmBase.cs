using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.EvaluationSupports.Algorirhms
{
    /// <summary>
    /// 统计方法接口
    /// </summary>
    public interface IAnalsisBase<T0, T1> : IXHSDKEvalAlgorirhm
        where T0 : class
        where T1 : class
    {
        /// <summary>
        /// 统计算法输入接口
        /// </summary>
        void Import(T0 type);

        /// <summary>
        /// 统计算法输出接口
        /// </summary>
        T1 Export();

        /// <summary>
        /// 运行算法耗费时间
        /// </summary>
        /// <returns></returns>
        double ExpendTime();

        /// <summary>
        /// 读取配置
        /// </summary>
        void ImportConfig(AlgorithmConfig configs);
    }
}
