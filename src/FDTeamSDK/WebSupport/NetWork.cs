using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using FDSDK.Extensions;
using System.Threading;
using System.Net.NetworkInformation;

namespace FDSDK.WebSupports
{
    /// <summary>
    /// NetWork帮助类
    /// </summary>
    public class NetWorkHelper
    {
        /// <summary>
        /// 检测主机连接状态
        /// </summary>
        /// <param name="hostAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool CheckIPConnectionStatus(string hostAddress, int port = 80)
        {
            return GetInstance().CheckConnectionStatus(hostAddress, port, ReceiveTimeout);
        }

        private static int _ReceiveTimeout = 500;
        /// <summary>
        /// 设置请求的响应过期时间(最低100
        /// </summary>
        public static int ReceiveTimeout
        {
            get { return _ReceiveTimeout; }
            set
            {
                int finalValue = value < 100 ? 100 : value;
                _ReceiveTimeout = finalValue;
            }
        }


        /// <summary>
        /// 检测端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool CheckPortIsInUse(int port)
        {
            bool flag = false;
            IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProps.GetActiveTcpListeners();
            foreach(var i in ipEndPoints)
            {
                if(i.Port == port)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        #region private members
        // 回调需要
        private delegate bool NetWorkConnectionStateCallBack();
        private AsyncCallback asyncCallBack = null;

        /// <summary>
        /// 网络唯一实例
        /// </summary>
        protected static NetWorkHelper Instance = null;

        private static NetWorkHelper GetInstance()
        {
            if (Instance == null)
                Instance = new NetWorkHelper();
            return Instance;
        }

        /// <summary>
        /// 新检测网络链接方法
        /// </summary>
        /// <param name="hostAddress"></param>
        /// <param name="port"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private bool CheckConnectionStatusNeo(string hostAddress, int port, int timeout = 2000)
        {
            bool flag = false;
            string portS = port == -1 ? string.Empty : port.ToString();
            string url = $"https://{hostAddress}:{portS}/";
            try
            {
                TcpClient client = new TcpClient(hostAddress, port);
                client.SendTimeout = timeout;
                client.Close();
                flag = true;
                return flag;
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ ex.Message},目标Host:{url}");
                return flag;
            }
        }

        private bool CheckConnectionStatus(string hostAddress, int port = 8080, int timeout = 2000)
        {
            bool flag = false;
            // 建立异步回调
            var act = new NetWorkConnectionStateCallBack(() =>
            {
                IPAddress farIP = new IPAddress(hostAddress.AsIPAddress());
                IPEndPoint point = new IPEndPoint(farIP, port);
                TcpClient client = new TcpClient();
                client.SendTimeout = timeout;
                client.ReceiveTimeout = timeout;
                try
                {
                    client.Connect(point);
                    flag = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (flag == false)
                    Console.WriteLine("与服务器的连接状态：中断 目标IP:" + farIP.ToString());
                else
                    Console.WriteLine("与服务器的连接状态：开启 目标IP:" + farIP.ToString());
                return flag;
            });
            var iResult = act.BeginInvoke(asyncCallBack, null);
            var result = act.EndInvoke(iResult);
            return result;
        }
#endregion
    }
}
