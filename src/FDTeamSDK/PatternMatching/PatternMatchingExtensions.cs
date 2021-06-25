using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FDSDK.PatternMatching
{
    /// <summary>
    /// 模式匹配扩展
    /// </summary>
    public static class PatternMatchingExtensions
    {
        #region Generic Method
        /// <summary>
        /// 获取一个泛型对象(如果未获取到则返回null)
        /// </summary>
        public static T TryGetGenericObject<T>(this object obj) where T : class
        {
            if (obj is T)
                return obj as T;
            else
                return null;
        }

        /// <summary>
        /// 获取一个泛型对象List (如果未获取到则返回null)
        /// </summary>
        public static List<T> TryGetGenericList<T>(this object obj) where T : class
        {
            if (obj is T)
                return obj as List<T>;
            else
                return null;
        }

        /// <summary>
        /// 是否为匿名键值对（基于字典映射）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsCryptKeyValuePair(this object obj)
        {
            return obj is KeyValuePair<string,object>;
        }

        /// <summary>
        /// 是否为匿名对象数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsCryptArray(this object obj)
        {
            return obj is object[];
        }

        /// <summary>
        /// 是否为匿名对象映射
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsObjectMap(this object obj)
        {
            return obj is Dictionary<string, object>;
        }

        /// <summary>
        /// 将对象进行转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Cast<T>(this object obj) where T : class
        {
            if (obj is T)
                return obj as T;
            else
                return null;
        }
        #endregion
    }
}
