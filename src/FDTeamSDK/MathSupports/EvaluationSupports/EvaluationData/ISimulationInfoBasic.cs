using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.EvaluationSupports
{
    /// <summary>
    /// 仿真结果基本信息的接口
    /// </summary>
    public interface ISimulationInfoBasic : IXHSDKEvaluationObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        double Value { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        double Time { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        string Source { get; set; }
    }
}
