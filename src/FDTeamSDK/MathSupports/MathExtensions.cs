using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 数学拓展方法
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// 将一个数字list以指定重复数字填充 
        /// </summary>
        /// <param name="decimalList"></param>
        /// <param name="padding"></param>
        public static List<decimal> Repeat(this List<decimal> decimalList, decimal padding)
        {
            if (decimalList.Count == 0)
                return new List<decimal> { 0 };
            var list = new List<decimal>();
            list.AddRange(decimalList);
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = padding;
            }
            return list;
        }

        /// <summary>
        /// 将一个数字数组以指定重复数字填充 
        /// </summary>
        public static decimal[] Repeat(this decimal[] decimalArray, decimal padding)
        {
            if (decimalArray.Length == 0)
                return new decimal[] { 0 };
            return decimalArray.ToList().Repeat(padding).ToArray();
        }

        /// <summary>
        /// 计算一个十进制数组的求和
        /// </summary>
        public static decimal Sum(this decimal[] decimalArray)
        {
            if (decimalArray.Length == 0)
                return 0;
            decimal sum = 0;
            foreach (var i in decimalArray)
            {
                sum += i;
            }
            return sum; 
        }

        /// <summary>
        /// 计算一个十进制数list的求和
        /// </summary>
        public static decimal Sum(this List<decimal> decimalArray)
        {
            if (decimalArray.Count == 0)
                return 0;
            decimal sum = 0;
            foreach (var i in decimalArray)
            {
                sum += i;
            }
            return sum;
        }

        /// <summary>
        /// 计算一个十进制数数组的方差
        /// </summary>
        public static decimal Variance(this decimal[] decimalArray)
        {
            if (decimalArray.Length == 0)
                return 0;
            decimal originalAverage = decimalArray.Average();
            decimal sumPow = 0;
            foreach (var i in decimalArray)
            {
                sumPow += (decimal)Math.Pow((double)(i - originalAverage), 2d);
            }
            return sumPow /= decimalArray.Length; 
        }

        /// <summary>
        /// 计算一个float数组的方差
        /// </summary>
        public static float Variance(this float[] decimalArray)
        {
            if (decimalArray.Length == 0)
                return 0;
            float originalAverage = decimalArray.Average();
            float sumPow = 0;
            foreach (var i in decimalArray)
            {
                sumPow += (float)Math.Pow((float)(i - originalAverage), 2d);
            }
            return sumPow /= decimalArray.Length;
        }

        /// <summary>
        /// 计算一个double数组的方差
        /// </summary>
        public static double Variance(this double[] decimalArray)
        {
            if (decimalArray.Length == 0)
                return 0;
            double originalAverage = decimalArray.Average();
            double sumPow = 0;
            foreach (var i in decimalArray)
            {
                sumPow += Math.Pow(i - originalAverage, 2d);
            }
            return sumPow /= decimalArray.Length;
        }

        /// <summary>
        /// 返回一个float数组的极差
        /// </summary>
        public static float CalcRange(this IEnumerable<float> arr)
        {
            return (arr.Max() - arr.Min());
        }

        /// <summary>
        /// 返回一个double数组的极差
        /// </summary>
        public static double CalcRange(this IEnumerable<double> arr)
        {
            return (arr.Max() - arr.Min());
        }

        /// <summary>
        /// 返回一个double数组的连乘结果
        /// </summary>
        public static double Multi(this IEnumerable<double> arr)
        {
            if (arr.Count() == 0)
                return 0;
            var list = arr.ToList();
            double num = list.First();
            for(int i = 1;i < list.Count;i++)
            {
                num *= list[i];
            }
            return num;
        }

        /// <summary>
        /// 返回一个float数组的标准差
        /// </summary>
        public static float StandradDeviation(this IEnumerable<float> arr)
        {
            return (float)Math.Sqrt(arr.Variance());
        }

        /// <summary>
        /// 返回一个float数组的标准差
        /// </summary>
        public static double StandradDeviation(this IEnumerable<double> arr)
        {
            return Math.Sqrt(arr.Variance());
        }

        /// <summary>
        /// 计算一个十进制数list的方差
        /// </summary>
        public static double Variance(this IEnumerable<double> decimalList)
        {
            return decimalList.ToArray().Variance();
        }

        /// <summary>
        /// 计算一个十进制数list的方差
        /// </summary>
        public static decimal Variance(this IEnumerable<decimal> decimalList)
        {
            return decimalList.ToArray().Variance();
        }

        /// <summary>
        /// 计算一个floatlist的方差
        /// </summary>
        public static float Variance(this IEnumerable<float> decimalList)
        {
            return decimalList.ToArray().Variance();
        }

        /// <summary>
        /// 将一个单精度浮点数组进行所有元素的放缩
        /// </summary>
        public static float[] Zoom(this float[] arr, float rate)
        {
            List<float> temp = new List<float>();
            foreach (float i in arr)
            {
                temp.Add(i * rate);
            }
            return temp.ToArray();
        }

        /// <summary>
        /// 将一个单精度浮点list进行所有元素的放缩
        /// </summary>
        public static List<float> Zoom(this List<float> arr, float rate)
        {
            List<float> temp = new List<float>();
            foreach (float i in arr)
            {
                temp.Add(i * rate);
            }
            return temp;
        }

        /// <summary>
        /// 创建一个重复数字的十进制数序列
        /// </summary>
        public static decimal[] CreateRepeatDecimalArray(int length, decimal repeatValue)
        {
            List<decimal> decimals = new List<decimal>();
            for (int i = 0; i < length; i++)
            {
                decimals.Add(repeatValue);
            }
            return decimals.ToArray();
        }

        /// <summary>
        /// 创建一个重复数字的单精度浮点数序列
        /// </summary>
        public static float[] CreateFloatDecimalArray(int length, float repeatValue,int digits)
        {
            List<float> decimals = new List<float>();
            for (int i = 0; i < length; i++)
            {
                decimals.Add(repeatValue.Round(digits));
            }
            return decimals.ToArray();
        }

        /// <summary>
        /// 转换为保留指定位数小数的单精度浮点型
        /// </summary>
        public static float Round(this float number, int digits)
        {
            return (float)Math.Round(number, digits);
        }

        /// <summary>
        /// 转换为保留指定位数小数的双精度浮点型
        /// </summary>
        public static double Round(this double number, int digits)
        {
            return Math.Round(number, digits);
        }

        /// <summary>
        /// 转换十进制数组类型为double类型并迭代访问
        /// </summary>
        public static IEnumerable<double> AsEnumerable(this decimal[] decArr)
        {
            foreach (var i in decArr)
                yield return (double)i;
        }

        /// <summary>
        /// 转换十进制list类型为double类型并迭代访问
        /// </summary>
        public static IEnumerable<double> AsEnumerable(this List<decimal> decArr)
        {
            foreach (var i in decArr)
                yield return (double)i;
        }

        /// <summary>
        /// 转换数组类型为double类型
        /// </summary>
        public static double[] ConvertToDoubleArray(this decimal[] decArr)
        {
            return decArr.AsEnumerable().ToArray();
        }

        /// <summary>
        /// 转换list类型为double类型
        /// </summary>
        public static List<double> ConvertToDoubleList(this decimal[] decArr)
        {
            return decArr.AsEnumerable().ToList();
        }

        /// <summary>
        /// 返回整个数组保留指定位数小数
        /// </summary>
        public static float[] Round(this float[] arr,int digits)
        {
            var temp = new List<float>();
            foreach (var i in arr)
            {
                temp.Add(i.Round(digits));
            }
            return temp.ToArray();
        }

        /// <summary>
        /// 返回整个数组保留指定位数小数
        /// </summary>
        public static double[] Round(this double[] arr, int digits)
        {
            var temp = new List<double>();
            foreach (var i in arr)
            {
                temp.Add(i.Round(digits));
            }
            return temp.ToArray();
        }

        /// <summary>
        /// 数组各个元素依次相乘
        /// </summary>
        public static float[] ArrayMultpie(this float[] arr0, float[] arr1)
        {
            if (arr0.Length != arr1.Length)
                return new float[] { 0f };
            List<float> outs = new List<float>();
            for (int i = 0; i < arr0.Length; i++)
            {
                outs.Add(arr0[i] * arr1[i]);
            }
            return outs.ToArray();
        }

        /// <summary>
        /// 计算数组各个元素相乘之和
        /// </summary>
        public static float ArrayMultpieSum(this float[] arr0, float[] arr1)
        {
            if (arr0.Length != arr1.Length)
                return 0f;
            return arr0.ArrayMultpie(arr1).Sum();
        }

        /// <summary>
        /// 数组元素依次相加
        /// </summary>
        /// <param name="arr0"></param>
        /// <param name="arr1"></param>
        /// <returns></returns>
        public static List<double> ArraySum(this IEnumerable<double> arr0,IEnumerable<double> arr1)
        {
            int len = arr0.Count() >= arr1.Count() ? arr0.Count() : arr1.Count();
            var list0 = arr0.ToList();
            var list1 = arr1.ToList();
            List<double> outList = new List<double>();
            for(int i = 0;i < len;i++)
            {
                outList.Add(list0[i] + list1[i]);
            }
            return outList;
        }

        /// <summary>
        /// 数组元素依次相减
        /// </summary>
        /// <param name="arr0"></param>
        /// <param name="arr1"></param>
        /// <returns></returns>
        public static List<double> ArrayDec(this IEnumerable<double> arr0, IEnumerable<double> arr1)
        {
            int len = arr0.Count();
            var list0 = arr0.ToList();
            var list1 = arr1.ToList();
            List<double> outList = new List<double>();
            for (int i = 0; i < len; i++)
            {
                outList.Add(list0[i] - list1[i]);
            }
            return outList;
        }

        /// <summary>
        /// 求倒数
        /// </summary>
        public static float Reciprocal(this float arg)
        {
            return 1 / arg;
        }

        /// <summary>
        /// 方差评估分数计算
        /// </summary>
        public static float VarianceTotal(this float variance)
        {
            if (variance < 0.000001f)
                return 100;
            if (variance >= 0.000001f && variance < 1)
            {
                return (1 - variance) * 20 + 80;
            }
            if (variance >= 1 && variance < 10)
            {
                return (variance / 10) * 20 + 60;
            }
            if (variance >= 10 && variance < 100)
            {
                return (variance / 100) * 20 + 40;
            }
            if (variance >= 100 && variance < 1000)
            {
                return (variance / 1000) * 20 + 20;
            }
            if (variance >= 1000 && variance < 10000)
            {
                return (variance / 10000) * 20;
            }
            return 0;
        }

        /// <summary>
        /// 方差评估分数计算
        /// </summary>
        public static double VarianceTotal(this double variance)
        {
            if (variance < 0.000001f)
                return 100;
            if (variance >= 0.000001f && variance < 1)
            {
                return (1 - variance) * 20 + 80;
            }
            if (variance >= 1 && variance < 10)
            {
                return (variance / 10) * 20 + 60;
            }
            if (variance >= 10 && variance < 100)
            {
                return (variance / 100) * 20 + 40;
            }
            if (variance >= 100 && variance < 1000)
            {
                return (variance / 1000) * 20 + 20;
            }
            if (variance >= 1000 && variance < 10000)
            {
                return (variance / 10000) * 20;
            }
            return 0;
        }

        /// <summary>
        /// 将点转化为双精度点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointD ToPointD(this System.Drawing.Point point)
        {
            return new PointD(point.X, point.Y);
        }

        /// <summary>
        /// 将点转化为双精度点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointD ToPointD(this System.Drawing.PointF point)
        {
            return new PointD(point.X, point.Y);
        }

        /// <summary>
        /// 将点转化为双精度点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointD ToPointD(this System.Windows.Point point)
        {
            return new PointD(point.X, point.Y);
        }

        /// <summary>
        /// 求两点之间线段长度比
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static double GetSlopeAbs(IFDPoint p0, IFDPoint p1)
        {
            return Math.Abs(p0.Y - p1.Y) / Math.Abs(p0.X - p1.X);
        }

        /// <summary>
        /// 求两点之间斜率
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static double GetSlope(IFDPoint p0, IFDPoint p1)
        {
            return (p0.X - p1.X) / (p0.Y - p1.Y);
        }

        /// <summary>
        /// 求两点间距平方
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static double GetDistanceSquare(IFDPoint p0, IFDPoint p1)
        {
            return Math.Abs(p0.X - p1.X) * Math.Abs(p0.Y - p1.Y);
        }

        /// <summary>
        /// 计算与目标点之间的叉积
        /// </summary>
        /// <returns></returns>
        public static double CalcCrossProduct(this IFDPoint srcPoint, IFDPoint dstPoint)
        {
            return srcPoint.X * dstPoint.Y - dstPoint.X * srcPoint.Y;
        }

        /// <summary>
        /// 获取多边形面积
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static double GetAreaValue(this IFDArea area)
        {
            if (area.Count < 3)
                return 0;
            if (area.Count == 3 && GetSlope(area[0], area[1]) == GetSlope(area[1], area[2]))
            {
                Console.WriteLine("三角形斜率相等,计算结果为0");
                return 0;
            }
            double tail = area.Last().CalcCrossProduct(area.First());
            double head = 0;
            for (int i = 0; i < area.Count - 1; i++)
            {
                head += area[i].CalcCrossProduct(area[i + 1]);
            }
            return Math.Abs(tail + head) / 2;
        }
    }
}
