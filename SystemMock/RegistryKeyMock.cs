using System;
using System.Collections.Generic;
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
        private Dictionary<string, object> values;

        public RegistryKeyMock(string name)
        {
            this.name = name;
            this.values = new Dictionary<string, object>();
            this.subkeys = new Dictionary<string, RegistryKeyMock>();
        }

        public void Close()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
        {
            throw new NotImplementedException();
        }

        public void DeleteSubKey(string subkey)
        {
            throw new NotImplementedException();
        }

        public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey)
        {
            throw new NotImplementedException();
        }

        public void DeleteSubKeyTree(string subkey)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

            return this.values[name];
        }

        public RegistryValueKind GetValueKind(string name)
        {
            throw new NotImplementedException();
        }

        public string[] GetValueNames()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey OpenSubKey(string name)
        {
            throw new NotImplementedException();
        }

        public RegistryKey RegistryKeyInstance
        {
            get { throw new NotImplementedException(); }
        }

        public void SetAccessControl(IRegistrySecurity registrySecurity)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string name, object value, RegistryValueKind valueKind)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string name, object value)
        {
            if (name == null)
            {
                name = "";
            }

            this.values[name] = value;
        }

        public int SubKeyCount
        {
            get { return 0; }
        }

        public int ValueCount
        {
            get { return 0; }
        }

        public RegistryView View
        {
            get { return RegistryView.Default; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
