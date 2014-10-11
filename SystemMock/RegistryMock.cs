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
            ////int separatorIndex = keyName.IndexOf('\\');
            ////var rootKeyName = keyName.Substring(0, separatorIndex);
            ////var subKeyName = keyName.Substring(separatorIndex + 1);

            ////var rootKey = this.keys[rootKeyName];
            ////var key = rootKey.CreateSubKey(subKeyName);
            var rootKey = this.keys[keyName];
            rootKey.SetValue(valueName, value);
        }

        public IRegistryKey Users
        {
            get { return this.usersKey; }
        }
    }
}
