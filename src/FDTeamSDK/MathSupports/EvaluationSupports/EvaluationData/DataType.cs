using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.EvaluationSupports
{
    /// <summary>
    /// 单个指标数据
    /// </summary>
    public class SimuData<TName, TValue>
    {
        public TName Name { get; set; }
        public TValue Value { get; set; }
    }

    /// <summary>
    /// 单个指标数据
    /// </summary>
    public class SimuData<TName, TWeight, TValue>
    {
        public TName Name { get; set; }
        public TWeight Weight { get; set; }
        public TValue Value { get; set; }
    }

    /// <summary>
    /// 单个指标数据
    /// </summary>
    public class SimuData<TClass, TName, TWeight, TValue>
    {
        public TClass Class { get; set; }
        public TName Name { get; set; }
        public TWeight Weight { get; set; }
        public TValue Value { get; set; }
    }

    /// <summary>
    /// 仿真单个数据
    /// </summary>
    public class SimuDataSimple
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 最终权重（经过分层计算得到）
        /// </summary>
        public double ActualWeight { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public double Time { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
    }
}
