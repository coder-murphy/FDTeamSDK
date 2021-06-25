using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FDSDK.MathSupports;

namespace FDSDK.Graphics2D
{
    /// <summary>
    /// 颜色组件
    /// </summary>
    public class XHColorComponent : IFDCustomComponent
    {
        /// <summary>
        /// 新建一个颜色组件
        /// </summary>
        public XHColorComponent()
        {
            GeneratorOption = ColorGeneratorOptions.NoAssign;
            IsRandomAlpha = false;
            Alpha = 255;
        }

        /// <summary>
        /// 色彩生成设置
        /// </summary>
        public ColorGeneratorOptions GeneratorOption { get; set; }

        /// <summary>
        /// 是否随机透明度
        /// </summary>
        public bool IsRandomAlpha { get; set; }

        /// <summary>
        /// Alpha的值
        /// </summary>
        public byte Alpha { get; set; }

        /// <summary>
        /// 重置颜色生成器
        /// </summary>
        public void ResetHistory()
        {
            ColorGeneratedHistoryList.Clear();
        }

        /// <summary>
        /// 根据配置产生下一个颜色
        /// </summary>
        /// <returns></returns>
        public Color NextColor()
        {
            var color = new Color();
            byte rValue = 0;
            byte gValue = 0;
            byte bValue = 0;
            byte aValue = Alpha;
            if (GeneratorOption == ColorGeneratorOptions.NoAssign)
            {
                rValue = (byte)rb.RandomInt(0, 255);
                gValue = (byte)rb.RandomInt(0, 255);
                bValue = (byte)rb.RandomInt(0, 255);
                while (ColorGeneratedHistoryList.Exists(x => x.R == rValue && x.G == gValue && x.B == bValue))
                {
                    rValue = (byte)rb.RandomInt(0, 255);
                    gValue = (byte)rb.RandomInt(0, 255);
                    bValue = (byte)rb.RandomInt(0, 255);
                }
                if (IsRandomAlpha)
                    aValue = (byte)rb.RandomInt(0, 255);
                color = Color.FromArgb(aValue, rValue, gValue, bValue);
                ColorGeneratedHistoryList.Add(color);
            }
            else if (GeneratorOption == ColorGeneratorOptions.ChartColor)
            {
                byte minValue = (byte)rb.RandomInt(50, 220);
                SetRandomChartColor(minValue, ref rValue, ref gValue, ref bValue);
                while (IsNextColorRGBLargeDiffer(rValue, gValue, bValue) == false)
                {
                    SetRandomChartColor(minValue,ref rValue,ref gValue,ref bValue);
                }
                if (IsRandomAlpha)
                    aValue = (byte)rb.RandomInt(0, 255);
                color = Color.FromArgb(aValue, rValue, gValue, bValue);
                ColorGeneratedHistoryList.Add(color);
            }
            return color;
        }

        /// <summary>
        /// 判断下一个颜色的值与颜色表区别是否大
        /// </summary>
        /// <param name="dstR"></param>
        /// <param name="dstG"></param>
        /// <param name="dstB"></param>
        /// <returns></returns>
        public bool IsNextColorRGBLargeDiffer(byte dstR, byte dstG, byte dstB)
        {
            if (ColorGeneratedHistoryList.Count == 0)
                return true;
            //var last = ColorGeneratedHistoryList.Last();
            return ColorGeneratedHistoryList.Any(x => (Math.Pow(x.R - dstR, 2) + Math.Pow(x.G - dstG, 2) + Math.Pow(x.B - dstB, 2) > 1000));
        }

        /// <summary>
        /// 设置随机表色
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        private void SetRandomChartColor(byte minValue,ref byte r,ref byte g,ref byte b)
        {
            int option = rb.RandomInt(0, 5);
            if(option == 0)
            {
                r = minValue;
                g = 0xFF;
                b = (byte)rb.RandomInt(minValue, 0xFF);
            }
            else if (option == 1)
            {
                r = minValue;
                g = (byte)rb.RandomInt(minValue, 0xFF);
                b = 0xFF;
            }
            else if(option == 2)
            {
                r = 0xFF;
                g = minValue;
                b = (byte)rb.RandomInt(minValue, 0xFF);
            }
            else if(option == 3)
            {
                r = (byte)rb.RandomInt(minValue, 0xFF);
                g = minValue;
                b = 0xFF;
            }
            else if (option == 4)
            {
                r = (byte)rb.RandomInt(minValue, 0xFF);
                g = 0xFF;
                b = minValue;
            }
            else if (option == 5)
            {
                r = 0xFF;
                g = (byte)rb.RandomInt(minValue, 0xFF);
                b = minValue;
            }
        }

        private List<Color> _ColorGeneratedHistoryList = null;
        /// <summary>
        /// 生成的颜色历史
        /// </summary>
        public List<Color> ColorGeneratedHistoryList
        {
            get
            {
                if (_ColorGeneratedHistoryList == null)
                    _ColorGeneratedHistoryList = new List<Color>();
                return _ColorGeneratedHistoryList;
            }
        }

        private List<Color> _ColorList = null;
        /// <summary>
        /// 生成的颜色列表
        /// </summary>
        public List<Color> ColorList
        {
            get
            {
                if (_ColorList == null)
                    _ColorList = new List<Color>();
                return _ColorList;
            }
        }

        /// <summary>
        /// 生成一个随机颜色列表
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public List<Color> CreateRandomColorList(uint length)
        {
            ColorList.Clear();
            for (int i = 0; i < length; i++)
            {
                var color = NextColor();
                ColorList.Add(color);
            }
            return ColorList;
        }

        private RandomBuilder rb = new RandomBuilder();
    }

    /// <summary>
    /// 色彩生成器设定
    /// </summary>
    public enum ColorGeneratorOptions
    {
        /// <summary>
        /// 未指定
        /// </summary>
        NoAssign,
        /// <summary>
        /// 图表颜色
        /// </summary>
        ChartColor,

    }
}
