using System;
using SystemInterface.Microsoft.Win32;
using NUnit.Framework;

namespace SystemMock.Tests
{
    [TestFixture]
    public class RegistryMockTests
    {
        private IRegistry registry;

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
    }
}
