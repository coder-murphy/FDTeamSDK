using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 自定义区域
    /// </summary>
    public class XHArea : IFDArea
    {
        /// <summary>
        /// 新建一个区域对象
        /// </summary>
        public XHArea()
        {

        }

        /// <summary>
        /// 根据点新建对象
        /// </summary>
        /// <param name="points"></param>
        public XHArea(IEnumerable<PointD> points)
        {
            Points.Clear();
            points.ToList().ForEach(i => Points.Add(i));
        }

        /// <summary>
        /// 根据索引返回该区域的点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IFDPoint this[int index]
        {
            get { return Points[index]; }
            set { Points[index] = value; }
        }

        /// <summary>
        /// 删除点
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (index > Points.Count - 1)
                return;
            Points.RemoveAt(index);
        }

        /// <summary>
        /// 清空点
        /// </summary>
        public void Clear()
        {
            this.Points.Clear();
        }

        /// <summary>
        /// 添加一个点
        /// </summary>
        public int Add(IFDPoint p)
        {
            Points.Add(p);
            return Points.Count - 1;
        }

        /// <summary>
        /// 添加一个点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public int Add(double x,double y)
        {
            Points.Add(new PointD(x, y));
            return Points.Count - 1;
        }

        /// <summary>
        /// 点的数量
        /// </summary>
        public int Count
        {
            get
            {
                return Points.Count;
            }
        }

        private List<IFDPoint> _Points = null;
        /// <summary>
        /// 区域的点
        /// </summary>
        public List<IFDPoint> Points
        {
            get
            {
                if (_Points == null)
                    _Points = new List<IFDPoint>();
                return _Points;
            }
        }

        /// <summary>
        /// 区域类型
        /// </summary>
        internal FDAreaType _AreaType = FDAreaType.Normal;

        /// <summary>
        /// 区域类型
        /// </summary>
        public FDAreaType AreaType
        {
            get { return _AreaType; }
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 根据路径点创建直角管状区域(以左上角边界为原点)
        /// </summary>
        /// <param name="path">路径点</param>
        /// <param name="width">宽度</param>
        public static XHArea CreateRightAnglePipeAreaFromPath(IEnumerable<PointD> path,double width)
        {
            if (path == null)
                return null;
            XHArea area = new XHArea();
            area._AreaType = FDAreaType.RightAngle;
            var xhPath = new FDPath(path);
            if (xhPath.Count == 0)
            {
                area._AreaType = FDAreaType.Unknown;
                return area;
            }
            else if (xhPath.Count == 1)
            {
                area.Add(xhPath[0].X - width / 2, xhPath[0].Y + width / 2);
                area.Add(xhPath[0].X + width / 2, xhPath[0].Y + width / 2);
                area.Add(xhPath[0].X + width / 2, xhPath[0].Y - width / 2);
                area.Add(xhPath[0].X - width / 2, xhPath[0].Y - width / 2);
                return area;
            }
            else
            {
                if(xhPath.IsRightAnglePath == false)
                {
                    area._AreaType = FDAreaType.Unknown;
                    return area;
                }
                double sw = width / 2;
                List<PointD> list0 = new List<PointD>();
                List<PointD> list1 = new List<PointD>();
                for (int i = 0; i < xhPath.Count - 1; i++)
                {
                    XHRightAngleDirection dir = xhPath.GetRightAngleDirectionByIndex(i);
                    if (i == 0)
                    {
                        // list0 路径左侧点
                        // list1 路径右侧点
                        Console.WriteLine($"{dir}");
                        if (dir == XHRightAngleDirection.Right)
                        {
                            list0.Add(new PointD(xhPath[i].X, xhPath[i].Y - sw));
                            list1.Add(new PointD(xhPath[i].X, xhPath[i].Y + sw));
                        }
                        else if (dir == XHRightAngleDirection.Left)
                        {
                            list0.Add(new PointD(xhPath[i].X, xhPath[i].Y + sw));
                            list1.Add(new PointD(xhPath[i].X, xhPath[i].Y - sw));
                        }
                        else if (dir == XHRightAngleDirection.Up)
                        {
                            list0.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y));
                            list1.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y));
                        }
                        else if (dir == XHRightAngleDirection.Down)
                        {
                            list0.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y));
                            list1.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y));
                        }
                    }
                    else
                    {
                        XHRightAngleDirection lastDir = xhPath.GetRightAngleDirectionByIndex(i - 1);
                        Console.WriteLine($"{dir.ToString()},{lastDir.ToString()}");
                        if (lastDir == XHRightAngleDirection.Left && dir == XHRightAngleDirection.Left)
                        {
                            list0.Add(new PointD(xhPath[i].X, xhPath[i].Y + sw));
                            list1.Add(new PointD(xhPath[i].X, xhPath[i].Y - sw));
                        }
                        else if (lastDir == XHRightAngleDirection.Left && dir == XHRightAngleDirection.Up)
                        {
                            list0.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y + sw));
                            list1.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y - sw));
                        }
                        else if (lastDir == XHRightAngleDirection.Left && dir == XHRightAngleDirection.Down)
                        {
                            list0.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y + sw));
                            list1.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y - sw));
                        }
                        else if (lastDir == XHRightAngleDirection.Right && dir == XHRightAngleDirection.Right)
                        {
                            list0.Add(new PointD(xhPath[i].X, xhPath[i].Y - sw));
                            list1.Add(new PointD(xhPath[i].X, xhPath[i].Y + sw));
                        }
                        else if (lastDir == XHRightAngleDirection.Right && dir == XHRightAngleDirection.Up)
                        {
                            list0.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y - sw));
                            list1.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y + sw));
                        }
                        else if (lastDir == XHRightAngleDirection.Right && dir == XHRightAngleDirection.Down)
                        {
                            list0.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y - sw));
                            list1.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y + sw));
                        }
                        else if (lastDir == XHRightAngleDirection.Up && dir == XHRightAngleDirection.Up)
                        {
                            list0.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y));
                            list1.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y));
                        }
                        else if (lastDir == XHRightAngleDirection.Up && dir == XHRightAngleDirection.Left)
                        {
                            list0.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y + sw));
                            list1.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y - sw));
                        }
                        else if (lastDir == XHRightAngleDirection.Up && dir == XHRightAngleDirection.Right)
                        {
                            list0.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y + sw));
                            list1.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y + sw));
                        }
                        else if (lastDir == XHRightAngleDirection.Down && dir == XHRightAngleDirection.Down)
                        {
                            list0.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y));
                            list1.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y));
                        }
                        else if (lastDir == XHRightAngleDirection.Down && dir == XHRightAngleDirection.Left)
                        {
                            list0.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y + sw));
                            list1.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y - sw));
                        }
                        else if (lastDir == XHRightAngleDirection.Down && dir == XHRightAngleDirection.Right)
                        {
                            list0.Add(new PointD(xhPath[i].X + sw, xhPath[i].Y - sw));
                            list1.Add(new PointD(xhPath[i].X - sw, xhPath[i].Y + sw));
                        }
                        if (i == xhPath.Count - 2)
                        {
                            PointD p0 = new PointD();
                            PointD p1 = new PointD();
                            p0.X = dir == XHRightAngleDirection.Left ? xhPath[i + 1].X : p0.X;
                            p0.Y = dir == XHRightAngleDirection.Left ? xhPath[i + 1].Y + sw : p0.Y;
                            p1.X = dir == XHRightAngleDirection.Left ? xhPath[i + 1].X : p1.X;
                            p1.Y = dir == XHRightAngleDirection.Left ? xhPath[i + 1].Y - sw : p1.Y;
                            p0.X = dir == XHRightAngleDirection.Right ? xhPath[i + 1].X : p0.X;
                            p0.Y = dir == XHRightAngleDirection.Right ? xhPath[i + 1].Y - sw : p0.Y;
                            p1.X = dir == XHRightAngleDirection.Right ? xhPath[i + 1].X : p1.X;
                            p1.Y = dir == XHRightAngleDirection.Right ? xhPath[i + 1].Y + sw : p1.Y;
                            p0.X = dir == XHRightAngleDirection.Up ? xhPath[i + 1].X - sw : p0.X;
                            p0.Y = dir == XHRightAngleDirection.Up ? xhPath[i + 1].Y : p0.Y;
                            p1.X = dir == XHRightAngleDirection.Up ? xhPath[i + 1].X + sw : p1.X;
                            p1.Y = dir == XHRightAngleDirection.Up ? xhPath[i + 1].Y : p1.Y;
                            p0.X = dir == XHRightAngleDirection.Down ? xhPath[i + 1].X + sw : p0.X;
                            p0.Y = dir == XHRightAngleDirection.Down ? xhPath[i + 1].Y : p0.Y;
                            p1.X = dir == XHRightAngleDirection.Down ? xhPath[i + 1].X - sw : p1.X;
                            p1.Y = dir == XHRightAngleDirection.Down ? xhPath[i + 1].Y : p1.Y;
                            list0.Add(p0);
                            list1.Add(p1);
                            Console.WriteLine($"AddTailPoint{p0},{p1}");
                        }
                    }
                }
                list1.Reverse();
                area.Points.AddRange(list0);
                area.Points.AddRange(list1);
            }
            return area;
        }

        /// <summary>
        /// 点是否在区域内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsPointInArea(IFDPoint point)
        {
            if (Count < 3)
                return false;
            int satisfyCount = 0;
            // 第一条线段为尾点与首点的连接线段
            for (int i = 0, j = Count - 1; i < Count; j = i, i++)
            {
                IFDPoint p1 = this[i];
                IFDPoint p2 = this[j];
                if (point.Y < p2.Y)
                {
                    if (p1.Y <= point.Y)
                    {
                        // 斜率判断
                        if ((point.Y - p1.Y) * (p2.X - p1.X) > (point.X - p1.X) * (p2.Y - p1.Y))
                        {
                            satisfyCount++;
                        }
                    }
                }
                else if (point.Y < p1.Y)
                {
                    if ((point.Y - p1.Y) * (p2.X - p1.X) < (point.X - p1.X) * (p2.Y - p1.Y))
                    {
                        satisfyCount++;
                    }
                }
            }
            // 射线与多边形交点为奇数时则在多边形之内，若为偶数个交点时则在多边形之外
            return satisfyCount % 2 == 1;
        }

        /// <summary>
        /// 计算面积
        /// </summary>
        /// <returns></returns>
        public double CalcAreaValue()
        {
            return this.GetAreaValue();
        }

        /// <summary>
        /// 根据点寻找索引
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(IFDPoint item)
        {
            return Points.IndexOf(item);
        }

        /// <summary>
        /// 在指定索引处插入点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, IFDPoint item)
        {
            Points.Insert(index, item);
        }

        void ICollection<IFDPoint>.Add(IFDPoint item)
        {
            Points.Add(item);
        }

        /// <summary>
        /// 判断是否有点存在于该区域内
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(IFDPoint item)
        {
            return Points.Exists(x => x.X == item.X && x.Y == item.Y);
        }

        /// <summary>
        /// 将区域内所有点按索引复制到一维数组中
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(IFDPoint[] array, int arrayIndex)
        {
            Points.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 从区域中删除点
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(IFDPoint item)
        {
            return Points.Remove(item);
        }

        /// <summary>
        /// 获取地区的点集迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IFDPoint> GetEnumerator()
        {
            return Points.GetEnumerator();
        }

        /// <summary>
        /// 获取地区的点集迭代器
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Points.GetEnumerator();
        }
    }

    /// <summary>
    /// 区域类型
    /// </summary>
    public enum FDAreaType
    {
        /// <summary>
        /// 未知多边形
        /// </summary>
        Unknown,
        /// <summary>
        /// 直角多边形
        /// </summary>
        RightAngle,
        /// <summary>
        /// 普通多边形
        /// </summary>
        Normal,
    }
}
