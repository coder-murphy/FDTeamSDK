using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.Extensions
{
    /// <summary>
    /// 地理拓展
    /// </summary>
    public static class GeographyExtensions
    {
        /// <summary>
        /// 将满足条件的字符串转化成地理坐标信息
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static LocationInfo ToGeoLoactionInfo(this string inStr)
        {
            var info = new LocationInfo();
            var filterGeoNumber = Define.DefineObjects.DecimalCharsGeo;
            var filterGeoSigns = Define.DefineObjects.DecimalCharsGeoSign;
            var signs = inStr.SplitFilter(filterGeoNumber);
            var values = inStr.SplitFilter(filterGeoSigns);
            if (signs.Length < 2 || values.Length < 2)
                return info.Error;
            var max = 2;
            for (int i = 0; i < max; i++)
            {
                string sign = signs[i];
                if (sign == "N" || sign == "n" || sign == "S" || sign == "s")
                {
                    info.LatitudeValue = float.Parse(values[i]);
                    info.LatitudeSign = (signs[i] == "N" || signs[i] == "n") ? 'N' : 'S';
                }
                if (sign == "E" || sign == "e" || sign == "W" || sign == "w")
                {
                    info.LongitudeValue = float.Parse(values[i]);
                    info.LongitudeSign = (signs[i] == "E" || signs[i] == "e") ? 'E' : 'W';
                }
            }
            return info;
        }
    }

    /// <summary>
    /// 地理位置信息
    /// </summary>
    public struct LocationInfo
    {
        /// <summary>
        /// 新建一个地理位置信息
        /// </summary>
        /// <param name="flag"></param>
        public LocationInfo(bool flag = true)
        {
            this.description = "no description";
            this.LongitudeSign = '#';
            this.LatitudeSign = '#';
            this.LongitudeValue = 0f;
            this.LatitudeValue = 0f;
            this.Infostring = null;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// 经度描述
        /// </summary>
        public char LongitudeSign;
        /// <summary>
        /// 纬度描述
        /// </summary>
        public char LatitudeSign;
        /// <summary>
        /// 经度值
        /// </summary>
        public float LongitudeValue;
        /// <summary>
        /// 纬度值
        /// </summary>
        public float LatitudeValue;
        /// <summary>
        /// 信息串
        /// </summary>
        public string Infostring
        {
            get
            {
                string outStr = description + '\n';
                string lon = "";
                string lat = "";
                if (LatitudeSign == 'N' || LatitudeSign == 'n')
                {
                    lon = "北纬：" + LatitudeValue.ToString("F3") + "\n";
                }
                else if (LatitudeSign == 'S' || LatitudeSign == 's')
                {
                    lon = "南纬：" + LatitudeValue.ToString("F3") + "\n";
                }
                if (LongitudeSign == 'E' || LongitudeSign == 'e')
                {
                    lat = "东经：" + LongitudeValue.ToString("F3") + "\n";
                }
                else if (LongitudeSign == 'W' || LongitudeSign == 'w')
                {
                    lat = "西经：" + LongitudeValue.ToString("F3") + '\n';
                }
                outStr += (lat + lon);
                return outStr;
            }
            private set { }
        }

        /// <summary>
        /// 错误的地理信息
        /// </summary>
        public LocationInfo Error
        {
            get 
            {
                var outStruct = new LocationInfo
                {
                    description = "错误的地理信息",
                    LongitudeSign = '#',
                    LatitudeSign = '#',
                    LongitudeValue = 0f,
                    LatitudeValue = 0f,
                    Infostring = null
                };
                return outStruct;
            }
            private set { }
        }
    }
}
