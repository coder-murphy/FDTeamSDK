using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.EvaluationSupports
{
    /// <summary>
    /// 统计功能扩展
    /// </summary>
    public static class EvaluationExtensions
    {
        /// <summary>
        /// 添加单个指标信息
        /// </summary>
        public static int AddSimuinfo(this List<SimuDataSimple> indicators, SimuDataSimple data)
        {
            indicators.Add(data);
            return indicators.Count - 1;
        }

        /// <summary>
        /// 添加单个指标信息
        /// </summary>
        public static int AddSimuinfo(this List<SimuDataSimple> indicators, string name, float weight, float value)
        {
            indicators.Add(new SimuDataSimple
            {
                Name = name,
                Weight = weight,
                Value = value
            });
            return indicators.Count - 1;
        }

    }
}
