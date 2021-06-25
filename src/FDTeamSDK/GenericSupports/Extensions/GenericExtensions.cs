using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Linq.Expressions;
using System.Web.Script.Serialization;
using FDSDK.MathSupports;
using System.Xml.Serialization;
using System.IO;

namespace FDSDK.GenericSupports.Extensions
{
    /// <summary>
    /// 泛型扩展
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        ///  二维数组转换为二维List
        /// </summary>
        public static List<List<T>> ToList<T>(this T[,] typeArr)
        {
            var list = new List<List<T>>();
            for (int i = 0; i <= typeArr.GetUpperBound(0); i++)
            {
                var childList = new List<T>();
                for (int j = 0; j <= typeArr.GetUpperBound(1); j++)
                {
                    if (typeArr[i, j] == null)
                    {
                        childList.Add(default(T));
                        continue;
                    }
                    childList.Add(typeArr[i, j]);
                }
                list.Add(childList);
            }
            return list;
        }

        /// <summary>
        /// 将二维list转换为二维非交错数组(矩阵)
        /// </summary>
        public static T[,] ToMatrixArray<T>(this List<List<T>> lists)
        {
            try
            {
                int min = lists.MinCount();
                // 只取二维list元素最少的个数作为宽度
                T[,] arr = new T[lists.Count, lists[0].Count];
                for (int i = 0; i < lists.Count;i++ )
                    for (int j = 0; j < lists[0].Count; j++)
                    {
                        if (lists[i][j] == null)
                        {
                            arr[i, j] = default(T);
                            continue;
                        }
                        arr[i, j] = lists[i][j];
                    }
                return arr;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 从二维list中获取元素最少的一行元素的个数
        /// </summary>
        public static int MinCount<T>(this List<List<T>> lists)
        {
            List<int> ints = new List<int>();
            foreach (var i in lists)
            {
                ints.Add(i.Count);
            }
            return ints.Min();
        }

        /// <summary>
        /// 从二维list中获取元素最多的一行元素的个数
        /// </summary>
        public static int MaxCount<T>(this List<List<T>> lists)
        {
            List<int> ints = new List<int>();
            foreach (var i in lists)
            {
                ints.Add(i.Count);
            }
            return ints.Max();
        }

        /// <summary>
        /// 二维数组迭代访问
        /// </summary>
        public static IEnumerable<T> AsEnumerable<T>(this T[,] typeArr)
        {
            foreach (var i in typeArr)
            {
                yield return i;
            }
        }

        /// <summary>
        /// 二维数组装进一维List并返回
        /// </summary>
        public static List<T> AsList<T>(this T[,] typeArr)
        {
            return typeArr.AsEnumerable().ToList();
        }

        /// <summary>
        /// 二维集合迭代为一维访问
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tSources"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this IEnumerable<IEnumerable<T>> tSources)
        {
            foreach (var i in tSources)
            {
                foreach (var j in i)
                {
                    yield return j;
                }
            }
        }

        /// <summary>
        /// 二维集合转换成一维list
        /// </summary>
        public static List<T> AsList<T>(this IEnumerable<IEnumerable<T>> lists)
        {
            return lists.AsEnumerable().ToList();
        }

        /// <summary>
        /// 将数组装进list
        /// </summary>
        public static void AddRange<T>(this List<T> list, T[] TSources)
        {
            foreach (var i in TSources)
            {
                list.Add(i);
            }
        }

        /// <summary>
        /// 将对象转化为json串
        /// </summary>
        public static string ToJson<T>(this T type)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            json.MaxJsonLength = int.MaxValue;
            return json.Serialize(type);
        }

        /// <summary>
        /// 转换为xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="path"></param>
        public static void ToXml<T>(this T type,string path)
        {
            XmlSerializer xml = new XmlSerializer(type.GetType());
            XmlWriter xmlWriter = XmlWriter.Create(path);
            xml.Serialize(xmlWriter, type);
        }

        /// <summary>
        /// 获取路径下的对象xml文件信息并转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadXmlToObject<T>(string path)where T :class,new()
        {
            Type type = typeof(T);
            XmlReader reader = XmlReader.Create(path);
            return reader.ReadContentAsObject() as T;
        }

        /// <summary>
        /// json串转为对象
        /// </summary>
        public static T JsonToObject<T>(this string jsonStr)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            json.MaxJsonLength = int.MaxValue;
            return json.Deserialize<T>(jsonStr);
        }

        /// <summary>
        /// 分割一个list为多个指定元素数量的list
        /// </summary>
        public static List<T[]> Split<T>(this List<T> list,int number)
        {
            int count = 0;
             List<T[]> lists = new List<T[]> ();
             List<T> temp = new List<T>();
            foreach (var i in list)
            {
                temp.Add(i);
                count++;
                if (count == number || list.IndexOf(i) == list.Count - 1)
                {
                    lists.Add(temp.ToArray());
                    temp.Clear();
                    count = 0;
                }
            }
            return lists;
        }

        /// <summary>
        /// 从一个list中取出随机的元素
        /// </summary>
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default(T);
            RandomBuilder rand = new RandomBuilder();
            return list[rand.RandomInt(0, list.Count - 1)];
        }

        #region ElementsMix
        /// <summary>
        /// 使list元素随机混淆
        /// </summary>
        public static List<T> Mix<T>(this List<T> list)
        {
            var temp = new List<T>();
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                var item = list.GetRandom();
                var index = list.IndexOf(item);
                temp.Add(item);
                list.RemoveAt(index);
            }
            return temp;
        }

        /// <summary>
        /// 使数组随机混淆
        /// </summary>
        public static T[] Mix<T>(this T[] arr)
        {
            var temp = new List<T>();
            var list = arr.ToList();
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                var item = list.GetRandom();
                var index = list.IndexOf(item);
                temp.Add(item);
                list.RemoveAt(index);
            }
            return temp.ToArray();
        }

        /// <summary>
        /// 二维交错数组随机混淆
        /// </summary>
        public static T[][] Mix<T>(this T[][] arrs)
        {
            var list = arrs.ToList();
            var temp = new List<T[]>();
            foreach (var i in list)
            {
                list.Add(i.Mix());
            }
            return list.Mix().ToArray();
        }

        /// <summary>
        /// 二维矩阵数组随机混淆
        /// </summary>
        public static T[,] Mix<T>(this T[,] arrs)
        {
            var list = arrs.ToList();
            var temp = new List<List<T>>();
            foreach (var i in list)
            {
                temp.Add(i.Mix());
            }
            return temp.Mix().ToMatrixArray();
        }

        /// <summary>
        /// 二维List随机混淆
        /// </summary>
        public static List<List<T>> Mix<T>(this List<List<T>> lists)
        {
            var temp = new List<List<T>>();
            foreach (var i in lists)
            {
                temp.Add(i.Mix());
            }
            return temp.Mix();
        }
        #endregion


        /// <summary>
        /// 生成一个指定长度的类型默认值数组
        /// </summary>
        /// <returns></returns>
        public static T[] Repeat<T>(int len)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < len; i++)
            {
                list.Add(default(T));
            }
            return list.ToArray();
        }

        /// <summary>
        /// 截取一个数组里指定开始位置和结束位置的元素
        /// </summary>
        public static IEnumerable<T> Intercept<T>(this T[] tArr,int begin, int end)
        {
            if (begin > end || begin < 0 || end < 1 || begin >= tArr.Length || end >= tArr.Length)
            {
                yield return default(T);
                yield break;
            }
            for (int i = begin; i <= end; i++)
            {
                yield return tArr[i];
            }
        }

        /// <summary>
        /// 截取集合开始位置和结束位置中间的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tSource"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IEnumerable<T> Intercept<T>(this IEnumerable<T> tSource,int begin,int end)
        {
            return tSource.ToArray().Intercept(begin, end);
        }

        /// <summary>
        /// 返回符合条件的第一个元素或T类型的默认值
        /// </summary>
        public static T MatchFirst<T>(this IEnumerable<T> TSource, Func<T, bool> predicate)
        {
            IEnumerable<T> tRes = TSource.Where(predicate);
            if (tRes.Count() > 0)
                return tRes.First();
            return default(T);
        }

        /// <summary>
        /// 对集合元素进行遍历操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tSource"></param>
        /// <param name="doAction"></param>
        public static void ForAll<T>(this IEnumerable<T> tSource,Action<T> doAction)
        {
            foreach(var i in tSource)
            {
                doAction(i);
            }
        }

        /// <summary>
        /// 获得一个对象的深拷贝
        /// </summary>
        /// <typeparam name="T">需要拷贝的类型</typeparam>
        /// <param name="tSrc">拷贝源</param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T tSrc) where T : class, new()
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xml.Serialize(ms, tSrc);
                ms.Position = default(long);
                return (T)xml.Deserialize(ms);
            }
        }

        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tSrc"></param>
        /// <returns></returns>
        public static T ObjectClone<T>(this T tSrc) where T : class, new()
        {
            T dst = new T();
            try
            {
                foreach (var j in dst.GetType().GetProperties())
                {
                    foreach (var i in tSrc.GetType().GetProperties())
                    {
                        if (i.Name == j.Name && i.PropertyType == j.PropertyType && j.Name != "Error" && j.Name != "Item")
                            j.SetValue(dst, i.GetValue(tSrc, null), null);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dst;
        }
    }
}
