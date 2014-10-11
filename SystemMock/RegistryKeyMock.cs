using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using SystemInterface.Microsoft.Win32;
using SystemInterface.Microsoft.Win32.SafeHandles;
using SystemInterface.Security.AccessControl;
using Microsoft.Win32;

namespace SystemMock
{
    public class RegistryKeyMock : IRegistryKey
    {
        private string name;
        private Dictionary<string, RegistryKeyMock> subkeys; 
        private Dictionary<string, Tuple<object, RegistryValueKind>> values;

        public RegistryKeyMock(string name)
        {
            this.name = name;
            this.values = new Dictionary<string, Tuple<object, RegistryValueKind>>();
            this.subkeys = new Dictionary<string, RegistryKeyMock>(StringComparer.OrdinalIgnoreCase);
        }

        public void Close()
        {
        }

        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions, IRegistrySecurity registrySecurity)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, IRegistrySecurity registrySecurity)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions options)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey CreateSubKey(string subkey)
        {
            var subKeySimpleName = subkey;
            string additionaSubKeysName = null;
            var separatorIndex = subKeySimpleName.IndexOf('\\');
            if (separatorIndex >= 0)
            {
                subKeySimpleName = subkey.Substring(0, separatorIndex);
                additionaSubKeysName = subkey.Substring(separatorIndex + 1);
            }

            RegistryKeyMock key;
            this.subkeys.TryGetValue(subKeySimpleName, out key);
            if (key == null)
            {
                key = new RegistryKeyMock(subKeySimpleName);
                this.subkeys.Add(subKeySimpleName, key);
            }

            if (additionaSubKeysName != null)
            {
                return key.CreateSubKey(additionaSubKeysName);
            }
            return key;
        }

        public void DeleteSubKey(string subkey)
        {
            this.DeleteSubKey(subkey, true);
        }

        public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
        {
            RegistryKeyMock key;
            if (this.subkeys.TryGetValue(subkey, out key))
            {
                if (key.SubKeyCount > 0)
                {
                    throw new InvalidOperationException(String.Format("Registry key '{0}' has subkeys and cannot be deleted.", subkey));
                }
            }
            if (throwOnMissingSubKey && key == null)
            {
                throw new ArgumentException("Registry key '{0}' does not exist.");
            }
            this.subkeys.Remove(subkey);
        }

        public void DeleteSubKeyTree(string subkey)
        {
            this.DeleteSubKeyTree(subkey, true);
        }

        public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey)
        {
            RegistryKeyMock key;
            if (throwOnMissingSubKey && !this.subkeys.TryGetValue(subkey, out key))
            {
                throw new ArgumentException("Registry key '{0}' does not exist.");
            }
            this.subkeys.Remove(subkey);
        }

        public void DeleteValue(string name, bool throwOnMissingValue)
        {
            throw new NotImplementedException();
        }

        public void DeleteValue(string name)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public IRegistryKey FromHandle(ISafeRegistryHandle handle, RegistryView view)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey FromHandle(ISafeRegistryHandle handle)
        {
            throw new NotImplementedException();
        }

        public IRegistrySecurity GetAccessControl(AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        public IRegistrySecurity GetAccessControl()
        {
            throw new NotImplementedException();
        }

        public string[] GetSubKeyNames()
        {
            return this.subkeys.Keys.OrderBy(key => key, StringComparer.OrdinalIgnoreCase).ToArray();
        }

        public object GetValue(string name, object defaultValue, RegistryValueOptions options)
        {
            throw new NotImplementedException();
        }

        public object GetValue(string name, object defaultValue)
        {
            throw new NotImplementedException();
        }

        public object GetValue(string name)
        {
            if (name == null)
            {
                name = "";
            }

            Tuple<object, RegistryValueKind> obj;
            if (!this.values.TryGetValue(name, out obj))
            {
                return null;
            }
            return obj.Item1;
        }

        public RegistryValueKind GetValueKind(string name)
        {
            if (name == null)
            {
                name = "";
            }

            Tuple<object, RegistryValueKind> obj;
            if (!this.values.TryGetValue(name, out obj))
            {
                throw new IOException(String.Format("Registry key value '{0}' does not exist.", name));
            }
            return obj.Item2;
        }

        public string[] GetValueNames()
        {
            return this.values.Keys.OrderBy(key => key, StringComparer.OrdinalIgnoreCase).ToArray();
        }

        public ISafeRegistryHandle Handle
        {
            get { throw new NotImplementedException(); }
        }

        public void Initialize(RegistryKey registryKey)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return this.name; }
        }

        public IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName, RegistryView view)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey OpenSubKey(string name, bool writable)
        {
            return this.OpenSubKey(name);
        }

        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey OpenSubKey(string name)
        {
            string subKeyName;
            var baseKeyName = this.GetBaseKeyName(name, out subKeyName);

            RegistryKeyMock key;
            this.subkeys.TryGetValue(baseKeyName, out key);
            if (key == null)
            {
                return null;
            }

            if (subKeyName != null)
            {
                return key.OpenSubKey(subKeyName);
            }

            return key;
        }

        public RegistryKey RegistryKeyInstance
        {
            get { throw new NotImplementedException(); }
        }

        public void SetAccessControl(IRegistrySecurity registrySecurity)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string name, object value)
        {
            this.SetValue(name, value, RegistryValueKind.Unknown);
        }

        public void SetValue(string name, object value, RegistryValueKind valueKind)
        {
            if (name == null)
            {
                name = "";
            }

            var keyValue = new Tuple<object, RegistryValueKind>(value, valueKind);
            this.values[name] = keyValue;
        }

        public int SubKeyCount
        {
            get { return this.subkeys.Count; }
        }

        public int ValueCount
        {
            get { return this.values.Count; }
        }

        public RegistryView View
        {
            get { return RegistryView.Default; }
        }

        public void Dispose()
        {
        }

        private string GetBaseKeyName(string fullKeyName, out string subKeyName)
        {
            subKeyName = null;

            var baseKeyName = fullKeyName;
            var separatorIndex = baseKeyName.IndexOf('\\');
            if (separatorIndex >= 0)
            {
                baseKeyName = fullKeyName.Substring(0, separatorIndex);
                subKeyName = fullKeyName.Substring(separatorIndex + 1);
            }

            return baseKeyName;
        }
    }
}
