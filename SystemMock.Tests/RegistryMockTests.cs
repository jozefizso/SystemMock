using System;
using SystemInterface.Microsoft.Win32;
using Microsoft.Win32;
using NUnit.Framework;

namespace SystemMock.Tests
{
    [TestFixture]
    public class RegistryMockTests
    {
        private RegistryMock registry;

        [SetUp]
        public void SetUp()
        {
            this.registry = new RegistryMock();
        }

        [Test]
        public void ClassesRoot_PropertyGetter_ReturnsCorrectRegistryKeyMock()
        {
            // Arrange & Act
            var actualKey = this.registry.ClassesRoot;

            // Assert
            Assert.IsNotNull(actualKey);
            Assert.AreEqual("HKEY_CLASSES_ROOT", actualKey.Name);
        }

        [Test]
        public void CurrentUser_PropertyGetter_ReturnsCorrectRegistryKeyMock()
        {
            // Arrange & Act
            var actualKey = this.registry.CurrentUser;

            // Assert
            Assert.IsNotNull(actualKey);
            Assert.AreEqual("HKEY_CURRENT_USER", actualKey.Name);
        }

        [Test]
        public void LocalMachine_PropertyGetter_ReturnsCorrectRegistryKeyMock()
        {
            // Arrange & Act
            var actualKey = this.registry.LocalMachine;

            // Assert
            Assert.IsNotNull(actualKey);
            Assert.AreEqual("HKEY_LOCAL_MACHINE", actualKey.Name);
        }

        [Test]
        public void CurrentConfig_PropertyGetter_ReturnsCorrectRegistryKeyMock()
        {
            // Arrange & Act
            var actualKey = this.registry.CurrentConfig;

            // Assert
            Assert.IsNotNull(actualKey);
            Assert.AreEqual("HKEY_CURRENT_CONFIG", actualKey.Name);
        }

        [Test]
        public void PerformanceData_PropertyGetter_ReturnsCorrectRegistryKeyMock()
        {
            // Arrange & Act
            var actualKey = this.registry.PerformanceData;

            // Assert
            Assert.IsNotNull(actualKey);
            Assert.AreEqual("HKEY_PERFORMANCE_DATA", actualKey.Name);
        }

        [Test]
        public void Users_PropertyGetter_ReturnsCorrectRegistryKeyMock()
        {
            // Arrange & Act
            var actualKey = this.registry.Users;

            // Assert
            Assert.IsNotNull(actualKey);
            Assert.AreEqual("HKEY_USERS", actualKey.Name);
        }

        [Test]
        public void SetValue_RootKeyName_SetsValueOfRegistryKey()
        {
            // Arrange
            var expectedValue = "Jozef";

            // Act
            this.registry.SetValue(@"HKEY_CURRENT_USER", "UserName", expectedValue);

            var actualRegistryKey = this.registry.CurrentUser;
            var actualUserNameValue = actualRegistryKey.GetValue("UserName");

            // Assert
            Assert.AreEqual(expectedValue, actualUserNameValue);
        }

        [Test]
        public void SetValue_RootKeyWithOneSubkeyName_SetsValueOfRegistryKeySubkey()
        {
            // Arrange
            var expectedValue = "SystemMock.Tests";

            // Act
            this.registry.SetValue(@"HKEY_LOCAL_MACHINE\System", "Name", expectedValue);

            var actualRegistryKey = this.registry.LocalMachine;
            var actualSubkey = actualRegistryKey.OpenSubKey("System");

            // Assert
            Assert.IsNotNull(actualSubkey, "Registry root key HKEY_LOCAL_MACHINE should have subkey named 'System'.");

            var actualNameValue = actualSubkey.GetValue("Name");

            Assert.AreEqual(expectedValue, actualNameValue);
        }
        [Test]
        public void SetValue_RootKeyWithDeepSubkeyHierarchy_CreatesCorrectKeysAndSetsValue()
        {
            // Arrange
            var expectedValue = "1.0.0";

            // Act
            this.registry.SetValue(@"HKEY_CURRENT_USER\Software\Contoso\BizApp", "Version", expectedValue);

            var actualRegistryKey = this.registry.CurrentUser;
            var actualSubkey = actualRegistryKey.OpenSubKey(@"Software\Contoso\BizApp");

            // Assert
            Assert.IsNotNull(actualSubkey, @"Registry root key HKEY_CURRENT_USER should have subkey named 'Software\Contoso\BizApp'.");

            var actualNameValue = actualSubkey.GetValue("Version");
            Assert.AreEqual("BizApp", actualSubkey.Name);
            Assert.AreEqual(expectedValue, actualNameValue);
        }
    }
}
