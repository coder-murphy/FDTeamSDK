using System;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using FDSDK.Define;
using FDSDK.GenericSupports.Extensions;

namespace FDSDK.WebSupports
{
    /// <summary>
    /// 远程请求
    /// </summary>
    public class RemoteRequest
    {
        private static int _RequestTimeout = 2000;
        /// <summary>
        /// 设置请求的响应过期时间(最低100
        /// </summary>
        public static int RequestTimeout
        {
            get { return _RequestTimeout; }
            set
            {
                int finalValue = value < 100 ? 100 : value;
                _RequestTimeout = finalValue;
            }
        }

        /// <summary>
        /// 生成供远程使用的json串
        /// </summary>
        /// <param name="paramNames"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static string ConvertToJson(string[] paramNames, string[] paramValue)
        {
            if (paramNames.Length != paramValue.Length)
            {
                Console.WriteLine("参数名称跟参数值数量不一样！");
                return null;
            }
            string outJson = "{";
            string jsonTail = "}";
            for (int i = 0; i < paramNames.Length; i++)
            {
                outJson += "\"" + paramNames[i] + "\":\"" + paramValue[i] + "\"";
                if (i < paramNames.Length - 1)
                    outJson += ",";
            }
            outJson += jsonTail;
            return outJson;
        }

        /// <summary>
        /// 通过http请求进行请求数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dstForm"></param>
        /// <param name="URL"></param>
        /// <param name="param">如果请求方式为Text则为无效参数，如果请求方式为Json则为json串</param>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <param name="showMessage"></param>
        /// <returns></returns>
        public static T LoadResultData<T>(Form dstForm, string URL, HttpParam param, Method method, ContentType contentType = ContentType.Json,bool showMessage = false)
        {
            string tempStr = null;
            try
            {
                byte[] data = new byte[1];
                if (contentType == ContentType.Text)
                {
                    if (param != null && param.Params.Count > 0)
                    {
                        URL += '?';
                        for (int i = 0; i < param.Params.Count; i++)
                        {
                            if (i != param.Params.Count - 1)
                                URL += param.Params[i].ToString() + '&';
                            else
                                URL += param.Params[i].ToString();
                        }
                    };
                }
                else if (contentType == ContentType.Json)
                {
                    data = Encoding.UTF8.GetBytes(param.Json);
                }
                // 建立http请求
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URL);
                webRequest.Method = DefineObjects.MethodPair[method];
                webRequest.ContentType = DefineObjects.ContentTypePair[contentType];
                webRequest.Proxy = null;
                webRequest.Timeout = RequestTimeout;
                webRequest.ContentLength = data.Length;
                using (Stream reqStream = webRequest.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }

                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                var stream = response.GetResponseStream();

                // 获取响应内容
                StreamReader sr0 = new StreamReader(stream, Encoding.UTF8);
                tempStr = sr0.ReadToEnd();
                stream.Close();
                sr0.Close();
                T resultData = tempStr.JsonToObject<T>();
                return resultData;
            }
            catch (WebException ex)
            {
                if (showMessage && dstForm != null)
                    dstForm.Invoke(new Action(() => { MessageBox.Show(dstForm, "从服务器拉取数据时出错！ " + ex.Message, "通知", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }));
                return default(T);
            }

        }

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="dstForm"></param>
        /// <param name="URL"></param>
        /// <param name="param"></param>
        /// <param name="contentType"></param>
        /// <param name="showMessage"></param>
        /// <returns></returns>
        public static bool UploadData(Form dstForm, string URL, string param, ContentType contentType = ContentType.Json, bool showMessage = false)
        {
            try
            {
                if (contentType == ContentType.Text)
                {
                    URL += "?" + "condition=" + param;
                    param = "";
                }
                // 建立http请求
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URL);
                webRequest.Method = DefineObjects.MethodPair[Method.POST];
                webRequest.ContentType = DefineObjects.ContentTypePair[contentType];
                webRequest.Proxy = null;
                webRequest.Timeout = RequestTimeout;
                byte[] data = Encoding.UTF8.GetBytes(param);
                webRequest.ContentLength = data.Length;
                // 写入请求
                using (Stream reqStream = webRequest.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                var res = (HttpWebResponse)webRequest.GetResponse();
                var stream = res.GetResponseStream();
                // 获取响应内容
                StreamReader sr0 = new StreamReader(stream, Encoding.UTF8);
                var tempStr = sr0.ReadToEnd();
                stream.Close();
                sr0.Close();
                return true;
            }
            catch (WebException ex)
            {
                if (showMessage)
                    dstForm.Invoke(new Action(() => { MessageBox.Show(dstForm, "从服务器拉取数据时出错！ " + ex.Message, "通知", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }));
                return false;
            }
        }
    }

    /// <summary>
    /// 请求类型
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 请求基于Json
        /// </summary>
        Json = 0,
        /// <summary>
        /// 请求基于字符串
        /// </summary>
        Text = 1
    }

    /// <summary>
    /// 请求方法
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// Http请求Get
        /// </summary>
        GET = 0,
        /// <summary>
        /// Http请求Put
        /// </summary>
        PUT = 1,
        /// <summary>
        /// Http请求Post
        /// </summary>
        POST = 2
    }
}
