using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDSDK.GenericSupports.Extensions;

namespace FDSDK.Component.Diagnostics
{
    /// <summary>
    /// Debug便捷扩展
    /// </summary>
    public static class DebugSupports
    {
        /// <summary>
        /// 断言调试
        /// </summary>
        /// <param name="srcObj"></param>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Assert(this object srcObj, object obj, string message = null)
        {
            if (message == null)
                Debug.Assert(obj != null);
            else
                Debug.Assert(obj != null, message);
        }

        /// <summary>
        /// 断言调试
        /// </summary>
        public static void Assert<T>(this object srcObj, T tSrc, string message = null)
        {
            if (message == null)
                Debug.Assert(tSrc != null);
            else
                Debug.Assert(tSrc != null, message);
        }

        /// <summary>
        /// 断言调试
        /// </summary>
        public static void Assert(this object srcObj, bool bValue, string message = null)
        {
            if (message == null)
                Debug.Assert(bValue == true);
            else
                Debug.Assert(bValue == true, message);
        }

        /// <summary>
        /// 设置日志目录
        /// </summary>
        /// <param name="path"></param>
        public static void SetLogCatalog(string path)
        {
            NotDefaultLogFilePath = path;
        }

        /// <summary>
        /// 设置日志目录
        /// </summary>
        /// <param name="fileName"></param>
        public static void SetLogName(string fileName)
        {
            string logName = string.Empty;
            if (fileName.Contains('.'))
                logName = fileName.Split('.').First();
            NotDefaultLogFileName = logName + ".log";
        }

        /// <summary>
        /// 放置log信息
        /// </summary>
        /// <param name="message"></param>
        public static void PutLogMessage(string message)
        {
            var fileName = NotDefaultLogFileName == null || NotDefaultLogFileName == string.Empty ? DefaultLogFileName : NotDefaultLogFileName;
            var filePath = NotDefaultLogFilePath == null || NotDefaultLogFilePath == string.Empty ? "" : NotDefaultLogFilePath;
            var storePath = filePath == "" ? fileName : filePath + '/' + fileName;
            if (Directory.Exists(filePath) == false && filePath != null && filePath != string.Empty)
                Directory.CreateDirectory(filePath);
            if (File.Exists(storePath) == false)
                File.CreateText(storePath);
            else
                File.AppendAllLines(storePath, new string[] { "Debug: \nTime:" + DateTime.Now.ToString() + "\nMessage:" + message });
        }

        /// <summary>
        /// 对象信息打印
        /// </summary>
        /// <param name="obj"></param>
        public static void PutLogMessage(object obj)
        {
            var fileName = NotDefaultLogFileName == null || NotDefaultLogFileName == string.Empty ? DefaultLogFileName : NotDefaultLogFileName;
            var filePath = NotDefaultLogFilePath == null || NotDefaultLogFilePath == string.Empty ? "" : NotDefaultLogFilePath;
            var storePath = filePath == "" ? fileName : filePath + '/' + fileName;
            if (Directory.Exists(filePath) == false && filePath != null && filePath != string.Empty)
                Directory.CreateDirectory(filePath);
            if (File.Exists(storePath) == false)
                File.CreateText(storePath);
            else
                File.AppendAllLines(storePath, new string[] { "Debug: \nTime:" + DateTime.Now.ToString() + "\nMessage:" + obj.ToJson() });
        }

        /// <summary>
        /// 默认日志文件名
        /// </summary>
        public const string DefaultLogFileName = "debug.log";

        /// <summary>
        /// 非默认日志文件名
        /// </summary>
        public static string NotDefaultLogFileName
        {
            get;
            private set;
        }

        /// <summary>
        /// 非默认日志文件名
        /// </summary>
        public static string NotDefaultLogFilePath
        {
            get;
            private set;
        }

        /// <summary>
        /// 万能调试
        /// </summary>
        /// <param name="text"></param>
        /// <param name="param"></param>
        public static void PutDebug(this string text, params object[] param)
        {
            Console.WriteLine(text, param);
        }

        /// <summary>
        /// 万能调试
        /// </summary>
        /// <param name="obj"></param>
        public static void PutJsonDebug(this object obj)
        {
            Console.WriteLine(obj.ToJson());
        }
    }
}
