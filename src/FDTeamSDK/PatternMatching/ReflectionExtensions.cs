using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FDSDK.PatternMatching
{
    /// <summary>
    /// 反射扩展
    /// </summary>
    public static class ReflectionExtensions
    {
        #region getstring
        /// <summary>
        /// 反射获取指定字段序列字符串值
        /// </summary>
        public static string[] TryGetValueStrings<T>(this T tSrc, IEnumerable<string> matchStrings = null)
        {
            FieldInfo[] proList = typeof(T).GetFields(BindingFlags.Public);
            List<string> paraNames = matchStrings.ToList();
            if (paraNames == null || paraNames.Count() == 0)
                return proList.Select(i => i.GetValue(i.Name).ToString()).ToArray();
            List<string> strList = new List<string>();
            for (int i = 0; i < paraNames.Count; i++)
            {
                IEnumerable<FieldInfo> dstCollection = proList.Where(f => f.Name == paraNames[i]);
                string dstStr = dstCollection.Count() == 0 ? "KnownValue" : dstCollection.First().GetValue(paraNames[i]).ToString();
                strList.Add(dstStr);
            }
            return strList.ToArray();
        }

        /// <summary>
        /// 从匿名对象中获取所有成员
        /// </summary>
        /// <param name="tSrc"></param>
        /// <returns></returns>
        public static object[] TryGetAllMembersByDict(this object tSrc)
        {
            Dictionary<string, object> pairs = tSrc as Dictionary<string, object>;
            return pairs.Values.ToArray();
        }

        /// <summary>
        /// 字典映射方式获取指定字段序列字符串值
        /// </summary>
        public static string[] TryGetValueStringsByDict(this object tSrc, IEnumerable<string> matchStrings = null, string signPara = "description")
        {
            Dictionary<string, object> pairs = tSrc as Dictionary<string, object>;
            var equalList = pairs.Select(i => i).ToList();
            if (matchStrings == null || matchStrings.Count() == 0)
                return new string[] { };
            List<string> paraNames = matchStrings.ToList();
            List<string> strList = new List<string>();
            for (int i = 0; i < paraNames.Count; i++)
            {
                List<KeyValuePair<string, object>> dstList = equalList.Select(j => j).Where(j => j.Key == paraNames[i]).ToList();
                if (dstList.Count == 0)
                    continue;
                // 如果有子对象
                string value = "";
                // 子对象需重写tostring才会生效
                if (dstList.First().Value is Dictionary<string, object>)
                {
                    var dic = dstList.First().Value as Dictionary<string, object>;
                    if (dic.ContainsKey(signPara))
                    {
                        value = dic[signPara] == null ?
                            "NULL" : dic[signPara].ToString();
                        strList.Add(value);
                    }
                }
                else
                {
                    value = dstList.First().Value == null ? "NULL" : dstList.First().Value.ToString();
                    strList.Add(value);
                }
            }
            return strList.ToArray();
        }
        #endregion

        #region get object
        /// <summary>
        /// 从一个匿名对象中获取所有成员
        /// </summary>
        /// <param name="objSrc"></param>
        /// <param name="matchStrings"></param>
        /// <param name="signPara"></param>
        /// <returns></returns>
        public static object[] TryGetObjectsByDict(this object objSrc, IEnumerable<string> matchStrings = null, string signPara = "description")
        {
            Dictionary<string, object> pairs = objSrc as Dictionary<string, object>;
            var equalList = pairs.Select(i => i).ToList();
            if (matchStrings == null || matchStrings.Count() == 0)
                return new object[] { };
            List<string> paraNames = matchStrings.ToList();
            List<object> objList = new List<object>();
            for (int i = 0; i < paraNames.Count; i++)
            {
                List<KeyValuePair<string, object>> dstList = equalList.Select(j => j).Where(j => j.Key == paraNames[i]).ToList();
                if (dstList.Count == 0)
                    continue;
                objList.AddRange(dstList.Select(x => x.Value));
                // 如果有子对象
                object value = null;
                // 子对象需重写tostring才会生效
                if (dstList.First().Value is Dictionary<string, object>)
                {
                    var dic = dstList.First().Value as Dictionary<string, object>;
                    if(dic.ContainsKey(signPara))
                    {
                        value = dic[signPara] == null ?
                            "NULL" : dic[signPara];
                        objList.Add(value);
                    }
                }
                else
                {
                    value = dstList.First().Value == null ? "NULL" : dstList.First().Value.ToString();
                    objList.Add(value);
                }
            }
            return objList.ToArray();
        }

        /// <summary>
        /// 从对象中获取指定匹配字段的对象成员
        /// </summary>
        /// <param name="objSrc"></param>
        /// <param name="matchString"></param>
        /// <param name="signPara">如果对象中有复合对象 需要抓取的匹配字段名</param>
        /// <returns></returns>
        public static object TryGetObjectByDict(this object objSrc, string matchString, string signPara = "description")
        {
            return objSrc.TryGetObjectsByDict(new string[] { matchString }, signPara).FirstOrDefault();
        }

        /// <summary>
        /// 从匿名对象中获取指定匹配字段的指定类型对象成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objSrc"></param>
        /// <param name="matchString"></param>
        /// <param name="signPara"></param>
        /// <returns></returns>
        public static T TryGetObjectByDict<T>(this object objSrc, string matchString, string signPara = "description") where T : class
        {
            var res = objSrc.TryGetObjectByDict(matchString, signPara);
            if (res is T)
                return res as T;
            return default(T);
        }

        /// <summary>
        /// 判断该匿名对象中是否包含指定字段名成员
        /// </summary>
        /// <param name="objSrc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool ContainsMember(this object objSrc, string propertyName)
        {
            return objSrc.TryGetObjectByDict(propertyName) != null;
        }
        #endregion


        #region judge object type
        /// <summary>
        /// 该匿名对象是否为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsString(this object obj)
        {
            return obj is string;
        }

        /// <summary>
        /// 该匿名对象是否为浮点型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsSingle(this object obj)
        {
            return obj is float || obj is double;
        }

        /// <summary>
        /// 该匿名对象是否为整数型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsInt(this object obj)
        {
            return obj is int || obj is long || obj is byte || obj is short;
        }

        /// <summary>
        /// 该匿名对象是否为布尔型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsBool(this object obj)
        {
            return obj is bool;
        }

        /// <summary>
        /// 该匿名对象是否为复合类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsComplexType(this object obj)
        {
            return obj as Dictionary<string, object> != null && (obj as Dictionary<string, object>).Count > 0;
        }
        #endregion


    }
}
