using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;

namespace Stugo.Config.Registry
{
    public class RegistrySettings
    {
        private readonly string baseKeyPath;
        private readonly RegistryHive hive;
        private readonly RegistryView view;


        public RegistrySettings(RegistryHive hive, RegistryView view, string baseKeyPath = "")
        {
            this.baseKeyPath = baseKeyPath;
            this.hive = hive;
            this.view = view;
        }


        public RegistrySettings(string baseKeyPath = "", bool perUser = false, bool force32bit = false)
            : this(
                perUser ? RegistryHive.CurrentUser : RegistryHive.LocalMachine,
                force32bit ? RegistryView.Registry32 : RegistryView.Default,
                baseKeyPath)
        {
        }


        public RegistrySettings OpenKey(string keyName)
        {
            return new RegistrySettings(hive, view, GetPath(keyName));
        }


        public bool HasKey(string path)
        {
            using (var baseKey = RegistryKey.OpenBaseKey(hive, view))
            using (var subKey = baseKey.OpenSubKey(GetPath(path)))
            {
                return subKey != null;
            }
        }


        public IEnumerable<string> GetSubKeyNames(string subKeyPath = null)
        {
            var path = GetPath(subKeyPath);

            using (var baseKey = RegistryKey.OpenBaseKey(hive, view))
            using (var subKey = baseKey.OpenSubKey(path))
            {
                if (subKey == null)
                    return new string[] { };
                else
                    return subKey.GetSubKeyNames();
            }
        }


        public IEnumerable<string> GetValueNames(string subKeyPath = null)
        {
            var path = GetPath(subKeyPath);

            using (var baseKey = RegistryKey.OpenBaseKey(hive, view))
            using (var subKey = baseKey.OpenSubKey(path))
            {
                if (subKey == null)
                    return new string[] { };
                else
                    return subKey.GetValueNames();
            }
        }


        public TValue GetValue<TValue>(string name, string subKeyPath = null, TValue defaultValue = default(TValue))
        {
            var path = GetPath(subKeyPath);

            using (var baseKey = RegistryKey.OpenBaseKey(hive, view))
            using (var subKey = baseKey.OpenSubKey(path))
            {
                object value = null;

                if (subKey != null)
                    value = subKey.GetValue(name);

                if (value == null)
                    return defaultValue;
                else
                    return (TValue)value;
            }
        }


        public void SetValue<TValue>(string name, TValue value, string subKeyPath = null)
        {
            var path = GetPath(subKeyPath);

            using (var baseKey = RegistryKey.OpenBaseKey(hive, view))
            using (var subKey = baseKey.CreateSubKey(path))
            {
                subKey.SetValue(name, value);
            }
        }


        private string GetPath(string subKeyPath)
        {
            if (string.IsNullOrEmpty(subKeyPath))
                return baseKeyPath;
            else
                return Path.Combine(baseKeyPath, subKeyPath);
        }
    }
}
