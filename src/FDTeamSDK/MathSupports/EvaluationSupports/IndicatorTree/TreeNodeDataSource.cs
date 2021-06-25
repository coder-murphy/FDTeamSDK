using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.EvaluationSupports
{
    /// <summary>
    /// 节点数据源
    /// </summary>
    public class TreeNodeDataSource : IXHSDKEvaluationObject
    {

        /// <summary>
        /// 算子类型
        /// </summary>
        public OperatorType Operator = OperatorType.Default;

        /// <summary>
        /// 数据源信息类别选项
        /// </summary>
        public int DataSourceMsgType { get; set; }

        /// <summary>
        /// 数据源选项
        /// </summary>
        public int DataSourceIndex { get; set; }

        /// <summary>
        /// 数据选取选项
        /// </summary>
        public int DataSourceAmountMode { get; set; }
    }

    /// <summary>
    /// 算子类型
    /// </summary>
    public enum OperatorType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 单值
        /// </summary>
        SingleValue,
        /// <summary>
        /// 求和
        /// </summary>
        Sum,
        /// <summary>
        /// 连乘
        /// </summary>
        Multipy,
        /// <summary>
        /// 计数
        /// </summary>
        Count,
        /// <summary>
        /// 均值
        /// </summary>
        Average,
        /// <summary>
        /// 极差
        /// </summary>
        Range,
        /// <summary>
        /// 方差
        /// </summary>
        Variance,
        /// <summary>
        /// 标准差
        /// </summary>
        StandardDeviation,
        /// <summary>
        /// 占总比
        /// </summary>
        MainRatio,
    }
}
