using System;
using System.Linq;
using System.Text;
using System.Management;
using System.Security.Cryptography;

namespace Halo_2_Launcher.Controllers
{
    public static class Security
    {
        public static string GetHardDriveSerial()
        {
            ManagementObjectSearcher WMI_Searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            string Serial = "";
            foreach (ManagementObject WMI_Object in WMI_Searcher.Get())
            {
                if (WMI_Object["SerialNumber"].ToString() != null)
                {
                    Serial = WMI_Object["SerialNumber"].ToString();
                    break;
                }
            }
            if (Serial != "")
                return CalculateMD5Hash(Serial.Trim());
            else
                return "No Serial?";
        }
        public static string CalculateMD5Hash(string input)
        {
            string result = "";
            using (MD5 hash = MD5.Create())
            {
                result = String.Join
                (
                    "",
                    from ba in hash.ComputeHash
                    (
                        Encoding.UTF8.GetBytes(input)
                    )
                    select ba.ToString("x2")
                );
            }
            return result;
        }
    }
}
