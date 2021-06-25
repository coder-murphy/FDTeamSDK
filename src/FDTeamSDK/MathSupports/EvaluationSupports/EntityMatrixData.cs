using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDSDK.MathSupports;
using FDSDK.MathSupports.EvaluationSupports.Algorirhms;

namespace FDSDK.MathSupports.EvaluationSupports
{

    /// <summary>
    /// 实体数值矩阵信息
    /// </summary>
    public class EntityMatrixData : IFDSDKMathObject
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 每一行的列名
        /// </summary>
        public List<string> ColumnNames { get; set; }

        /// <summary>
        /// 数值矩阵
        /// </summary>
        public FloatMatrix Matrix { get; set; }

        /// <summary>
        /// 统计算法配置
        /// </summary>
        public AlgorithmConfig Extra { get; set; }
    }
}
