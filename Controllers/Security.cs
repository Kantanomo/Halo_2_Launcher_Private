using System;
using System.Linq;
using System.Text;
using System.Management;
using System.Security.Cryptography;

namespace Halo_2_Launcher.Controllers
{
    public static class Security
    {
        public static string GetSerial()
        {
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            string text = "";
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ManagementObject managementObject = (ManagementObject)enumerator.Current;
                    var serial = managementObject["SerialNumber"];
                    bool flag = serial != null;
                    if (flag)
                    {
                        text = serial.ToString();
                        break;
                    }
                }
            }
            managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_processor");
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ManagementObject managementObject = (ManagementObject)enumerator.Current;
                    var serial = managementObject["ProcessorID"].ToString();
                    bool flag = serial != null;
                    if(flag)
                    {
                        text += serial.ToString();
                        break;
                    }
                }
            }
            return CalculateMD5Hash(text);
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
