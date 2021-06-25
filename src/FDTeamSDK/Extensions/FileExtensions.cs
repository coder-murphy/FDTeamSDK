using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FDSDK.WebSupports;
using FDSDK.Extensions;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using FDSDK.GenericSupports.Extensions;

namespace FDSDK.Extensions
{
    /// <summary>
    /// 文件扩展方法集(developer：hujiamin)
    /// </summary>
    public static class FileExtensions //从FTP上传下载删除文件 
    {
        /// <summary>
        /// 在本地路径下查找以sign为后缀的文件路径(供平台三维模型使用)
        /// </summary>
        /// <param name="LocalDirPath">DirPath为文件夹路径</param>
        /// <param name="sign"></param>
        /// <returns>返回以sign结尾的文件名 用来作为远程服务器创建文件夹的名称</returns>
        public static string GetFilePathOfExtension(string LocalDirPath,string sign)//"\*\*"  .gif   /   .mdl
        {
            if (LocalDirPath == null || Path.HasExtension(LocalDirPath))//如果当前传入的文件夹路径为空或者有后缀,退出
                return null;
            foreach (var file in Directory.GetFiles(LocalDirPath))
            {
                if (Path.GetExtension(file) == sign)//如果当前文件是以mdl结尾
                    return file;//将当前查找到的文件名称作为返回值
            }
            return null;//如果未找到.gif格式文件
        }

        /// <summary>
        /// 读取当前文件的大小
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns>文件大小</returns>
        public static long FileSize(string filename)
        {
            if (!File.Exists(filename))
                return -1;//当前文件不存在
            FileInfo info=new FileInfo(filename);
            return info.Length;
        }

        /// <summary>
        /// 读取小文件字节流,  图标转换方法Image image=new MemoryStream(ReadPictureFile(xx/xx/xx))
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns>返回文件字节流</returns>
        public static byte[] ReadFile(string filename)
        {
            if (filename == null || filename == "")
                return null;
            if (File.Exists(filename))
            {
                FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                int byteLength = (int)fileStream.Length;
                byte[] fileBytes = new byte[byteLength];
                fileStream.Read(fileBytes, 0, byteLength);
                fileStream.Close();
                fileStream.Dispose();
                return fileBytes;
            }
            return null;
        }
        /// <summary>
        ///功能:从FTP服务器下载文件到本地
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="PORT"></param>
        /// <param name="USER">用户</param>
        /// <param name="PWD">密码</param>
        /// <param name="serverpathfilename">文件在FTP服务器的路径+文件名</param>
        /// <param name="tmppath">tmppath本地路径</param>
        /// <returns>本地路径+文件名</returns>
        public static string DownLoadFileToLocal(string IP,string PORT,string USER,string PWD,string serverpathfilename, string tmppath)//从FTP下载图标文件到本地,用完之后请调用DeleteTMPIconFile(string)进行删除
        {
            FTPHelper ftp = new FTPHelper();
            bool ok = ftp.DownLoadTmpFile(IP, PORT, serverpathfilename, tmppath, USER, PWD);
            if (ok == false)
                return null;
            else
                return tmppath + "\\" + Path.GetFileName(serverpathfilename);
        }

        /// <summary>
        /// 上传文件到FTP
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="PORT"></param>
        /// <param name="USER"></param>
        /// <param name="PWD"></param>
        /// <param name="FileName">FileName本地文件路径</param>
        /// <param name="ServerPath">ServerPath服务器路径</param>
        /// <returns>文件名+guid</returns>
        public static string UploadFileToFTP(string IP, string PORT, string USER, string PWD, string FileName, string ServerPath)//上传文件到ftp
        {
            if (IP == null)
                return null;
            if (ServerPath == null || ServerPath == "")
                return null;
            FTPHelper ftp = new FTPHelper();
            if (File.Exists(FileName))
            {
                string nameguid = ftp.UpLoadFile(IP, PORT, ServerPath, FileName, USER, PWD);
                //上传之后返回的时 文件名_guid
                return FileName + nameguid;
            }
            return null;
        }

        /// <summary>
        /// 从本地删除或者从FTP上删除文件
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="PORT"></param>
        /// <param name="USER"></param>
        /// <param name="PWD"></param>
        /// <param name="FilePath"></param>
        public static void DeleteFileFromServer(string IP, string PORT, string USER, string PWD, string FilePath)//从本地或者服务器路径删除文件
        {
            if (IP == null)//如果IP为空
            {
                if (File.Exists(FilePath))//检查本地路径上是否有文件
                    File.Delete(FilePath);//存在则删除文件
            }
            else//如果IP不为空
            {
                FTPHelper ftp = new FTPHelper();
                ftp.DeleteFile(IP, PORT, FilePath, USER, PWD);//从FTP服务器中删除文件
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        public static void CreateDirector(string dirPath)
        {
            if (!Directory.Exists(dirPath))//如果文件夹不存在创建文件加
                Directory.CreateDirectory(dirPath);
        }

        /// <summary>
        /// 保存json文件
        /// </summary>
        public static string SaveJsonFile<T>(this T type)
        {
            string json = type.ToJson();
            string path = "";
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "json文件(*.json)|*.json";
            dialog.Title = "请选择json文件保存位置";
            dialog.FileName = "新序列化文本";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = dialog.FileName.ToString();
                FileStream fs = new FileStream(path, FileMode.Create);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                fs.Write(bytes, 0, bytes.Length);
            }
            return path;
        }

        /// <summary>
        /// 从完整路径中获取文件名
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetFileNameFromFullPath(string fullPath)
        {
            if (fullPath.Split('.').Length != 2)
                return string.Empty;
            var fileName = fullPath.Replace('\\', '/').Split('/').Last();
            return fileName;
        }
    }
}
