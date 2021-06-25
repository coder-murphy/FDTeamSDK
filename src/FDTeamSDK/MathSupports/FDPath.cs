using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 路径
    /// </summary>
    public class FDPath : IEnumerable<PointD>,IFDSDKMathObject
    {
        /// <summary>
        /// 点集
        /// </summary>
        internal List<PointD> Points
        {
            get {
                if (_Points == null)
                    _Points = new List<PointD>();
                return _Points;
            }
        }
        private List<PointD> _Points = null;

        /// <summary>
        /// 根据点集合新建一个路径对象
        /// </summary>
        /// <param name="points"></param>
        public FDPath(IEnumerable<PointD> points)
        {
            Points.AddRange(points);
        }

        /// <summary>
        /// 新建一个路径对象
        /// </summary>
        public FDPath()
        {

        }

        /// <summary>
        /// 路径点的数量
        /// </summary>
        public int Count
        {
            get { return this.Count(); }
        }

        /// <summary>
        /// 根据索引访问点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PointD this[int index]
        {
            get { return Points[index]; }
        }

        /// <summary>
        /// 清除所有路径点
        /// </summary>
        public void Clear()
        {
            Points.Clear();
        }

        /// <summary>
        /// 是否为直角路径
        /// </summary>
        public bool IsRightAnglePath
        {
            get
            {
                if (this.Count() == 0 || this.Count() == 1)
                    return false;
                var list = this.ToList();
                for(int i = 0;i < list.Count - 1;i++)
                {
                    if(list[i].X == list[i + 1].X && list[i].Y == list[i + 1].Y)
                    {
                        return false;
                    }
                    else if(list[i].X != list[i + 1].X && list[i].Y != list[i + 1].Y)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 获取指定索引处的路径方向
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public XHRightAngleDirection GetRightAngleDirectionByIndex(int index)
        {
            if (this.IsRightAnglePath == false)
                return XHRightAngleDirection.Unknown;
            else if (index == this.Count() - 1)
                return XHRightAngleDirection.Final;
            else
            {
                if (this[index].X < this[index + 1].X)
                    return XHRightAngleDirection.Right;
                else if (this[index].X > this[index + 1].X)
                    return XHRightAngleDirection.Left;
                else if (this[index].Y < this[index + 1].Y)
                    return XHRightAngleDirection.Down;
                else if (this[index].Y > this[index + 1].Y)
                    return XHRightAngleDirection.Up;
                else
                    return XHRightAngleDirection.Unknown;
            }
        }

        #region Enumerator
        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<PointD> GetEnumerator()
        {
            return ((IEnumerable<PointD>)Points).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<PointD>)Points).GetEnumerator();
        }
        #endregion

    }

    /// <summary>
    /// 直角路径方向
    /// </summary>
    public enum XHRightAngleDirection
    {
        /// <summary>
        /// 上
        /// </summary>
        Up,
        /// <summary>
        /// 下
        /// </summary>
        Down,
        /// <summary>
        /// 左
        /// </summary>
        Left,
        /// <summary>
        /// 右
        /// </summary>
        Right,
        /// <summary>
        /// 终点
        /// </summary>
        Final,
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
    }
}
