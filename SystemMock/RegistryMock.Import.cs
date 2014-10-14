using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using SystemInterface.Microsoft.Win32;

namespace SystemMock
{
    public partial class RegistryMock
    {
        public void Import(string registryText)
        {
            using (var reader = new StringReader(registryText))
            {
                string line;
                IRegistryKey currentKey = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("["))
                    {
                        var registryKey = line.TrimStart('[').TrimEnd(']');

                        currentKey = this.CreateSubKey(registryKey);
                        continue;
                    }

                    int separator = line.IndexOf('=');
                    if (separator > 0)
                    {
                        var valueName = line.Substring(0, separator);
                        var valueValue = line.Substring(separator + 1);

                        if (valueName == "@")
                        {
                            valueName = "";
                        }
                        valueName = valueName.Trim('"');
                        valueValue = valueValue.Trim('"');

                        if (currentKey != null)
                        {
                            currentKey.SetValue(valueName, valueValue);
                        }
                    }
                }
            }
        }
    }
}
