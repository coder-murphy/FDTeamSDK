using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.Geography
{
    /// <summary>
    /// 地理区域
    /// </summary>
    public class GISArea : IFDSDKMathObject
    {
        /// <summary>
        /// 新建一个区域对象
        /// </summary>
        public GISArea()
        {

        }

        /// <summary>
        /// 根据点新建对象
        /// </summary>
        /// <param name="points"></param>
        public GISArea(IEnumerable<GISPoint> points)
        {
            Points.Clear();
            points.ToList().ForEach(i => Points.Add(i));
        }

        /// <summary>
        /// 根据索引返回该区域的点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GISPoint this[int index]
        {
            get { return Points[index]; }
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
        /// 添加一个地理点
        /// </summary>
        public int Add(GISPoint point)
        {
            Points.Add(point);
            return Points.Count - 1;
        }

        /// <summary>
        /// 添加一个地理点
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public int Add(double lon, double lat, double alt)
        {
            Points.Add(new GISPoint(lon, lat, alt));
            return Points.Count - 1;
        }

        /// <summary>
        /// 点的数量
        /// </summary>
        public int PointsCount
        {
            get
            {
                return Points.Count;
            }
        }

        private List<GISPoint> _Points = null;
        /// <summary>
        /// 区域的点
        /// </summary>
        public List<GISPoint> Points
        {
            get
            {
                if (_Points == null)
                    _Points = new List<GISPoint>();
                return _Points;
            }
        }
    }
}
