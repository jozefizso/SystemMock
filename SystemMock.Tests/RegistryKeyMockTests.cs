using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SystemInterface.Microsoft.Win32;
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
        public void SetValue_RegistryValueKindParameter_CreatesValueInRegistryWithSpecifiedKind()
        {
            // Arrange
            var expectedValueKind = RegistryValueKind.DWord;

            // Act
            this.registryKey.SetValue("MyValueName", 1, expectedValueKind);
            var actualValueKind = this.registryKey.GetValueKind("MyValueName");

            // Assert
            Assert.AreEqual(expectedValueKind, actualValueKind);
        }

        [Test]
        public void GetValueKind_NonExistingValueName_ThrowsIOException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<IOException>(() => this.registryKey.GetValueKind("Non-existing Value Name"));
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
        [TestCase("SubKey")]
        [TestCase("SubKey 1")]
        [TestCase("SubKey #1")]
        [TestCase(" _ New Key #1")]
        public void CreateSubKey_SimpleSubKeyName_CreatesImmediateSubKey(string expectedSubKeyName)
        {
            // Arrange

            // Act
            var actualSubKey = this.registryKey.CreateSubKey(expectedSubKeyName);

            // Assert
            Assert.IsNotNull(actualSubKey);
            Assert.AreSame(expectedSubKeyName, actualSubKey.Name);
        }

        [Test]
        public void CreateSubKey_ComplexSubKeyName_CreatesSubKeys()
        {
            // Arrange
            var actualCreatedSubKey = this.registryKey.CreateSubKey(@"MyKey\SubKey");

            // Act
            var actualMyKey = this.registryKey.OpenSubKey("MyKey");

            // Assert
            Assert.IsNotNull(actualCreatedSubKey, "RegistryKeyMock should create key 'MyKey'.");
            Assert.AreEqual("SubKey", actualCreatedSubKey.Name);

            Assert.IsNotNull(actualMyKey, "RegistryKeyMock should open key 'MyKey'.");

            var actualSubSubKey = actualMyKey.OpenSubKey("SubKey");
            Assert.IsNotNull(actualSubSubKey);
            Assert.AreSame(actualCreatedSubKey, actualSubSubKey);
            Assert.AreEqual("SubKey", actualSubSubKey.Name);
        }

        [Test]
        [TestCase(@"Key\SubKey", new string[] { "Key", "SubKey" })]
        [TestCase(@"Key\SubKey\MySubKey", new string[] { "Key", "SubKey", "MySubKey" })]
        public void CreateSubKey_ComplexSubKeyName_CreatesAllSubKeys(string complexSubKeyName, string[] expectedSubKeyNames)
        {
            // Arrange

            // Act
            this.registryKey.CreateSubKey(complexSubKeyName);

            // Assert
            IRegistryKey key = this.registryKey;
            for(int i = 0; i < expectedSubKeyNames.Length; i++)
            {
                var expectedSubKeyName = expectedSubKeyNames[i];
                var actualSubKey = key.OpenSubKey(expectedSubKeyName);
                Assert.IsNotNull(key, "Registry key with name '{0}' should exist.", expectedSubKeyName);
                Assert.AreEqual(expectedSubKeyName, actualSubKey.Name);

                key = actualSubKey;
            }
        }

        [Test]
        public void CreateSubKey_SimpleSubKeyNameCreatedTwice_DoesNotOverwriteExistingKey()
        {
            // Arrange
            var key = this.registryKey.CreateSubKey("MyKey");
            key.SetValue("MyValue", "1");

            // Act
            var actualKey = this.registryKey.CreateSubKey("MyKey");

            // Assert
            Assert.AreSame(key, actualKey, "RegistryKeyMock should return existing subkey.");
        }

        [Test]
        [TestCase("SubKey")]
        [TestCase("SubKey 1")]
        [TestCase("SubKey #1")]
        [TestCase(" _ New Key #1")]
        public void OpenSubKey_SimpleSubKeyName_CreatesImmediateSubKey(string expectedSubKeyName)
        {
            // Arrange
            this.registryKey.CreateSubKey(expectedSubKeyName);

            // Act
            var actualSubKey = this.registryKey.OpenSubKey(expectedSubKeyName);

            // Assert
            Assert.IsNotNull(actualSubKey);
            Assert.AreSame(expectedSubKeyName, actualSubKey.Name);
        }

        [Test]
        public void OpenSubKey_NonExistingSubKeyName_ReturnsNull()
        {
            // Arrange & Act
            var actualSubKey = this.registryKey.OpenSubKey("Non-existing Sub Key");

            // Assert
            Assert.IsNull(actualSubKey);
        }

        [Test]
        public void OpenSubKey_ComplexSubKeyName_OpensCorrectSubKey()
        {
            // Arrange
            var key1 = this.registryKey.CreateSubKey(@"Key");
            var key2 = key1.CreateSubKey("SubKey");
            var key3 = key2.CreateSubKey("SubSubKey");

            // Act
            var actualKey = this.registryKey.OpenSubKey(@"Key\SubKey\SubSubKey");

            // Assert
            Assert.IsNotNull(actualKey, @"RegistryKeyMock should open subkey 'Key\SubKey\SubSubKey'.");
            Assert.AreSame("SubSubKey", actualKey.Name);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void OpenSubKey_OpenForWritingOverload_OpensCorrectSubKey(bool writable)
        {
            // Arrange
            const string expectedSubKeyName = "MyKey";
            this.registryKey.CreateSubKey(expectedSubKeyName);

            // Act
            var actualSubKey = this.registryKey.OpenSubKey(expectedSubKeyName, writable);

            // Assert
            Assert.IsNotNull(actualSubKey);
            Assert.AreSame(expectedSubKeyName, actualSubKey.Name);
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
        public void DeleteSubKey_RegistryKeyWithExistingSubKey_RemovesTheSubKey()
        {
            // Arrange
            this.registryKey.CreateSubKey("MyKey");

            // Act
            this.registryKey.DeleteSubKey("MyKey");

            var actualKey = this.registryKey.OpenSubKey("MyKey");

            // Assert
            Assert.IsNull(actualKey, "Registry subkey 'MyKey' should not exist.");
        }

        [Test]
        public void DeleteSubKey_SubKeyInOtherCaseSensitivity_RemovesTheSubKey()
        {
            // Arrange
            this.registryKey.CreateSubKey("MyKey");

            // Act
            this.registryKey.DeleteSubKey("MYKEY");

            var actualKey = this.registryKey.OpenSubKey("MyKey");

            // Assert
            Assert.IsNull(actualKey, "Registry subkey 'MyKey' should not exist.");
        }

        [Test]
        public void DeleteSubKey_RegistryKeyHasSubKeys_ThrowsInvalidOperationExceptionOnDeletion()
        {
            // Arrange
            this.registryKey.CreateSubKey(@"MyKey\SubKey");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => this.registryKey.DeleteSubKey("MyKey"));

            var actualKey = this.registryKey.OpenSubKey("MyKey");
            Assert.IsNotNull(actualKey);
        }

        [Test]
        public void DeleteSubKey_NonExistingSubKeyName_ThrowsArgumentException()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentException>(() => this.registryKey.DeleteSubKey("Non-existing subkey"));
        }

        [Test]
        public void DeleteSubKey_ForceExceptionOnNonExistingSubKeyName_ThrowsArgumentException()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentException>(() => this.registryKey.DeleteSubKey("Non-existing subkey", true));
        }

        [Test]
        public void DeleteSubKeyTree_RegistryKeyWithoutSubKeys_RemovesTheSubKey()
        {
            // Arrange
            this.registryKey.CreateSubKey("MyKey");

            // Act
            this.registryKey.DeleteSubKeyTree("MyKey");

            var actualKey = this.registryKey.OpenSubKey("MyKey");

            // Assert
            Assert.IsNull(actualKey, "Registry subkey 'MyKey' should not exist.");
        }

        [Test]
        public void DeleteSubKeyTree_SubKeyInOtherCaseSensitivity_RemovesTheSubKey()
        {
            // Arrange
            this.registryKey.CreateSubKey("MyKey");

            // Act
            this.registryKey.DeleteSubKeyTree("MYKEY");

            var actualKey = this.registryKey.OpenSubKey("MyKey");

            // Assert
            Assert.IsNull(actualKey, "Registry subkey 'MyKey' should not exist.");
        }

        [Test]
        public void DeleteSubKeyTree_RegistryKeyHasSubKeys_DeletesTheTree()
        {
            // Arrange
            this.registryKey.CreateSubKey(@"MyKey\SubKey");

            // Act & Assert
            this.registryKey.DeleteSubKeyTree("MyKey");

            var actualKey = this.registryKey.OpenSubKey("MyKey");
            Assert.IsNull(actualKey);
        }

        [Test]
        public void DeleteSubKeyTree_NonExistingSubKeyName_ThrowsArgumentException()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentException>(() => this.registryKey.DeleteSubKeyTree("Non-existing subkey"));
        }

        [Test]
        public void DeleteSubKeyTree_ForceExceptionOnNonExistingSubKeyName_ThrowsArgumentException()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentException>(() => this.registryKey.DeleteSubKeyTree("Non-existing subkey", true));
        }

        [Test]
        public void DeleteSubKey_DoNotThrowExceptionOnNonExistingSubKeyName_DoesNothing()
        {
            // Arrange

            // Act
            this.registryKey.DeleteSubKey("Non-existing subkey", false);

            // Assert
            Assert.Pass();
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