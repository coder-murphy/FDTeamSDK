using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FDSDK.Extensions
{
    /// <summary>
    /// 流扩展方法集
    /// </summary>
    public static class StreamExtensions
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("Kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private const int OF_READWRITE = 2;
        private const int OF_SHARE_DENY_NONE = 0x40;
        private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 检测文件是否被占用
        /// </summary>
        public static bool IsFileOccupied(string fileName)
        {
            IntPtr handle = _lopen(fileName, OF_READWRITE | OF_SHARE_DENY_NONE);
            CloseHandle(handle);
            return handle == HFILE_ERROR ? true : false;
        }

        /// <summary>
        /// 字符串转换为流
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Stream AsStream(this string inStr,Encoding encoding)
        {
            byte[] bytes = new byte[1];
            bytes = encoding.GetBytes(inStr);
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// 将流转换为字符串
        /// </summary>
        /// <returns></returns>
        public static string AsString(this Stream inStream,Encoding encoding)
        {
            var reader = new StreamReader(inStream, encoding);
            return reader.ReadToEnd();
        }

#region ImageProcess
        /// <summary>
        /// 将图像转化为比特流
        /// </summary>
        public static byte[] ConvertImageToBytes(this Image img)
        {
            if (img == null)
                return null;
            // 图像装入byte流
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img,typeof(byte[]));
        }

        /// <summary>
        /// 将比特流转化成图像
        /// </summary>
        public static Image ConvertBytesToImage(this byte[] bytes)
        {
            try
            {
                // 创建比特流的内存流
                var ms = new MemoryStream(bytes);
                var img = Image.FromStream(ms);
                return img;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 图像转化为Base64
        /// </summary>
        public static string ToBase64String(this Image img)
        {
            string strOut = string.Empty;
            var bytes = img.ConvertImageToBytes();
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64转化为图片
        /// </summary>
        public static Image Base64ToImage(this string base64Str)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64Str);
                var ms = new MemoryStream(bytes);
                return Image.FromStream(ms);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 图片大小不能超过
        /// </summary>
        /// <param name="img"></param>
        /// <param name="defaultWidth"></param>
        /// <param name="defaultHeight"></param>
        /// <returns></returns>
        public static bool JudgeImageSizeExceed(this Image img, int defaultWidth = 256, int defaultHeight = 256)
        {
            return img.Width <= defaultWidth && img.Height <= defaultHeight;
        }
#endregion

        /// <summary>
        /// 从文件中读取对象
        /// </summary>
        public static T LoadJsonFile<T>(string path)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                using (StreamReader reader = File.OpenText(path))
                {
                    var info = reader.ReadToEnd();
                    var obj = serializer.Deserialize<T>(info);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show(ex.Message);
                return default(T);
            }
        }

    }
}
