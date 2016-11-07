using Microsoft.Win32;
using Stugo.Config.Registry;
using System.IO;
using Xunit;

namespace Stugo.Config.Test.Registry
{
    public class RegistrySettingsTest
    {
        private const RegistryHive TestHive = RegistryHive.CurrentUser;
        private const RegistryView TestView = RegistryView.Default;
        private const string TestKeyPath = @"Software\Stugo\UnitTest\Stugo.Castor";


        [Fact]
        public void It_can_list_child_keys()
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
                }
            }

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            var keys = registrySettings.GetSubKeyNames();

            Assert.Equal(new[] { "Key1", "Key2", "Key3" }, keys);
        }


        [Fact]
        public void It_can_list_child_values()
        {
            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                // cleanup
                baseKey.DeleteSubKeyTree(TestKeyPath, false);

                // create source data
                using (var subKey = baseKey.CreateSubKey(TestKeyPath))
                {
                    subKey.SetValue("Value1", 1);
                    subKey.SetValue("Value2", 2);
                    subKey.SetValue("Value3", 3);
                }
            }

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            var values = registrySettings.GetValueNames();

            Assert.Equal(new[] { "Value1", "Value2", "Value3" }, values);
        }


        [Fact]
        public void It_can_list_child_keys_in_subkey()
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
                }
            }

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            var keys = registrySettings.GetSubKeyNames(subKeyName);

            Assert.Equal(new[] { "Key1", "Key2", "Key3" }, keys);
        }


        [Fact]
        public void It_can_list_child_values_in_subkey()
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

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            var values = registrySettings.GetValueNames(subKeyName);

            Assert.Equal(new[] { "Value1", "Value2", "Value3" }, values);
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

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            var value = registrySettings.GetValue<int>("Value1");

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

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);

            var value = registrySettings.GetValue<int>(
                "Value1", subKeyPath: subKeyName);

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

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            var value = registrySettings.GetValue<int>(
                "Value4", defaultValue: 400);

            Assert.Equal(400, value);
        }


        [Fact]
        public void It_can_open_a_subkey()
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

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            var subSettings = registrySettings.OpenKey(subKeyName);
            var value = subSettings.GetValue<int>("Value1");

            Assert.Equal(1, value);
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

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            registrySettings.SetValue("Value1", 1);

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

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            registrySettings.SetValue("Value1", 1, subKeyPath: subKeyName);

            using (var baseKey = RegistryKey.OpenBaseKey(TestHive, TestView))
            {
                using (var subKey = baseKey.OpenSubKey(path))
                {
                    Assert.Equal(1, subKey.GetValue("Value1"));
                }
            }
        }


        [Fact]
        public void It_can_confirm_existence_of_key()
        {
            var path = TestKeyPath;

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
                }
            }

            var registrySettings = new RegistrySettings(TestKeyPath, perUser: true);
            var hasKey = registrySettings.HasKey("Key1");

            Assert.True(hasKey);
        }
    }
}
