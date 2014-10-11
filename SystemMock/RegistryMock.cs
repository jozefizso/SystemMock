using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterface.Microsoft.Win32;
using Microsoft.Win32;

namespace SystemMock
{
    public class RegistryMock : IRegistry
    {
        private RegistryKeyMock classesRootKey;
        private RegistryKeyMock currentConfigKey;
        private RegistryKeyMock currentUserKey;
        private RegistryKeyMock localMachineKey;
        private RegistryKeyMock performanceDataKey;
        private RegistryKeyMock usersKey;

        private Dictionary<string, RegistryKeyMock> keys; 

        public RegistryMock()
        {
            this.classesRootKey = new RegistryKeyMock(RegistryConstants.HKEY_CLASSES_ROOT);
            this.currentConfigKey = new RegistryKeyMock(RegistryConstants.HKEY_CURRENT_CONFIG);
            this.currentUserKey = new RegistryKeyMock(RegistryConstants.HKEY_CURRENT_USER);
            this.localMachineKey = new RegistryKeyMock(RegistryConstants.HKEY_LOCAL_MACHINE);
            this.performanceDataKey = new RegistryKeyMock(RegistryConstants.HKEY_PERFORMANCE_DATA);
            this.usersKey = new RegistryKeyMock(RegistryConstants.HKEY_USERS);

            this.keys = new Dictionary<string, RegistryKeyMock>();
            this.keys[RegistryConstants.HKEY_CLASSES_ROOT] = this.classesRootKey;
            this.keys[RegistryConstants.HKEY_CURRENT_CONFIG] = this.currentConfigKey;
            this.keys[RegistryConstants.HKEY_CURRENT_USER] = this.currentUserKey;
            this.keys[RegistryConstants.HKEY_LOCAL_MACHINE] = this.localMachineKey;
            this.keys[RegistryConstants.HKEY_PERFORMANCE_DATA] = this.performanceDataKey;
            this.keys[RegistryConstants.HKEY_USERS] = this.usersKey;
        }

        public IRegistryKey ClassesRoot
        {
            get { return this.classesRootKey; }
        }

        public IRegistryKey CurrentConfig
        {
            get { return this.currentConfigKey; }
        }

        public IRegistryKey CurrentUser
        {
            get { return this.currentUserKey; }
        }

        public object GetValue(string keyName, string valueName, object defaultValue)
        {
            throw new NotImplementedException();
        }

        public IRegistryKey LocalMachine
        {
            get { return this.localMachineKey; }
        }

        public IRegistryKey PerformanceData
        {
            get { return this.performanceDataKey; }
        }

        public void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string keyName, string valueName, object value)
        {
            string subKeyName;
            var rootKeyName = this.GetRootKeyName(keyName, out subKeyName);
            var rootKey = this.keys[rootKeyName];
            var subKey = (IRegistryKey)rootKey;
            if (subKeyName != null)
            {
                subKey = rootKey.CreateSubKey(subKeyName);
            }
            subKey.SetValue(valueName, value);
        }

        public IRegistryKey Users
        {
            get { return this.usersKey; }
        }

        private string GetRootKeyName(string fullKeyName, out string subKeyName)
        {
            subKeyName = null;

            var rootKeyName = fullKeyName;
            var separatorIndex = rootKeyName.IndexOf('\\');
            if (separatorIndex >= 0)
            {
                rootKeyName = fullKeyName.Substring(0, separatorIndex);
                subKeyName = fullKeyName.Substring(separatorIndex + 1);
            }

            return rootKeyName;
        }
    }
}
