using System;

namespace Stugo.Config
{
    public static class ConfigProviderExtensions
    {
        public static Uri[] GetChildren(this IConfigProvider config, string path = ".")
        {
            return config.GetChildren(new Uri(path, UriKind.RelativeOrAbsolute));
        }


        public static TValue GetValue<TValue>(this IConfigProvider config, string path,
            TValue defaultValue = default(TValue))
        {
            return config.GetValue(new Uri(path, UriKind.RelativeOrAbsolute), defaultValue);
        }


        public static void SetValue<TValue>(this IConfigProvider config, string path, TValue value)
        {
            config.SetValue(new Uri(path, UriKind.RelativeOrAbsolute), value);
        }
    }
}
