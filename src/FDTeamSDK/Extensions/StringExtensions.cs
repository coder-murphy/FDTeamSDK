using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using FDSDK.Define;
using System.Text.RegularExpressions;

namespace FDSDK.Extensions
{
    /// <summary>
    /// 字符串扩展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 转换为浮点数组
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static float[] ToFloatArray(this string strIn)
        {
            if (strIn.isDecimal() == false)
                return null;
            var strArray = strIn.Split(',');
            var outFloats = new List<float>();
            for (int i = 0; i < strArray.Length; i++)
            {
                outFloats.Add(Convert.ToSingle(strArray[i]));
            }
            return outFloats.ToArray();
        }

        /// <summary>
        /// 浮点数转换为字符串数组
        /// </summary>
        public static string[] ToStringArray(this float[] arr)
        {
            var list = new List<string>();
            foreach (var i in arr)
            {
                list.Add(i.ToString());
            }
            return list.ToArray();
        }
        /// <summary>
        /// 将字符串数组通过指定连接符连接起来
        /// </summary>
        public static string Connect(this string[] arr, char splitChar)
        {
            string outStr = "";
            foreach (var i in arr)
            {
                outStr += (i + splitChar);
            }
            return outStr;
        }

        /// <summary>
        ///  是否为数字
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool isDecimal(this string strIn)
        {
            bool result = true;
            foreach (var i in strIn)
            {
                if (DefineObjects.DecimalChars.Contains(i) != true)
                    result = false;
            }
            return result;
        }

        /// <summary>
        /// 将所有stringlist里的元素遍历为字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string AsString(this List<string> list)
        {
            var str = "";
            foreach (var i in list)
            {
                str += i;
            }
            return str;
        }

        /// <summary>
        /// 将字符串转换为IP地址（示例 AsIPAddress("192.168.1.1"); ）
        /// </summary>
        /// <param name="ipStr"></param>
        /// <returns></returns>
        public static byte[] AsIPAddress(this string ipStr)
        {
            var arr = ipStr.Split('.');
            if (arr.Length != 4 || ipStr.isDecimal() == false || (ipStr.isDecimal() && ipStr.Contains(",")))
                return new byte[] { 0, 0, 0, 0 };
            var byteArray = new List<byte>();
            foreach (var b in arr)
            {
                var conv = Convert.ToInt32(b);
                if (conv > 0x000000ff)
                    conv = 0x000000ff;
                byteArray.Add(Convert.ToByte(conv));
            }
            return byteArray.ToArray();
        }

        /// <summary>
        /// 将比特流转换成字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string AsString(this byte[] bytes)
        {  
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///  将字符串转化为比特流
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] AsBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 过滤含空字符串的数组
        /// </summary>
        /// <param name="strs">传入字符串</param>
        /// <returns></returns>
        public static string[] NullFilter(this string[] strs)
        {
            var list = new List<string>();
            foreach (var i in strs)
            {
                if (i == null)
                    continue;
                list.Add(i);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 过滤指定字符数组内容字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="signs"></param>
        /// <returns></returns>
        public static string Filter(this string str, char[] signs)
        {
            string outStr = "";
            foreach (var i in str)
            {
                if (signs.Contains(i))
                    continue;
                outStr += i;
            }   
            return outStr;
        }

        /// <summary>
        /// 过滤指定字符字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string Filter(this string str, char sign)
        {
            string outStr = "";
            foreach (var i in str)
            {
                if (i == sign)
                    continue;
                outStr += i;
            }
            return outStr;
        }

        /// <summary>
        /// 根据指定字符数组分割一个字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="signs"></param>
        /// <returns></returns>
        public static string[] SplitFilter(this string str, char[] signs)
        {
            if (signs == null || signs.ToList().Count == 0)
                return new string[] { str };
            var tempList = new List<string>();
            string tempStr = "";
            int index = 0;
            foreach (char i in str)
            {
                if (signs.Contains(i) != true)
                {
                    tempStr += i; 
                }
                if ((signs.Contains(i) && tempStr.Length != 0) || 
                    (index == str.Length - 1 && signs.Contains(i) != true))
                {
                    tempList.Add(tempStr);
                    tempStr = "";
                }
                index++;
            }
            var outArr = tempList.ToArray();
            return outArr;
        }

        /// <summary>
        /// 将字符串转换为整数
        /// </summary>
        public static int AsInt(this string str)
        {
            int result = 0;
            int.TryParse(str,out result);
            return result;
        }
        /// <summary>
        /// 将字符串转换为单精度浮点型并保留指定位数
        /// digits：指定保留位数
        /// </summary>
        public static float AsFloat(this string str,int digits)
        {
            return (float)str.AsDouble(digits);
        }

        /// <summary>
        /// 将字符串转换为单精度浮点型并保留指定位数
        /// digits：指定保留位数
        /// </summary>
        public static double AsDouble(this string str, int digits)
        {
            string outStr = str;
            double result = 0;
            double.TryParse(outStr, out result);
            return Math.Round(result, digits);
        }

        /// <summary>
        /// 转换为中文时间字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string AsLongDateString(this DateTime time)
        {
            return time.ToLongDateString().ToString();
        }

        /// <summary>
        /// 去掉双反斜杠
        /// </summary>
        public static string DouBackSlashRem(this string str)
        {
            return str.Replace('\\', '/');
        }

        /// <summary>
        /// 根据定义标志判断字符串是否合法
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <param name="judgeOption">定义合法模式</param>
        /// <param name="len">字符串限制长度</param>
        /// <returns></returns>
        public static bool IsLegalStringByDefinePattern(this string inStr, RegexPatternOptions judgeOption, uint len = 20)
        {
            if (string.IsNullOrWhiteSpace(inStr))
                throw new ArgumentException("参数字符串非法");
            if (len < 1)
                throw new ArgumentException("字符串长度不能为0");
            string pattern = string.Empty;
            bool spaceOption = false;
            if (judgeOption == RegexPatternOptions.OnlyAlphabet)
            {
                pattern = @"[^[A-Za-z]+$]{1," + len.ToString() + "}";
            }
            else if (judgeOption == RegexPatternOptions.OnlyAlphabetNumber)
            {
                pattern = @"[^[A-Za-z0-9]+$]{1," + len.ToString() + "}";
            }
            else if (judgeOption == RegexPatternOptions.OnlyAlphabetNumberUnderline)
            {
                pattern = @"^[A-Za-z0-9_]{1," + len.ToString() + "}$";
            }
            else if (judgeOption == RegexPatternOptions.OnlyNumber)
            {
                pattern = @"[^[0-9]+$]{1," + len.ToString() + "}";
            }
            else if (judgeOption == RegexPatternOptions.OnlyAlphabetNoSpace)
            {
                pattern = @"[^[A-Za-z]+$]{1," + len.ToString() + "}";
                spaceOption = true;
            }
            else if (judgeOption == RegexPatternOptions.OnlyAlphabetNumberNoSpace)
            {
                pattern = @"[^[A-Za-z0-9]+$]{1," + len.ToString() + "}";
                spaceOption = true;
            }
            else if (judgeOption == RegexPatternOptions.OnlyAlphabetNumberUnderlineNoSpace)
            {
                pattern = @"^[A-Za-z0-9_]{1," + len.ToString() + "}$";
                spaceOption = true;
            }
            else if (judgeOption == RegexPatternOptions.OnlyNumberNoSpace)
            {
                pattern = @"[^[0-9]+$]{1," + len.ToString() + "}";
                spaceOption = true;
            }
            Regex regex = new Regex(pattern);
            if (spaceOption == false)
                return regex.IsMatch(inStr);
            else
                return regex.IsMatch(inStr) && inStr.Contains(' ') == false;
        }

        /// <summary>
        /// 将字符串序列中的字符串拼接成一个字符串
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="splitSign"></param>
        /// <returns></returns>
        public static string ConcatString(IEnumerable<string> strings, string splitSign = "")
        {
            var list = strings.ToList();
            string temp = string.Empty;
            list.ForEach(x =>
            {
                temp += (x + splitSign);
            });
            if (splitSign != null && splitSign.Length > 0)
            {
                temp.Remove(temp.Length - 1);
            }
            return temp;
        }

        /// <summary>
        /// 获取文件路径的的上一级文件夹
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string AbsFilePathGetRootFolder(string fileName)
        {
            string folder = string.Empty;
            List<string> pathSplit = new List<string>();
            if (fileName.Contains('.'))
            {
                folder = fileName.Split('.').First();
            }
            else
            {
                folder = fileName;
            }
            pathSplit = folder.Replace('\\', '/').Split('/').ToList();
            if (pathSplit.Count > 0)
                pathSplit.RemoveAt(pathSplit.Count - 1);
            folder = string.Empty;
            for (int i = 0; i < pathSplit.Count; i++)
            {
                folder += (pathSplit[i] + '/');
            }
            return folder;
        }

        /// <summary>
        /// 根据谓词过滤字符串中字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static string Filter(this string str,Predicate<char> predicate)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var i in str)
                if (predicate(i) == false) sb.Append(i);
            return sb.ToString();
        }

    }

    /// <summary>
    /// 正则匹配选项
    /// </summary>
    public enum RegexPatternOptions
    {
        /// <summary>
        /// 只包含数字字母
        /// </summary>
        OnlyAlphabetNumber,
        /// <summary>
        /// 只包含字母
        /// </summary>
        OnlyAlphabet,
        /// <summary>
        /// 只包含数字
        /// </summary>
        OnlyNumber,
        /// <summary>
        /// 只包含数字字母下划线
        /// </summary>
        OnlyAlphabetNumberUnderline,
        /// <summary>
        /// 只包含数字字母(不包含空格)
        /// </summary>
        OnlyAlphabetNumberNoSpace,
        /// <summary>
        /// 只包含字母(不包含空格)
        /// </summary>
        OnlyAlphabetNoSpace,
        /// <summary>
        /// 只包含数字(不包含空格)
        /// </summary>
        OnlyNumberNoSpace,
        /// <summary>
        /// 只包含数字字母下划线(不包含空格)
        /// </summary>
        OnlyAlphabetNumberUnderlineNoSpace,
    }
}
