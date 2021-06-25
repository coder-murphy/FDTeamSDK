using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.MathSupports
{
    /// <summary>
    /// 区域接口
    /// </summary>
    public interface IFDArea : IFDSDKMathObject, IList<IFDPoint>
    {
        /// <summary>
        /// 区域类型
        /// </summary>
        FDAreaType AreaType { get; }

        /// <summary>
        /// 区域的点
        /// </summary>
        List<IFDPoint> Points { get; }

        /// <summary>
        /// 添加点
        /// </summary>
        int Add(double x, double y);

        /// <summary>
        /// 点是否在区域内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        bool IsPointInArea(IFDPoint point);

        /// <summary>
        /// 计算面积
        /// </summary>
        /// <returns></returns>
        double CalcAreaValue();
    }
}
