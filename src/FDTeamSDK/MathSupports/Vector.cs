using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 向量拓展方法
    /// </summary>
    public static class VectorExtensions
    {

    }


    /// <summary>
    /// 三维矢量
    /// </summary>
    public class Vector3D : IFDSDKMathObject
    {
        /// <summary>
        /// 原点向量
        /// </summary>
        public static Vector3D Zero
        {
            get { return new Vector3D(0d, 0d, 0d); }
            private set { }
        }

        /// <summary>
        /// 创建一个三维矢量
        /// </summary>
        public Vector3D()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        /// <summary>
        /// 创建一个三维矢量
        /// </summary>
        public Vector3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        /// <summary>
        /// X轴分量
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y轴分量
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Z轴分量
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// 向量模长
        /// </summary>
        public double Module
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)); }
            private set { }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #region operators
        public static Vector3D operator +(Vector3D op0, Vector3D op1)
        {
            return new Vector3D(op0.X + op1.X, op0.Y + op1.Y, op0.Z + op1.Z);
        }

        public static Vector3D operator -(Vector3D op0, Vector3D op1)
        {
            return new Vector3D(op0.X - op1.X, op0.Y - op1.Y, op0.Z - op1.Z);
        }

        public static bool operator ==(Vector3D op0, Vector3D op1)
        {
            return op0.X == op1.X && op0.Y == op1.Y && op0.Z == op1.Z;
        }

        public static bool operator !=(Vector3D op0, Vector3D op1)
        {
            return (op0 == op1) == false;
        }
        #endregion
    }

    /// <summary>
    /// 二维矢量
    /// </summary>
    public class Vector2D : IFDSDKMathObject
    {
        /// <summary>
        /// 原点向量
        /// </summary>
        public static Vector2D Zero
        {
            get { return new Vector2D(0d, 0d); }
            private set { }
        }

        /// <summary>
        /// 创建一个二维矢量
        /// </summary>
        public Vector2D()
        {
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// 创建一个二维矢量
        /// </summary>
        public Vector2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        /// <summary>
        /// X轴分量
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y轴分量
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 向量模长
        /// </summary>
        public double Module
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); }
            private set { }
        }

        /// <summary>
        /// 二维向量转换成点
        /// </summary>
        public PointF ToPointF()
        {
            return new PointF((float)this.X, (float)this.Y);
        }

        /// <summary>
        /// 二维向量相加
        /// </summary>
        /// <param name="op0"></param>
        /// <param name="op1"></param>
        /// <returns></returns>
        public static Vector2D operator +(Vector2D op0, Vector2D op1)
        {
            return new Vector2D(op0.X + op1.X, op0.Y + op1.Y);
        }

        /// <summary>
        /// 二维向量相减
        /// </summary>
        /// <param name="op0"></param>
        /// <param name="op1"></param>
        /// <returns></returns>
        public static Vector2D operator -(Vector2D op0, Vector2D op1)
        {
            return new Vector2D(op0.X - op1.X, op0.Y - op1.Y);
        }

        /// <summary>
        /// 二维向量是否相等
        /// </summary>
        /// <param name="op0"></param>
        /// <param name="op1"></param>
        /// <returns></returns>
        public static bool operator ==(Vector2D op0, Vector2D op1)
        {
            return op0.X == op1.X && op0.Y == op1.Y;
        }
        
        /// <summary>
        /// 二维向量是否不等
        /// </summary>
        /// <param name="op0"></param>
        /// <param name="op1"></param>
        /// <returns></returns>
        public static bool operator !=(Vector2D op0, Vector2D op1)
        {
            return (op0 == op1) == false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
