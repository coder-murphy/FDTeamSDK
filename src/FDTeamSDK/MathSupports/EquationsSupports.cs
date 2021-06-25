using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 方程组件
    /// </summary>
    public class EquationsSupports : IFDSDKMathModule
    {

        /// <summary>
        /// 获取直线方程的a b c三个系数 ()
        /// </summary>
        /// <returns></returns>
        public static EquationCoefficient GetStraightLineCoefficient(PointD p0,PointD p1)
        {
            EquationCoefficient ec = new EquationCoefficient();
            ec.CoefficientType = EquationCoefficientType.StraightLineEquation;
            double slope = MathExtensions.GetSlopeAbs(p0, p1);
            double deltaY = GetStraightLineZeroXDeltaY(p0, p1);
            ec.A = slope;
            ec.B = -1;
            ec.C = deltaY;
            return ec;
        }

        /// <summary>
        /// 经过两点的直线在X=0处Y的值
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        private static double GetStraightLineZeroXDeltaY(PointD p0, PointD p1)
        {
            return p0.Y - (Math.Abs(p0.X) / Math.Abs(p1.X - p0.X)) * Math.Abs(p1.Y - p0.Y);
        }
    }
}
