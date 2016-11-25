using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace Stugo.Config.Registry
{
    public class RegistryConfigProvider : IConfigProvider
    {
        public RegistryConfigProvider(RegistryHive hive, RegistryView view, string basePath = "/")
            : this(CreateUri(hive, view, basePath))
        {
        }


        public RegistryConfigProvider(Uri baseUri)
        {
            if (!baseUri.AbsolutePath.EndsWith("/"))
                throw new ArgumentException("The base URI must point to a key");

            BaseUri = baseUri;
        }


        public Uri BaseUri { get; }


        public Uri[] GetChildren(Uri path)
        {
            path = new Uri(BaseUri, path);

            if (!path.AbsolutePath.EndsWith("/"))
                throw new ArgumentException("The path must point to a key");

            using (var baseKey = OpenBaseKey(path))
            using (var subKey = baseKey.OpenSubKey(GetKeyName(path)))
            {
                return subKey.GetSubKeyNames().Select(x => x + "/")
                    .Union(subKey.GetValueNames())
                    .Select(x => new Uri(path, x))
                    .ToArray();
            }
        }


        public TValue GetValue<TValue>(Uri path, TValue defaultValue = default(TValue))
        {
            path = new Uri(BaseUri, path);
            var name = Path.GetFileName(path.AbsolutePath);

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The path must point to a value");

            using (var baseKey = OpenBaseKey(path))
            using (var subKey = baseKey.OpenSubKey(GetKeyName(path)))
            {
                return (TValue)(subKey?.GetValue(name) ?? defaultValue);
            }
        }


        public void SetValue<TValue>(Uri path, TValue value)
        {
            path = new Uri(BaseUri, path);
            var name = Path.GetFileName(path.AbsolutePath);

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The path must point to a value");

            using (var baseKey = OpenBaseKey(path))
            using (var subKey = baseKey.CreateSubKey(GetKeyName(path)))
            {
                subKey.SetValue(name, value);
            }
        }


        private RegistryKey OpenBaseKey(Uri path)
        {
            var hive = RegistryHiveConverter.FromString(path.Authority);
            var view = RegistryViewConverter.FromString(path.Scheme);
            return RegistryKey.OpenBaseKey(hive, view);
        }


        private static string GetKeyName(Uri path) => Path.GetDirectoryName(path.AbsolutePath).TrimStart('\\');


        private static Uri CreateUri(RegistryHive hive, RegistryView view, string basePath)
        {
            return new Uri(
                new Uri($"{RegistryViewConverter.ToString(view)}://{RegistryHiveConverter.ToString(hive)}/"),
                basePath
            );
        }
    }
}
