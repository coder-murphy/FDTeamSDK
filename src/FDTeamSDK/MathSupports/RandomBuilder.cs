using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 随机数生成器
    /// </summary>
    public class RandomBuilder : IFDSDKMathModule
    {
        /// <summary>
        /// 该随机数生成器的种子
        /// </summary>
        public int Seed
        {
            get;
            private set;
        }

        /// <summary>
        /// 新建一个随机生成器实例
        /// </summary>
        public RandomBuilder()
        {
            InitBuilder();
        }

        /// <summary>
        /// 返回一个随机整数
        /// </summary>
        public int RandomInt(int min, int max)
        {
            return Random.Next(min, max);
        }

        /// <summary>
        /// 返回一个随机浮点数
        /// </summary>
        public float RandomFloat(float min, float max, int digits = 2)
        {
            float outValue = 0f;
            if (min >= 0 && max >= 0)
                outValue = min + ((max - min) * (float)Random.NextDouble());
            else if (min < 0 && max >= 0)
                outValue = (max - min) * (float)Random.NextDouble();
            else
                outValue = min + Math.Abs(max - min) * (float)Random.NextDouble();
            return outValue;
        }

        /// <summary>
        /// 返回一个指定范围内随机浮点数
        /// </summary>
        public float RandomRangeFloat(float number, float range, int digits = 2)
        {
            return RandomFloat(number - range, number + range,digits);
        }

        /// <summary>
        /// 返回一个随机双精度浮点数
        /// </summary>
        public double RandomDouble(double min, double max, int digits = 2)
        {
            double outValue = 0f;
            if (min >= 0 && max >= 0)
                outValue = min + ((max - min) * Random.NextDouble());
            else if (min < 0 && max >= 0)
                outValue = (max - min) * Random.NextDouble();
            else
                outValue = min + Math.Abs(max - min) * Random.NextDouble();
            return outValue;
        }

        /// <summary>
        /// 返回一个指定范围内随机双精度浮点数
        /// </summary>
        public double RandomRangeDouble(double number, double range, int digits = 2)
        {
            return RandomDouble(number - range, number + range,digits);
        }

        /// <summary>
        /// 返回一个单精度随机浮点数组
        /// </summary>
        public float[] RandomFloatArray(int length, float min, float max, int digits = 2)
        {
            List<float> outList = new List<float>();
            for (int i = 0; i < length; i++)
            {
                outList.Add(this.RandomFloat(min, max, digits));
            }
            return outList.ToArray();
        }

        /// <summary>
        /// 返回一个指定范围内单精度随机浮点数组
        /// </summary>
        public float[] RandomRangeFloatArray(int length, float number, float range, int digits = 2)
        {
            List<float> outList = new List<float>();
            for (int i = 0; i < length; i++)
            {
                outList.Add(this.RandomRangeFloat(number, range, digits));
            }
            return outList.ToArray();
        }

        /// <summary>
        /// 返回一个指定范围内双精度随机浮点数组
        /// </summary>
        public double[] RandomRangeDoubleArray(int length, double number, double range, int digits = 2)
        {
            List<double> outList = new List<double>();
            for (int i = 0; i < length; i++)
            {
                outList.Add(RandomRangeDouble(number, range, digits));
            }
            return outList.ToArray();
        }

        /// <summary>
        /// 生成一组随机的权重（权重之和=1）
        /// </summary>
        /// <param name="length"></param>
        /// <param name="digits">位数</param>
        /// <returns></returns>
        public float[] RandomWeight(int length,int digits)
        {
            if (length < 2)
                return new float[] { 1.0f };
            var intervals = new RandomBuilder().RandomFloatArray(length - 1, 0.01f, 0.99f, digits).ToList().OrderBy(s => s).ToList();
            intervals.Add(1.0f);
            var list = new List<float>
            {
                intervals[0]
            };
            for (int i = 1; i < intervals.Count; i++)
            {
                list.Add(intervals[i] - intervals[i - 1]);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 生成一组随机的权重（权重之和=1）
        /// </summary>
        /// <param name="length"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public double[] RandomWeightDouble(int length, int digits)
        {
            if (length < 2)
                return new double[] { 1.0f };
            var intervals = new RandomBuilder().RandomFloatArray(length - 1, 0.01f, 0.99f, digits).ToList().OrderBy(s => s).ToList();
            intervals.Add(1.0f);
            var list = new List<double>();
            list.Add(intervals[0]);
            for (int i = 1; i < intervals.Count; i++)
            {
                list.Add(intervals[i] - intervals[i - 1]);
            }
            return list.ToArray();
        }
        #region private members
        private static Random Random = new Random();

        private void InitBuilder()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[4];
            provider.GetBytes(bytes);
            Seed = BitConverter.ToInt32(bytes, 0);
            Random = new Random(Seed);
        }
        #endregion
    }
}
