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

                    if (currentKey == null)
                    {
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

                        if (valueValue.StartsWith("\""))
                        {
                            valueValue = valueValue.Trim('"');
                            valueValue = valueValue.Replace(@"\\", @"\");
                        }
                        else if (valueValue.StartsWith("hex:"))
                        {
                            var bytes = HexValueToBytes(valueValue);
                            currentKey.SetValue(valueName, bytes);
                            continue;
                        }

                        currentKey.SetValue(valueName, valueValue);
                    }
                }
            }
        }

        private byte[] HexValueToBytes(string hexValue)
        {
            hexValue = hexValue.Remove(0, "hex:".Length);
            var values = hexValue.Split(',');

            var buffer = new byte[values.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(values[i], 16);
            }

            return buffer;
        }
    }
}
