using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDSDK.GenericSupports.Extensions;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 方程系数类
    /// </summary>
    public class EquationCoefficient : IFDSDKMathObject
    {
        /// <summary>
        /// 系数A
        /// </summary>
        public double A { get; set; }

        /// <summary>
        /// 系数B
        /// </summary>
        public double B { get; set; }

        /// <summary>
        /// 系数C
        /// </summary>
        public double C { get; set; }

        /// <summary>
        /// 系数D
        /// </summary>
        public double D { get; set; }

        /// <summary>
        /// 获取直线方程Y的值
        /// </summary>
        /// <returns></returns>
        public double GetStraightLineYValue(double xValue)
        {
            if (B == 0)
                return 0;
            double resVal = (-A * xValue - C) / B;
            Console.WriteLine($"x = {xValue};outputY = {resVal}");
            return resVal;
        }

        /// <summary>
        /// 系数类型
        /// </summary>
        public EquationCoefficientType CoefficientType { get; internal set; }

        /// <summary>
        /// 将系数作为数组返回
        /// </summary>
        /// <returns></returns>
        public double[] ToArray()
        {
            if (CoefficientType == EquationCoefficientType.StraightLineEquation)
                return new double[] { A, B, C };
            return new double[] { };
        }

        /// <summary>
        /// 将系数组转换为指定形式的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToArray().ToJson();
        }

        /// <summary>
        /// 转换为表达式字符串
        /// </summary>
        /// <returns></returns>
        public string ToExpressionString()
        {
            if (CoefficientType == EquationCoefficientType.StraightLineEquation)
            {
                string opB = B < 0 ? B.ToString() : "+" + B.ToString();
                opB = B == -1 ? "-" : opB;
                string opC = C < 0 ? C.ToString() : "+" + C.ToString();
                return $"直线方程表达式:{A}x{opB}y{opC}=0";
            }
            return "无方程表达式字符串生成";
        }
    }

    

    /// <summary>
    /// 系数类型
    /// </summary>
    public enum EquationCoefficientType
    {
        /// <summary>
        /// 直线方程
        /// </summary>
        StraightLineEquation,
    }
}
