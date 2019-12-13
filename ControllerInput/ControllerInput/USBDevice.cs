using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace ControllerInput
{
    class USBDevice
    {
        public string DeviceID { get; private set; }
        public string PnpDeviceId { get; private set; }
        public string FriendlyName { get; private set; }
        public string Description { get; private set; }

        public USBDevice(string deviceID, string pnpDeviceId, string friendlyName, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceId = pnpDeviceId;
            this.FriendlyName = friendlyName;
            this.Description = description;
        }

        public override string ToString()
        {
            return FriendlyName;
            //return string.Format("{0} {1} {2} {3}",DeviceID, PnpDeviceId, FriendlyName, Description);
        }

        public static List<USBDevice> getUSBDevices()
        {
            List<USBDevice> devices = new List<USBDevice>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity WHERE Name like 'XBOX%' or Name like 'HID%'"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDevice(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Name"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();

            return devices;
        }
    }
}
