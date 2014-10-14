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

        [Test]
        public void Import_SingleRegistryKeyWithSeveralValuesWithRelaxedSyntax_CreatesRegistryKey()
        {
            // Arrange

            // Act
            this.registry.Import(@"[HKEY_CURRENT_USER\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"Version=1.2.0" + Environment.NewLine +
                                 @"Uninstall=0" + Environment.NewLine +
                                 @"Guid={A88848E9-7FB0-4591-A957-A1B9D2C3B3EF}" + Environment.NewLine);

            var actualKey = this.registry.CurrentUser.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.IsNotNull(actualKey, @"Registry key 'HKEY_CURRENT_USER\Software\SystemMock\UnitTests' must exists.");
            Assert.AreEqual("1.2.0", actualKey.GetValue("Version"));
            Assert.AreEqual("0", actualKey.GetValue("Uninstall"));
            Assert.AreEqual("{A88848E9-7FB0-4591-A957-A1B9D2C3B3EF}", actualKey.GetValue("Guid"));
        }

        [Test]
        public void Import_ValueWithEscapedPathCharacters_CreatesRegistryKey()
        {
            // Arrange

            // Act
            this.registry.Import(@"[HKEY_LOCAL_MACHINE\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"""SourceDir""=""C:\\Users\\Jozef\\Downloads\\""" + Environment.NewLine);

            var actualKey = this.registry.LocalMachine.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.IsNotNull(actualKey, @"Registry key 'HKEY_LOCAL_MACHINE\Software\SystemMock\UnitTests' must exists.");
            Assert.AreEqual(@"C:\Users\Jozef\Downloads\", actualKey.GetValue("SourceDir"));
        }

        [Test]
        public void Import_ValueWithEscapedHexBytes_CreatesValueWithBytes()
        {
            // Arrange
            var expectedBytes = new byte[] { 0xD0, 0x8c, 0x9D };

            // Act
            this.registry.Import(@"[HKEY_LOCAL_MACHINE\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"""Token""=hex:d0,8c,9d" + Environment.NewLine);

            var actualKey = this.registry.LocalMachine.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.IsNotNull(actualKey, @"Registry key 'HKEY_LOCAL_MACHINE\Software\SystemMock\UnitTests' must exists.");
            Assert.AreEqual(expectedBytes, actualKey.GetValue("Token"));
        }

        [Test]
        public void Import_TwoRegistryKeys_CreatesBothKeys()
        {
            // Arrange

            // Act
            this.registry.Import(@"[HKEY_LOCAL_MACHINE\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"@=""Machine Key""" + Environment.NewLine +
                                 @"" + Environment.NewLine +
                                 @"[HKEY_CURRENT_USER\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"@=""User Key""" + Environment.NewLine);

            var actualMachineKey = this.registry.LocalMachine.OpenSubKey(@"Software\SystemMock\UnitTests");
            var actualUserKey = this.registry.CurrentUser.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.IsNotNull(actualMachineKey, @"Registry key 'HKEY_LOCAL_MACHINE\Software\SystemMock\UnitTests' must exists.");
            Assert.IsNotNull(actualUserKey, @"Registry key 'HKEY_CURRENT_USER\Software\SystemMock\UnitTests' must exists.");
            Assert.AreEqual("Machine Key", actualMachineKey.GetValue(null));
            Assert.AreEqual("User Key", actualUserKey.GetValue(null));
        }

        [Test]
        public void Import_Comment_IsIgnored()
        {
            // Arrange

            // Act
            this.registry.Import(@";[HKEY_LOCAL_MACHINE\Software\SystemMock\UnitTests]");

            var actualMachineKey = this.registry.LocalMachine;

            // Assert
            Assert.AreEqual(0, actualMachineKey.SubKeyCount);
        }

        [Test]
        public void Import_Comment2_IsIgnored()
        {
            // Arrange

            // Act
            this.registry.Import(@"[HKEY_LOCAL_MACHINE\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"Key1=""Machine Key""" + Environment.NewLine +
                                 @"" + Environment.NewLine +
                                 @";[HKEY_CURRENT_USER\Software\SystemMock\UnitTests]" + Environment.NewLine +
                                 @"Key2=""User Key""" + Environment.NewLine);

            var actualMachineKey = this.registry.LocalMachine.OpenSubKey(@"Software\SystemMock\UnitTests");

            // Assert
            Assert.AreEqual(2, actualMachineKey.ValueCount);
        }
    }
}
