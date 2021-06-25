using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDSDK.WebSupports;

namespace FDSDK.Define
{
    /// <summary>
    /// 常量声明类
    /// </summary>
    public static class DefineObjects
    {
        /// <summary>
        /// 合法数字
        /// </summary>
        public static readonly char[] DecimalChars = new char[]
        {
            '0','1','2','3','4','5','6','7','8','9','.',','
        };

        /// <summary>
        /// 合法数字(地理)
        /// </summary>
        public static readonly char[] DecimalCharsGeo = new char[]
        {
            '0','1','2','3','4','5','6','7','8','9','.'
        };

        /// <summary>
        /// 合法地理标识(地理)
        /// </summary>
        public static readonly char[] DecimalCharsGeoSign = new char[]
        {
            'N','n','S','s','E','e','W','w'
        };

        /// <summary>
        /// 远程请求方法键值对
        /// </summary>
        public static readonly Dictionary<Method, string> MethodPair = new Dictionary<Method, string>()
        {
            {Method.GET,"GET"},
            {Method.POST,"POST"},
            {Method.PUT,"PUT"}
        };

        /// <summary>
        ///  远程请求连接方式键值对
        /// </summary>
        public static readonly Dictionary<ContentType, string> ContentTypePair = new Dictionary<ContentType, string>()
        {
            {ContentType.Json,"application/json"},
            {ContentType.Text,"application/text"},
        };
    }
}
