using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 羲和点接口
    /// </summary>
    public interface IFDPoint : IFDSDKMathObject
    {
        /// <summary>
        /// X坐标
        /// </summary>
        double X { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        double Y { get; set; }

        /// <summary>
        /// 转化为浮点点
        /// </summary>
        /// <returns></returns>
        PointF ToPointF();

        /// <summary>
        /// 转化为Drawing使用的点
        /// </summary>
        /// <returns></returns>
        Point ToDrawingPoint();

        /// <summary>
        /// 转化为windows使用的点
        /// </summary>
        /// <returns></returns>
        System.Windows.Point ToWindowsPoint();

    }
}
