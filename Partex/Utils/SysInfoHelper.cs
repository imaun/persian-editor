using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Diagnostics;

namespace Farcin.Editor.Utils
{
    public static class SysInfoHelper
    {

        public static SysWindowsInfo GetWindowsInfo() {
            var searcher = new ManagementObjectSearcher(
                "select * from Win32_OperatingSystem"
            );
            foreach(var obj in searcher.Get()) {
                return new SysWindowsInfo {
                    Title = obj["Caption"].ToString(),
                    Version = obj["Version"].ToString(),
                    User = Environment.UserName
                };
            }

            return null;
        }

        public static TimeSpan UpTime {
            get {
                using (var uptime = new PerformanceCounter("System", "System Up Time")) {
                    uptime.NextValue();       //Call this an extra time before reading its value
                    return TimeSpan.FromSeconds(uptime.NextValue());
                }
            }
        }

        public static string LocalizedUpTime {
            get {
                var uptime = UpTime;
                string result = string.Empty;
                if (uptime.Days > 0)
                    result += $"{uptime.Days} روز ";
                if (uptime.Hours > 0)
                    result += $"{uptime.Hours} ساعت ";
                if (uptime.Minutes > 0)
                    result += $"{uptime.Minutes} دقیقه ";
                if (uptime.Seconds > 0)
                    result += $"{uptime.Seconds} ثانیه";

                return result;
            }
        }
    }

    public class SysWindowsInfo
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string User { get; set; }
    }
}
