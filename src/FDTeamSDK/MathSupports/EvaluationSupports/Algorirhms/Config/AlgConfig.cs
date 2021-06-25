using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.MathSupports.EvaluationSupports.Algorirhms
{
    /// <summary>
    /// 算法配置
    /// </summary>
    public class AlgorithmConfig : IXHSDKEvaluationObject
    {
        /// <summary>
        /// 蒙特卡洛法抽样次数
        /// </summary>
        public int MonteCarioPickTimes { get; set; }

        /// <summary>
        /// 可用度
        /// </summary>
        public float[] ADCAvailableArray { get; set; }

        /// <summary>
        /// 权重组
        /// </summary>
        public List<float> WeightList { get; set; }

        /// <summary>
        /// 是否使用毫秒
        /// </summary>
        public bool UseMillSeconds { get; set; }

        /// <summary>
        /// 提供一个默认的算法配置
        /// </summary>
        public static AlgorithmConfig Default
        {
            get
            {
                return new AlgorithmConfig()
                {
                    MonteCarioPickTimes = 100,
                    ADCAvailableArray = new float[] { 1.0f, 0.7f, 0.3f, 0f },
                    WeightList = new List<float> { 20, 30, 50, 80 },
                    UseMillSeconds = false
                };
            }
            protected set { }
        }
    }
}
