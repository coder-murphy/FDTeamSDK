using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDSDK.MathSupports.Geography
{
    /// <summary>
    /// 地理点
    /// </summary>
    public struct GISPoint : IFDSDKMathObject
    {
        /// <summary>
        /// 新建一个地理点
        /// </summary>
        public GISPoint(double lon,double lat,double alt)
        {
            Longitude = lon;
            Latitude = lat;
            Altitude = alt;
        }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; } 

        /// <summary>
        /// 高度
        /// </summary>
        public double Altitude { get; set; }

        /// <summary>
        /// 比较点是否相等
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static bool operator== (GISPoint p0,GISPoint p1)
        {
            return p0.Longitude == p1.Longitude && p0.Latitude == p1.Latitude && p0.Altitude == p1.Altitude;
        }

        /// <summary>
        /// 比较点是否不等
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static bool operator !=(GISPoint p0, GISPoint p1)
        {
            return p0.Longitude != p1.Longitude || p0.Latitude != p1.Latitude || p0.Altitude != p1.Altitude;
        }

        /// <summary>
        /// 点相加
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static GISPoint operator+(GISPoint p0, GISPoint p1)
        {
            return new GISPoint
            {
                Longitude = p0.Longitude + p1.Longitude,
                Latitude = p0.Latitude + p1.Latitude,
                Altitude = p0.Altitude + p1.Altitude,
            };
        }

        /// <summary>
        /// 点相减
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static GISPoint operator -(GISPoint p0, GISPoint p1)
        {
            return new GISPoint
            {
                Longitude = p0.Longitude - p1.Longitude,
                Latitude = p0.Latitude - p1.Latitude,
                Altitude = p0.Altitude - p1.Altitude,
            };
        }

        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// 获取该对象的哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
