using System;
using Microsoft.Win32;

namespace Stugo.Config.Registry
{
    public static class RegistryHiveConverter
    {
        public static string ToString(RegistryHive hive)
        {
            switch (hive)
            {
                case RegistryHive.ClassesRoot:
                    return "HKEY_CLASSES_ROOT";
                case RegistryHive.CurrentConfig:
                    return "HKEY_CURRENT_CONFIG";
                case RegistryHive.CurrentUser:
                    return "HKEY_CURRENT_USER";
                case RegistryHive.DynData:
                    return "HKEY_DYN_DATA";
                case RegistryHive.LocalMachine:
                    return "HKEY_LOCAL_MACHINE";
                case RegistryHive.PerformanceData:
                    return "HKEY_PERFORMANCE_DATA";
                case RegistryHive.Users:
                    return "HKEY_USERS";
                default:
                    throw new ArgumentException($"Unknown registry hive {hive}");
            }
        }


        public static RegistryHive FromString(string hive)
        {
            switch (hive.ToUpper())
            {
                case "HKEY_CLASSES_ROOT":
                    return RegistryHive.ClassesRoot;
                case "HKEY_CURRENT_CONFIG":
                    return RegistryHive.CurrentConfig;
                case "HKEY_CURRENT_USER":
                    return RegistryHive.CurrentUser;
                case "HKEY_DYN_DATA":
                    return RegistryHive.DynData;
                case "HKEY_LOCAL_MACHINE":
                    return RegistryHive.LocalMachine;
                case "HKEY_PERFORMANCE_DATA":
                    return RegistryHive.PerformanceData;
                case "HKEY_USERS":
                    return RegistryHive.Users;
                default:
                    throw new ArgumentException($"Unknown registry hive {hive}");
            }
        }
    }
}
