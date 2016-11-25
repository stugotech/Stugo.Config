using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Stugo.Config.Registry;
using Xunit;

namespace Stugo.Config.Test.Registry
{
    public class RegistryConfigProviderTest
    {
        private const RegistryHive TestHive = RegistryHive.CurrentUser;
        private const RegistryView TestView = RegistryView.Default;
        private const string TestKeyPath = @"Software\Stugo\UnitTest\Stugo.Config";
        private const string TestKeyUri = "registry://HKEY_CURRENT_USER/Software/Stugo/UnitTest/Stugo.Config/";


        [Fact]
        public void It_can_list_child_items()
        {
            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                // cleanup
                baseKey.DeleteSubKeyTree(TestKeyPath, false);

                // create source data
                using (var subKey = baseKey.CreateSubKey(TestKeyPath))
                {
                    subKey.CreateSubKey("Key1").Dispose();
                    subKey.CreateSubKey("Key2").Dispose();
                    subKey.CreateSubKey("Key3").Dispose();
                    subKey.SetValue("Value1", 1);
                    subKey.SetValue("Value2", 2);
                    subKey.SetValue("Value3", 3);
                }
            }

            var rootUri = new Uri(TestKeyUri);
            var config = new RegistryConfigProvider(rootUri);
            var keys = config.GetChildren().Select(x => rootUri.MakeRelativeUri(x).ToString());

            Assert.Equal(new[] { "Key1/", "Key2/", "Key3/", "Value1", "Value2", "Value3" }, keys);
        }


        [Fact]
        public void It_can_list_child_items_in_subkey()
        {
            const string subKeyName = "subkey";
            var path = Path.Combine(TestKeyPath, subKeyName);

            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                // cleanup
                baseKey.DeleteSubKeyTree(path, false);

                // create source data
                using (var subKey = baseKey.CreateSubKey(path))
                {
                    subKey.CreateSubKey("Key1").Dispose();
                    subKey.CreateSubKey("Key2").Dispose();
                    subKey.CreateSubKey("Key3").Dispose();
                    subKey.SetValue("Value1", 1);
                    subKey.SetValue("Value2", 2);
                    subKey.SetValue("Value3", 3);
                }
            }

            var rootUri = new Uri(TestKeyUri);
            var subkeyUri = new Uri(rootUri, subKeyName + "/");
            var config = new RegistryConfigProvider(rootUri);
            var keys = config.GetChildren(subKeyName+"/").Select(x => subkeyUri.MakeRelativeUri(x).ToString());

            Assert.Equal(new[] { "Key1/", "Key2/", "Key3/", "Value1", "Value2", "Value3" }, keys);
        }


        [Fact]
        public void It_can_get_a_value()
        {
            var path = TestKeyPath;

            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                // cleanup
                baseKey.DeleteSubKeyTree(path, false);

                // create source data
                using (var subKey = baseKey.CreateSubKey(path))
                {
                    subKey.SetValue("Value1", 1);
                    subKey.SetValue("Value2", 2);
                    subKey.SetValue("Value3", 3);
                }
            }

            var rootUri = new Uri(TestKeyUri);
            var config = new RegistryConfigProvider(rootUri);
            var value = config.GetValue<int>("./Value1");

            Assert.Equal(1, value);
        }


        [Fact]
        public void It_can_get_a_value_in_a_subkey()
        {
            const string subKeyName = "subkey";
            var path = Path.Combine(TestKeyPath, subKeyName);

            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                // cleanup
                baseKey.DeleteSubKeyTree(path, false);

                // create source data
                using (var subKey = baseKey.CreateSubKey(path))
                {
                    subKey.SetValue("Value1", 1);
                    subKey.SetValue("Value2", 2);
                    subKey.SetValue("Value3", 3);
                }
            }

            var rootUri = new Uri(TestKeyUri);
            var config = new RegistryConfigProvider(rootUri);
            var value = config.GetValue<int>($"{subKeyName}/Value1");

            Assert.Equal(1, value);
        }


        [Fact]
        public void It_can_get_return_a_default_value_if_none_exists()
        {
            var path = TestKeyPath;

            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                // cleanup
                baseKey.DeleteSubKeyTree(path, false);

                // create source data
                using (var subKey = baseKey.CreateSubKey(path))
                {
                    subKey.SetValue("Value1", 1);
                    subKey.SetValue("Value2", 2);
                    subKey.SetValue("Value3", 3);
                }
            }

            var rootUri = new Uri(TestKeyUri);
            var config = new RegistryConfigProvider(rootUri);
            var value = config.GetValue($"Value4", 400);

            Assert.Equal(400, value);
        }


        [Fact]
        public void It_can_set_a_value()
        {
            var path = TestKeyPath;

            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                // cleanup
                baseKey.DeleteSubKeyTree(path, false);
            }

            var rootUri = new Uri(TestKeyUri);
            var config = new RegistryConfigProvider(rootUri);
            config.SetValue("Value1", 1);

            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                using (var subKey = baseKey.OpenSubKey(path))
                {
                    Assert.Equal(1, subKey.GetValue("Value1"));
                }
            }
        }


        [Fact]
        public void It_can_set_a_value_in_a_subkey()
        {
            const string subKeyName = "subkey";
            var path = Path.Combine(TestKeyPath, subKeyName);

            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                // cleanup
                baseKey.DeleteSubKeyTree(path, false);
            }

            var rootUri = new Uri(TestKeyUri);
            var config = new RegistryConfigProvider(rootUri);
            config.SetValue($"{subKeyName}/Value1", 1);

            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                using (var subKey = baseKey.OpenSubKey(path))
                {
                    Assert.Equal(1, subKey.GetValue("Value1"));
                }
            }
        }
    }
}
