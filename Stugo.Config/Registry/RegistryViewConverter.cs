using System;
using Microsoft.Win32;

namespace Stugo.Config.Registry
{
    public static class RegistryViewConverter
    {
        public static string ToString(RegistryView view)
        {
            switch (view)
            {
                case RegistryView.Default:
                    return "registry";
                case RegistryView.Registry32:
                    return "registry32";
                case RegistryView.Registry64:
                    return "registry64";
                default:
                    throw new ArgumentException($"Unknown registry view {view}");
            }
        }


        public static RegistryView FromString(string view)
        {
            switch (view)
            {
                case "registry":
                    return RegistryView.Default;
                case "registry32":
                    return RegistryView.Registry32;
                case "registry64":
                    return RegistryView.Registry64;
                default:
                    throw new ArgumentException($"Unknown registry view {view}");
            }
        }
    }
}
