using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using NUnit.Framework;

namespace SystemMock.Tests
{
    [TestFixture]
    public class RegistryKeyMockTests
    {
        private RegistryKeyMock registryKey;

        [SetUp]
        public void SetUp()
        {
            this.registryKey = new RegistryKeyMock("RegKey");
        }


        [Test]
        public void Constructor_KeyNameParameter_InitializesRegistryKeyProperly()
        {
            // Arrange
            var expectedName = "MyKeyName";

            // Act
            var registryKey = new RegistryKeyMock(expectedName);
            var actualKeyName = registryKey.Name;

            // Assert
            Assert.AreEqual(expectedName, actualKeyName);
            Assert.AreEqual(0, registryKey.SubKeyCount);
            Assert.AreEqual(0, registryKey.ValueCount);
            Assert.AreEqual(RegistryView.Default, registryKey.View);
        }

        [Test]
        public void SetValue_NullValueNameParameter_CreatesDefaultValueInRegistry()
        {
            // Arrange
            var expectedValue = "value 123";
            this.registryKey.SetValue(null, expectedValue);

            // Act
            var actualValue = this.registryKey.GetValue(null);

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void SetValue_StringEmptyValueNameParameter_CreatesDefaultValueInRegistry()
        {
            // Arrange
            var expectedValue = "value 234";
            this.registryKey.SetValue("", expectedValue);

            // Act
            var actualValue = this.registryKey.GetValue("");

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void SetValue_NullValueNameParameter_CanBeAccessAsEmptyValueName()
        {
            // Arrange
            var expectedValue = "value 345";
            this.registryKey.SetValue(null, expectedValue);

            // Act
            var actualValue = this.registryKey.GetValue("");

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void SetValue_ValueNameParameter_CreatesValueInRegistry()
        {
            // Arrange
            var expectedValue = "value 456";
            this.registryKey.SetValue("MyValueName", expectedValue);

            // Act
            var actualValue = this.registryKey.GetValue("MyValueName");

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}