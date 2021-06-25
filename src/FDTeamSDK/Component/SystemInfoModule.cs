using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Diagnostics;
using System.Timers;

namespace FDSDK.Component
{
    /// <summary>
    /// 系统信息模块
    /// </summary>
    public class SystemInfoModule : IFDCustomComponent
    {
        /// <summary>
        /// 新建一个系统信息模块
        /// </summary>
        public SystemInfoModule()
        {
            Initialization();
        }

        /// <summary>
        /// 计算机名
        /// </summary>
        public string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        /// <summary>
        /// 操作系统版本
        /// </summary>
        public string OSVersion
        {
            get
            {
                return Environment.OSVersion.VersionString;
            }
        }
        /// <summary>
        /// 总内存
        /// </summary>
        public string CapacityMemoryString { get; private set; }
        /// <summary>
        /// 可用内存
        /// </summary>
        public string AvaliableMemoryString { get; private set; }
        /// <summary>
        /// 已使用内存
        /// </summary>
        public string UsingMemoryString { get; private set; }
        /// <summary>
        /// CPU使用率
        /// </summary>
        public string CPUUsingRatio
        {
            get; private set;
        }

        /// <summary>
        /// 内存占用率
        /// </summary>
        public string MemoryUsingRatio
        {
            get; private set;
        }

        /// <summary>
        /// 处理器数目
        /// </summary>
        public int CPUCount
        {
            get { return Environment.ProcessorCount; }
        }

        /// <summary>
        /// .Net版本
        /// </summary>
        public string FrameWorkVersionString
        {
            get { return Environment.Version.ToString(); }
        }

        /// <summary>
        /// BIOS序列号
        /// </summary>
        public string BIOSSerialNumber
        {
            get; private set;
        }

        /// <summary>
        /// CPU主频率
        /// </summary>
        public string CPUFrequency
        {
            get; private set;
        }

        /// <summary>
        /// 系统信息事件响应
        /// </summary>
        /// <param name="obj"></param>
        public delegate void SystemInfoChangeHandler(SystemInfoModule obj);

        /// <summary>
        /// 系统信息改变事件
        /// </summary>
        public event SystemInfoChangeHandler SystemInfosUpdate = null;

        /// <summary>
        /// 获取总内存
        /// </summary>
        /// <returns></returns>
        public double GetMemoryTotal()
        {

            ObjectQuery objQuery = new ObjectQuery("select * from Win32_PhysicalMemory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(objQuery);
            ManagementObjectCollection vals = searcher.Get();
            double ramCapacity = 0;
            double totalCapacity = 0;
            foreach (var i in vals)
            {
                totalCapacity += Convert.ToDouble(i.GetPropertyValue("Capacity"));
            }
            ramCapacity = totalCapacity / 1048576;
            return ramCapacity;
        }

        /// <summary>
        /// 获取可用内存
        /// </summary>
        /// <returns></returns>
        public double GetMemoryAvailable()
        {
            double ramCapacity = 0;
            double totalCapacity = 0;
            ObjectQuery objQuery = new ObjectQuery("select * from Win32_PerfRawData_PerfOS_Memory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(objQuery);
            ManagementObjectCollection vals = searcher.Get();
            foreach (var i in vals)
            {
                totalCapacity += Convert.ToDouble(i.GetPropertyValue("Availablebytes"));
            }
            ramCapacity = totalCapacity / 1048576;
            return ramCapacity;
        }

        /// <summary>
        /// 获取CPU频率
        /// </summary>
        /// <returns></returns>
        public double GetCPUFrequency()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            var list = new List<double>();
            foreach (var i in searcher.Get())
            {
                list.Add(Convert.ToSingle(i.Properties["CurrentClockSpeed"].Value));
            }
            return list.First() / 1000;
        }

        /// <summary>
        /// 获取BIOS序列号
        /// </summary>
        /// <returns></returns>
        public string GetBIOSSerialNumber()
        {
            ManagementClass cla = new ManagementClass("Win32_BIOS");
            ManagementObjectCollection collection = cla.GetInstances();
            string biosId = string.Empty;
            foreach (var i in collection)
            {
                biosId = i.Properties["SerialNumber"].Value.ToString();
                break;
            }
            return biosId;
        }

        /// <summary>
        /// 获取bios时钟信息
        /// </summary>
        /// <returns></returns>
        public string GetBIOSTimeSpanInfo()
        {
            ManagementClass cla = new ManagementClass("Win32_BIOS");
            TimeSpan time = cla.Options.Timeout;
            double day = time.Days;
            double hour = time.Hours;
            double minute = time.Minutes;
            double second = time.Seconds;
            double millsecond = time.Milliseconds;
            var str = "{0}:{1}:{2}:{3}:{4}";
            return string.Format(str, day, hour, minute, second, millsecond);
        }

        /// <summary>
        /// BIOS时钟信息
        /// </summary>
        public string BIOSTimeSpanInfo
        {
            get; private set;
        }

        private void OnSystemInfosUpdate()
        {
            if (SystemInfosUpdate != null)
                SystemInfosUpdate(this);
        }

        private void Initialization()
        {
            double mt = GetMemoryTotal();
            double ma = GetMemoryAvailable();
            CapacityMemoryString = Math.Round(mt, 3).ToString() + "MB";
            AvaliableMemoryString = Math.Round(ma, 3).ToString() + "MB";
            UsingMemoryString = Math.Round(mt - ma, 3).ToString() + "MB";
            MemoryUsingRatio = Math.Round((mt - ma) * 100 / mt, 3).ToString() + "%";
            CPUFrequency = Math.Round(GetCPUFrequency(), 3).ToString() + "GHz";
            BIOSSerialNumber = GetBIOSSerialNumber();
            BIOSTimeSpanInfo = GetBIOSTimeSpanInfo();
            RefershInfos();
            OnSystemInfosUpdate();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (s, e) =>
            {
                RefershInfos();
                OnSystemInfosUpdate();
            };
            timer.Start();
        }

        private Timer timer = null;
        private PerformanceCounter cpuCounter = null;
        private PerformanceCounter ramCounter = null;

        private void RefershInfos()
        {
            double mt = GetMemoryTotal();
            double ma = GetMemoryAvailable();
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor WHERE Name=\"_Total\"");
            var cpuItem = searcher.Get().Cast<ManagementObject>().Select(item => new { PercentProcessorTime = item["PercentProcessorTime"] }).First();
            if (cpuItem != null)
            {
                CPUUsingRatio = cpuItem.PercentProcessorTime.ToString() + "%";
            }
            AvaliableMemoryString = Math.Round(ma, 3).ToString() + "MB";
            UsingMemoryString = Math.Round(mt - ma, 3).ToString() + "MB";
            MemoryUsingRatio = Math.Round((mt - ma) * 100 / mt, 3).ToString() + "%";
            BIOSTimeSpanInfo = GetBIOSTimeSpanInfo();
        }
    }
}
