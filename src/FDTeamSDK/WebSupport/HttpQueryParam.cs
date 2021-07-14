using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDSDK.WebSupports
{
    /// <summary>
    /// Http请求所用参数对
    /// </summary>
    public class HttpQueryParam
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 将参数对转化为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Key + '=' + Value;
        }
    }

    /// <summary>
    /// Http请求所用参数类
    /// </summary>
    public class HttpParam
    {
        private List<HttpQueryParam> _Params;
        /// <summary>
        /// Http请求对象所包含的参数列表(仅Text模式有效)
        /// </summary>
        public List<HttpQueryParam> Params
        {
            get {
                if (_Params == null)
                    _Params = new List<HttpQueryParam>();
                return _Params;
            }
            set
            {
                _Params = value;
            }
        }

        /// <summary>
        /// 参数所携带的Json串
        /// </summary>
        public string Json { get; set; }

        public object testc()
        {
            return RemoteRequest.LoadResultData<object>(null, "www.baidu.com", new HttpParam
            {
                Json = string.Empty,
                Params = new List<HttpQueryParam>
                { 
                    new HttpQueryParam
                    {
                        Key = "userID",
                        Value = "123456"
                    }
                }
            },
            Method.POST,
             ContentType.Text,
             false
             );
        }
    }
}
