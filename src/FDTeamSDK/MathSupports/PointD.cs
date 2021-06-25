using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 双精度点
    /// </summary>
    public class PointD : IFDPoint
    {
        /// <summary>
        /// 新建一个双精度点
        /// </summary>
        public PointD()
        {

        }

        /// <summary>
        /// 新建一个双精度点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// X坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 转换为浮点型
        /// </summary>
        /// <returns></returns>
        public PointF ToPointF()
        {
            return new PointF((float)this.X, (float)this.Y);
        }

        /// <summary>
        /// 转换为system.drawing的点
        /// </summary>
        /// <returns></returns>
        public Point ToDrawingPoint()
        {
            return new Point((int)X, (int)Y);
        }

        /// <summary>
        /// 转换为windows的点
        /// </summary>
        /// <returns></returns>
        public System.Windows.Point ToWindowsPoint()
        {
            return new System.Windows.Point(X, Y);
        }

        /// <summary>
        /// 点相加
        /// </summary>
        /// <param name="op0"></param>
        /// <param name="op1"></param>
        /// <returns></returns>
        public static PointD operator +(PointD op0, PointD op1)
        {
            return new PointD(op0.X + op1.X, op0.Y + op1.Y);
        }

        /// <summary>
        /// 点相减
        /// </summary>
        /// <param name="op0"></param>
        /// <param name="op1"></param>
        /// <returns></returns>
        public static PointD operator -(PointD op0, PointD op1)
        {
            return new PointD(op0.X - op1.X, op0.Y - op1.Y);
        }

        /// <summary>
        /// 点相等
        /// </summary>
        /// <param name="op0"></param>
        /// <param name="op1"></param>
        /// <returns></returns>
        public static bool operator ==(PointD op0, PointD op1)
        {
            return op0.X == op1.X && op0.Y == op1.Y;
        }

        /// <summary>
        /// 点不等
        /// </summary>
        /// <param name="op0"></param>
        /// <param name="op1"></param>
        /// <returns></returns>
        public static bool operator !=(PointD op0, PointD op1)
        {
            return (op0 == op1) == false;
        }

        /// <summary>
        /// 比较两个点是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// 获取点的哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
