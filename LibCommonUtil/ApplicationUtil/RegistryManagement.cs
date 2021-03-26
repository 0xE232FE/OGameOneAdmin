using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace LibCommonUtil
{
    sealed public class RegistryManagement
    {
        /***********************************************************************************************************/


        #region ------ Public Static Methods ------


        public static string Read(RegistryKey baseRegistryKey, string subKey, string KeyName)
        {
            // Opening the registry key
            RegistryKey rk = baseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    return (string)sk1.GetValue(KeyName);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        public static bool Write(RegistryKey baseRegistryKey, string subKey, string KeyName, object Value)
        {
            try
            {
                RegistryKey rk = baseRegistryKey;

                RegistryKey sk1 = rk.CreateSubKey(subKey);

                sk1.SetValue(KeyName, Value);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool DeleteKey(RegistryKey baseRegistryKey, string subKey, string KeyName)
        {
            try
            {
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // If the RegistrySubKey doesn't exists -> (true)
                if (sk1 == null)
                    return true;
                else
                    sk1.DeleteValue(KeyName);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool DeleteSubKeyTree(RegistryKey baseRegistryKey, string subKey)
        {
            try
            {
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists, I delete it
                if (sk1 != null)
                    rk.DeleteSubKeyTree(subKey);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static int SubKeyCount(RegistryKey baseRegistryKey, string subKey)
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.SubKeyCount;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static int ValueCount(RegistryKey baseRegistryKey, string subKey)
        {
            try
            {
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.ValueCount;
                else
                    return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        #endregion ------ Public Static Methods ------


        /***********************************************************************************************************/
    }
}
