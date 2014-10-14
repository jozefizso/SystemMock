using System;
using NUnit.Framework;

namespace SystemMock.Tests
{
    [TestFixture]
    public class RegistryMock_Import_Tests
    {
        private RegistryMock registry;

        [SetUp]
        public void SetUp()
        {
            this.registry = new RegistryMock();
        }

        [Test]
        public void Import_SingleRegistryKey_CreatesRegistryKey()
        {
            // Arrange

            // Act
            this.registry.Import(@"[HKEY_CURRENT_USER\Software\SystemMock\UnitTests]");

            var actualKey = this.registry.CurrentUser.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.IsNotNull(actualKey, @"Registry key 'HKEY_CURRENT_USER\Software\SystemMock\UnitTests' must exists.");
        }

        [Test]
        public void Import_SingleRegistryKeyWithValue_CreatesRegistryKey()
        {
            // Arrange

            // Act
            this.registry.Import(@"[HKEY_CURRENT_USER\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"""Version""=""1""");

            var actualKey = this.registry.CurrentUser.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.IsNotNull(actualKey, @"Registry key 'HKEY_CURRENT_USER\Software\SystemMock\UnitTests' must exists.");

            var actualVersionValue = actualKey.GetValue("Version");
            Assert.AreEqual("1", actualVersionValue);
        }

        [Test]
        public void Import_SingleRegistryKeyWithSeveralValues_CreatesRegistryKey()
        {
            // Arrange

            // Act
            this.registry.Import(@"[HKEY_CURRENT_USER\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"""Version""=""1.2.0""" + Environment.NewLine +
                                 @"""Uninstall""=""0""" + Environment.NewLine +
                                 @"""Guid""=""{A88848E9-7FB0-4591-A957-A1B9D2C3B3EF}""" + Environment.NewLine);

            var actualKey = this.registry.CurrentUser.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.IsNotNull(actualKey, @"Registry key 'HKEY_CURRENT_USER\Software\SystemMock\UnitTests' must exists.");
            Assert.AreEqual("1.2.0", actualKey.GetValue("Version"));
            Assert.AreEqual("0", actualKey.GetValue("Uninstall"));
            Assert.AreEqual("{A88848E9-7FB0-4591-A957-A1B9D2C3B3EF}", actualKey.GetValue("Guid"));
        }

        [Test]
        public void Import_SingleRegistryKeyWithDefaultValue_CreatesRegistryKey()
        {
            // Arrange

            // Act
            this.registry.Import(@"[HKEY_CURRENT_USER\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"@=""Default Value""" + Environment.NewLine);

            var actualKey = this.registry.CurrentUser.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.IsNotNull(actualKey, @"Registry key 'HKEY_CURRENT_USER\Software\SystemMock\UnitTests' must exists.");
            Assert.AreEqual("Default Value", actualKey.GetValue(null));
        }

        [Test]
        public void Import_SingleRegistryKeyWithDefaultValueAndAtCharacter_CreatesRegistryKey()
        {
            // Arrange

            // Act
            this.registry.Import(@"[HKEY_CURRENT_USER\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"@=""Default Value""" + Environment.NewLine +
                                 @"""@""=""At Value""" + Environment.NewLine);

            var actualKey = this.registry.CurrentUser.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.IsNotNull(actualKey, @"Registry key 'HKEY_CURRENT_USER\Software\SystemMock\UnitTests' must exists.");
            Assert.AreEqual("Default Value", actualKey.GetValue(null));
            Assert.AreEqual("At Value", actualKey.GetValue("@"));
        }
    }
}
