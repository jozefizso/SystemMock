using System;
using System.Collections.Generic;
using System.Globalization;
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

        [Test]
        public void GetValue_NonExistingValueName_RetursNull()
        {
            // Arrange & Act
            var actualValue = this.registryKey.GetValue("NonExistingValueName");

            // Assert
            Assert.IsNull(actualValue);
        }

        [Test]
        public void ValueCount_RegistryKeyHasSomeValuesSet_RetursNumberOfValuesInRegistryKey([Range(1,5)] int valuesCount)
        {
            // Arrange
            var expectedValue = valuesCount;

            for (int i = 0; i < valuesCount; i++)
            {
                this.registryKey.SetValue("Value"+ i, i.ToString(CultureInfo.InvariantCulture));
            }

            // Act
            var actualValue = this.registryKey.ValueCount;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void SubKeyCount_RegistryKeyHasSomeSubKies_RetursNumberOfSubKiesInRegistryKey([Range(1,5)] int keysCount)
        {
            // Arrange
            var expectedValue = keysCount;

            for (int i = 0; i < keysCount; i++)
            {
                this.registryKey.CreateSubKey("Key"+ i);
            }

            // Act
            var actualValue = this.registryKey.SubKeyCount;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void GetValueNames_KeyWithOrderedValues_ReturnsArrayOfValueNames()
        {
            // Arrange
            var expectedValues = new string[] { "", "Value1", "Value2", "Value3" };

            foreach (var valueName in expectedValues)
            {
                this.registryKey.SetValue(valueName, "");
            }

            // Act
            var actualValueNames = this.registryKey.GetValueNames();

            // Assert
            CollectionAssert.AreEqual(expectedValues, actualValueNames);
        }

        [Test]
        public void GetValueNames_KeyWithUnorderedValues_ReturnsArrayOfValueNames()
        {
            // Arrange
            var expectedValues = new string[] { "", "1", "10", "2", "Abc", "t", "TEST", "Test1", "Test2", "Testík" };

            this.registryKey.SetValue("t", "");
            this.registryKey.SetValue("Testík", "");
            this.registryKey.SetValue("Abc", "");
            this.registryKey.SetValue("", "");
            this.registryKey.SetValue("10", "");
            this.registryKey.SetValue("2", "");
            this.registryKey.SetValue("1", "");
            this.registryKey.SetValue("Test1", "");
            this.registryKey.SetValue("TEST", "");
            this.registryKey.SetValue("Test2", "");

            // Act
            var actualValueNames = this.registryKey.GetValueNames();

            // Assert
            CollectionAssert.AreEqual(expectedValues, actualValueNames);
        }

        [Test]
        public void GetSubKeyNames_UnorderedSubKeys_ReturnsArrayOfSubKeyNamesInOrder()
        {
            // Arrange
            var expectedValues = new string[] { "", "1", "10", "2", "Abc", "t", "TEST", "Test1", "Test2", "Testík" };

            this.registryKey.CreateSubKey("t");
            this.registryKey.CreateSubKey("Testík");
            this.registryKey.CreateSubKey("Abc");
            this.registryKey.CreateSubKey("");
            this.registryKey.CreateSubKey("10");
            this.registryKey.CreateSubKey("2");
            this.registryKey.CreateSubKey("1");
            this.registryKey.CreateSubKey("Test1");
            this.registryKey.CreateSubKey("TEST");
            this.registryKey.CreateSubKey("Test2");

            // Act
            var actualValueNames = this.registryKey.GetSubKeyNames();

            // Assert
            CollectionAssert.AreEqual(expectedValues, actualValueNames);
        }

        [Test]
        public void Dispose_CallingDisposeMethod_DoesNothing()
        {
            // Arrange
            IDisposable disposable = this.registryKey;

            // Act
            disposable.Dispose();

            // Assert
            Assert.Pass();
        }

        [Test]
        public void Close_CallingCloseMethod_DoesNothing()
        {
            // Arrange & Act
            this.registryKey.Close();

            // Assert
            Assert.Pass();
        }
    }
}