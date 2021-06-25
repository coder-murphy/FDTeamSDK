using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using FDSDK.Extensions;

namespace FDSDK.WebSupports
{
    /// <summary>
    /// FTP传输助手
    /// </summary>
    public class FTPHelper
    {
        //public string ConfFilePath = "E:\\ConfFile.txt";//配置文件默认路径
        /// <summary>
        /// 上传下载文件时,每次读取文件的大小
        /// </summary>
        public const int DefaultByteSize = 2048;//上传下载文件时,每次读取文件的大小

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
        /// 从ftp中删除文件夹
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="serverDirPath"></param>
        /// <param name="user"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public bool RemoveDirectory(string ip, string port, string serverDirPath, string user, string passWord)//PORT可以为""或者:8080
        {//测试OK
            try
            {
                if (ip == null)
                    return false;
                if (port == null)
                    port = "";//默认端口打开
                string[] nameList = GetFileListFromFTPDir(ip, port, serverDirPath, user, passWord);//获取的是完整路径
                if (nameList != null)//如果文件列表不为空
                {
                    foreach (var name in nameList)
                    {
                        if (name == "." || name == ".." || name == null || name == "")
                        {
                            continue;
                        }
                        else
                        {
                            if (Path.HasExtension(name))
                                DeleteFile(ip, port, serverDirPath + "/" + name, user, passWord);//删除文件
                            else
                                RemoveDirectory(ip, port, serverDirPath + "/" + name, user, passWord);//递归删除文件夹    
                        }
                    }
                }
                //移出当前空文件夹
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("FTP://" + ip + port + serverDirPath);
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.RemoveDirectory;
                request.Credentials = new NetworkCredential(user, passWord);
                request.Timeout = RequestTimeout;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse(); //接收响应,必须有
                response.Close();
                request.Abort();
                return true;//将文件名进行返回
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{serverDirPath}");
                return false;
            }
        }

        /// <summary>
        /// 判断ftp文件夹路径下是否存在某个文件夹名
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口</param>
        /// <param name="serverPath">ftp服务器路径</param>
        /// <param name="dirName">匹配的文件夹名称</param>
        /// <param name="user">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns>存在1,不存在0,异常-1</returns>
        public int DirExistFromFTP(string ip, string port, string serverPath, string dirName, string user, string passWord)
        {
            try
            {
                if (ip == null)
                    return -1;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("FTP://" + ip + port + serverPath);
                request.UseBinary = true;//设置传输方式                                                       //设置传输数据方式
                request.Method = WebRequestMethods.Ftp.ListDirectory;                              //设置客户端需要上传文件
                request.Credentials = new NetworkCredential(user, passWord);                      //设置FTP账号密码
                request.Timeout = RequestTimeout;
                FtpWebResponse Response = (FtpWebResponse)request.GetResponse();
                Stream ResponseStream = Response.GetResponseStream(); //获取远程响应流,并从中获取文件夹组的名称
                StreamReader sreader = new StreamReader(ResponseStream, Encoding.UTF8);//从读者流中读取流数据
                string nameList = sreader.ReadToEnd();
                //拆分nameList与DirName判段是否相同,如果相同返回true,如果不相同返回false
                string[] names = nameList.Replace("\r\n", "\n").Split('\n');
                foreach (var tmp in names)
                    if (tmp.Equals(dirName))//如果存在当前文件夹
                        return 1;//
                    else
                        continue;//如果不存在文件夹名称,比对下一个
                sreader.Close();
                ResponseStream.Close();//关闭响应流
                Response.Close();
                request.Abort();
                return 0;//如果不存在文件夹名称,返回false
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{serverPath}");
                return -1;
            }
        }

        /// <summary>
        /// 文件是否存在于FTP上
        /// </summary>
        /// <returns></returns>
        public bool FileExistFromFTP(string ip, string port, string serverPath, string user, string passWord)
        {
            try
            {
                if (ip == null)
                    return false;
                if (serverPath == null || serverPath.Length == 0)
                    return false;
                string temp = serverPath.Replace('\\', '/');
                string dir = StringExtensions.AbsFilePathGetRootFolder(temp);
                string fileName = temp.Split('/').Last();
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("FTP://" + ip + ':' + port + '/' + dir);
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(user, passWord);
                request.Timeout = RequestTimeout;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream(); //获取远程响应流,并从中获取文件夹组的名称
                StreamReader sreader = new StreamReader(responseStream, Encoding.UTF8);//从读者流中读取流数据
                string nameList = sreader.ReadToEnd();
                string[] names = nameList.Replace("\r\n", "\n").Split('\n');
                foreach (var tmp in names)
                    if (tmp.Equals(fileName))//如果存在当前文件夹
                        return true;//
                    else
                        continue;//如果不存在文件夹名称,比对下一个
                sreader.Close();
                responseStream.Close();//关闭响应流
                response.Close();
                request.Abort();
                return false;//如果不存在文件夹名称,返回false
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{serverPath}");
                return false;
            }
        }

        /// <summary>
        /// 在ftp服务器上创建文件夹
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="dirName"></param>
        /// <param name="user"></param>
        /// <param name="passWord"></param>
        /// <param name="forceAdd">指示是否以覆盖方式添加文件夹</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool CreateDirFromFTP(string ip, string port, string dirName, string user, string passWord,bool forceAdd = false)
        {
            try
            {
                if (ip == null)
                    return false;
                if (Path.HasExtension(dirName))//如果传入的路径有扩展名,退出
                    return false;
                //判断远程服务器当前路径存在不存在DirName
                string s2 = Path.GetFileNameWithoutExtension(dirName);
                string s1 = dirName.TrimEnd(s2.ToArray());
                //判断需要创建的文件夹下是否存在重名的文件夹
                int ok = DirExistFromFTP(ip, port, s1.Replace("\\", "/"), s2, user, passWord);
                if (ok == 1 || ok == -1)//如果重名文件夹存在,返回-1退出
                {
                    if(forceAdd == false)
                        return false;//创建文件夹失败
                    string path = s1.Replace("\\", "/") + s2;
                    RemoveDirectory(ip, port, path, user, passWord);
                }
                string url = "FTP://" + ip + port + "/" + dirName;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.UseBinary = true;//设置传输方式                                                       //设置传输数据方式
                request.Method = WebRequestMethods.Ftp.MakeDirectory;                              //设置客户端需要上传文件
                request.Credentials = new NetworkCredential(user, passWord);                      //设置FTP账号密码
                request.Timeout = RequestTimeout;
                FtpWebResponse Response = (FtpWebResponse)request.GetResponse(); //获取远程响应
                Response.Close();//关闭响应流
                return true;
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{dirName}");
                return false;
            }
        }

        /// <summary>
        /// 获取ftp当前文件夹路径下的所有文件或者文件夹名称
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口</param>
        /// <param name="serverDirPath">服务端文件夹路径</param>
        /// <param name="user">用户名</param>
        /// <param name="PASSWD">用户密码</param>
        /// <remarks>补充:如果要获取文件/文件夹详细信息, 请修改Request.Method = WebRequestMethods.Ftp.ListDirectoryDetail;   </remarks>
        /// <returns>返回当前路径下的所有文件或者文件夹名称</returns>
        public string[] GetFileListFromFTPDir(string ip, string port, string serverDirPath, string user, string PASSWD)
        {
            try
            {
                if (ip == null||Path.HasExtension(serverDirPath))//如果IP为空,或者传入的路径带有后缀
                    return null;
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("FTP://" + ip + port + serverDirPath + "/");
                request.UseBinary = true;//设置传输方式                                                       //设置传输数据方式
                request.Method = WebRequestMethods.Ftp.ListDirectory;                              //设置客户端需要上传文件
                request.Credentials = new NetworkCredential(user, PASSWD);                      //设置FTP账号密码
                request.Timeout = RequestTimeout;
                FtpWebResponse Response = (FtpWebResponse)request.GetResponse();
                Stream ResponseStream = Response.GetResponseStream(); //获取远程响应流,并从中获取文件夹组的名称
                StreamReader sreader = new StreamReader(ResponseStream, Encoding.UTF8);//从读者流中读取流数据
                string nameList = sreader.ReadToEnd();
                //拆分nameList与DirName判段是否相同,如果相同返回true,如果不相同返回false
                sreader.Close();
                ResponseStream.Close();//关闭响应流
                Response.Close();
                request.Abort();
                return nameList.Replace("\r\n", "\n").Split('\n');
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{serverDirPath}");
                return null;
            }
        }

        /// <summary>
        /// 上传文件到FTP
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="PORT"></param>
        /// <param name="ServerDirPath"></param>
        /// <param name="LocalFileName"></param>
        /// <param name="USER"></param>
        /// <param name="PASSWD"></param>
        /// <param name="isDir"></param>
        /// <returns>本地文件的文件名+后缀</returns>
        public string UpLoadFile(string IP,string PORT,string ServerDirPath,string LocalFileName,string USER,string PASSWD,bool isDir = false)//Local_Path中带文件名,PORT可以为""或者":8080/",Server_Path是服务器端的路径
        {//测试OK
            try
            {
                if (IP == null)
                {
                    MessageBox.Show("请检查IP地址");
                    return null;
                }
                if (PORT == null)
                    PORT = "";//默认端口打开
                FileStream filestream = new FileStream(LocalFileName, FileMode.Open, FileAccess.Read);//用文件流打开文件
                string[] PATH_Buf = LocalFileName.Split('/');//最后一个字符串是文件名
                bool doubleXG = LocalFileName.Contains("\\");
                if (doubleXG)
                {
                    PATH_Buf = LocalFileName.Split('\\');
                }
                string sChar = isDir ? "/" : string.Empty;
                string url = "FTP://" + IP + PORT + ServerDirPath + sChar + PATH_Buf[PATH_Buf.Length - 1];
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.UseBinary = true;                                                       //设置传输数据方式
                request.Method = WebRequestMethods.Ftp.UploadFile;                              //设置客户端需要上传文件
                request.Credentials = new NetworkCredential(USER, PASSWD);                      //设置FTP账号密码
                request.Timeout = RequestTimeout;
                Stream ResquestStream = request.GetRequestStream();                             //获取请求流用来上传数据
                byte[] BinaryArry = new byte[DefaultByteSize];
                int readSize = 0;
                while ((readSize = filestream.Read(BinaryArry, 0, BinaryArry.Length)) > 0)      //将fileStream读取到的数据依次上传到FTP
                {
                    ResquestStream.Write(BinaryArry, 0, readSize);
                    //此处用来加载进度条
                }
                ResquestStream.Close();
                //ResquestStream.Dispose();
                filestream.Close();
                return PATH_Buf[PATH_Buf.Length - 1];//需要发送给数据库作为保存
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{ServerDirPath}");
                return null;
            }
        }

        /// <summary>
        /// 上传文件到FTP
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="PORT"></param>
        /// <param name="Server_Path"></param>
        /// <param name="Local_Path_FileName"></param>
        /// <param name="USER"></param>
        /// <param name="PASSWD"></param>
        /// <returns></returns>
        public string UpLoadFile_g(string IP, string PORT, string Server_Path, string Local_Path_FileName, string USER, string PASSWD)//Local_Path中带文件名,PORT可以为""或者":8080/",Server_Path是服务器端的路径
        {
            try
            {
                if (IP == null)
                    return null;
                if (PORT == null)
                    PORT = "";//默认端口打开
                int readSize = 0;
                FileStream filestream = new FileStream(Local_Path_FileName, FileMode.Open, FileAccess.Read);//用文件流打开文件
                string[] PATH_Buf = Local_Path_FileName.Split('\\');//最后一个字符串是文件名
                string file_guid_name = AddGuidToFileName(PATH_Buf[PATH_Buf.Length - 1]);
                string newfile_guid_name = new UTF8Encoding().GetString(new UTF8Encoding().GetBytes(file_guid_name));
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("FTP://" + IP + PORT + Server_Path + newfile_guid_name);
                request.UseBinary = true;                                                       //设置传输数据方式
                request.Method = WebRequestMethods.Ftp.UploadFile;                              //设置客户端需要上传文件
                request.Credentials = new NetworkCredential(USER, PASSWD);                      //设置FTP账号密码
                request.Timeout = RequestTimeout;
                Stream ResquestStream = request.GetRequestStream();                             //获取请求流用来上传数据
                byte[] BinaryArry = new byte[DefaultByteSize];
                while ((readSize = filestream.Read(BinaryArry, 0, BinaryArry.Length)) > 0)      //将fileStream读取到的数据依次上传到FTP
                {
                    ResquestStream.Write(BinaryArry, 0, readSize);
                }
                ResquestStream.Close();
                ResquestStream.Dispose();
                filestream.Close();
                filestream.Dispose();
                return file_guid_name;//需要发送给数据库作为保存
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{Server_Path}");
                return null;
            }
        }

        /// <summary>
        /// 下载文件到本地(适用于用FileDialog类选择的文件)
        /// </summary>
        /// <param name="IP">地址</param>
        /// <param name="PORT">端口</param>
        /// <param name="ServerPathFileName">服务器端路径(文件名)</param>
        /// <param name="LocalPath">本地文件名</param>
        /// <param name="USER"></param>
        /// <param name="PASSWD"></param>
        /// <returns></returns>
        public bool DownLoadFile(string IP, string PORT, string ServerPathFileName, string LocalPath, string USER, string PASSWD)//
        {//测试OK
            string dstUrl = null;
            try
            {
                if (IP == null)
                    return false;
                if (PORT == null)
                    PORT = "";//默认端口打开
                if (ServerPathFileName == null)
                {
                    return false;
                }
                if (ServerPathFileName.Length == 0)
                {
                    return false;
                }
                if (ServerPathFileName.First() != '/' && ServerPathFileName.First() != '\\')
                    ServerPathFileName = '/' + ServerPathFileName;

                if (PORT == null)
                    dstUrl = "FTP://" + IP + ServerPathFileName;
                else
                    dstUrl = "FTP://" + IP + PORT + ServerPathFileName;
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(dstUrl);
                request.Timeout = RequestTimeout;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(USER, PASSWD);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                FileStream filestream = new FileStream(LocalPath, FileMode.Create, FileAccess.ReadWrite);
                byte[] ByteArray = new byte[DefaultByteSize];
                int readsize = 0;
                while ((readsize = stream.Read(ByteArray, 0, DefaultByteSize)) > 0)
                {
                    filestream.Write(ByteArray, 0, readsize);
                }
                request.Abort();
                stream.Close();
                stream.Dispose();
                response.Close();
                filestream.Close();
                filestream.Dispose();
                return true;
            }
            catch(WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{dstUrl}");
                return false;
            }
        }

        /// <summary>
        /// 下载ftp文件到本地文件夹
        /// </summary>
        /// <param name="IP">地址</param>
        /// <param name="PORT">端口</param>
        /// <param name="ServerPathFileName">服务器端文件具体路径</param>
        /// <param name="LocalDir">本地文件夹</param>
        /// <param name="USER">用户名</param>
        /// <param name="PASSWD">密码</param>
        /// <returns></returns>
        public bool DownLoadTmpFile(string IP, string PORT, string ServerPathFileName, string LocalDir, string USER, string PASSWD)//
        {
            try
            {
                if (IP == null)
                    return false;
                if (PORT == null)
                    PORT = "";//默认端口打开
                if (ServerPathFileName.Length == 0 || ServerPathFileName == null)
                    return false;
                if (ServerPathFileName.First() != '/' || ServerPathFileName.First() != '\\')
                    ServerPathFileName = '/' + ServerPathFileName;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("FTP://" + IP + PORT + ServerPathFileName);
                //request.Timeout = 100;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(USER, PASSWD);
                request.Timeout = RequestTimeout;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                FileStream filestream = new FileStream(LocalDir + "\\" +Path.GetFileName(ServerPathFileName), FileMode.Create, FileAccess.ReadWrite);
                byte[] ByteArray = new byte[DefaultByteSize];
                int readsize = 0;
                while ((readsize = stream.Read(ByteArray, 0, DefaultByteSize)) > 0)
                {
                    filestream.Write(ByteArray, 0, readsize);
                }
                request.Abort();
                stream.Close();
                stream.Dispose();
                response.Close();
                filestream.Close();
                filestream.Dispose();
                return true;
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{ServerPathFileName}");
                return false;
            }

        }

        /// <summary>
        /// 从ftp删除文件
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="PORT"></param>
        /// <param name="ServerFileName"></param>
        /// <param name="USER"></param>
        /// <param name="PASSWD"></param>
        /// <returns></returns>
        public bool DeleteFile(string IP, string PORT, string ServerFileName, string USER, string PASSWD)//PORT可以为""或者:8080
        {//测试OK
            try
            {
                if (IP == null)
                    return false;
                if (PORT == null)
                    PORT = "";//默认端口打开
                if (ServerFileName.First() != '/' || ServerFileName.First() != '\\')
                    ServerFileName = '/' + ServerFileName;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("FTP://" + IP + PORT + ServerFileName);
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(USER, PASSWD);
                request.Timeout = RequestTimeout;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse(); //接收响应,必须有
                response.Close();
                return true;//将文件名进行返回
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{ex.Message}目标FTP路径：{ServerFileName}");
                return false;
            }
        }

        /// <summary>
        /// 给文件名添加guid
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns>返回文件名+"_"+guid</returns>
        public string AddGuidToFileName(string FileName)                                         
        {
            if (FileName == null || FileName == "")  
                return null;
            string[] buf = FileName.Split('.');
            if (buf.Length != 2)
                return null;
            //test.txt->test_************.txt
            return buf[0] + "_" + System.Guid.NewGuid().ToString("N") + "." + buf[1];
        }
    }
}
