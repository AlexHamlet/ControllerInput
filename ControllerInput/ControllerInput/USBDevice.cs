using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;

namespace ControllerInput
{
    class USBDevice
    {
        //Variables that I might need?
        public string VendorID { get; private set; }
        public string ProductId { get; private set; }
        public string UsagePage { get; private set; }
        public string UsageID { get; private set; }
        public string FriendlyName { get; private set; }
        public string Description { get; private set; }
        public string[] HardwareId { get; private set; }

        /// <summary>
        /// Constructor for a USB Device
        /// </summary>
        /// <param name="friendlyName">The listed name/What you would like to call it</param>
        /// <param name="description">Unnecessary</param>
        /// <param name="hardwareID">The array of hardwareIDs used by the system</param>
        public USBDevice(string friendlyName, string description, string[] hardwareID)
        {
            try
            {
                this.FriendlyName = friendlyName;
                this.Description = description;
                this.HardwareId = hardwareID;

                //Find VendorID, ProductID, UsageID, and UsagePage
                for (int p = 0; p < HardwareId.Length; p++)
                {
                    if (VendorID == null)
                    {
                        if (hardwareID[p].Contains("VID"))
                        {
                            VendorID = HardwareId[p].Substring(HardwareId[p].LastIndexOf("VID_") + 4, 4);
                        }
                    }
                    if (ProductId == null)
                    {
                        if (hardwareID[p].Contains("PID"))
                        {
                            ProductId = HardwareId[p].Substring(HardwareId[p].LastIndexOf("PID_") + 4, 4);
                        }
                    }
                    if (UsageID == null)
                    {
                        if (hardwareID[p].Contains("U:"))
                        {
                            UsageID = HardwareId[p].Substring(HardwareId[p].LastIndexOf("U:") + 2, 4);
                        }
                    }
                    if (UsagePage == null)
                    {
                        if (hardwareID[p].Contains("UP:"))
                        {
                            UsagePage = HardwareId[p].Substring(HardwareId[p].LastIndexOf("UP:") + 3, 4);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Returns the friendly name
        /// </summary>
        /// <returns>The friendly name of the USBDevice</returns>
        public override string ToString()
        {
            try
            {
                return FriendlyName;
                //return string.Format("{0} {1} {2} {3}",VendorID, ProductId, UsageID, UsagePage);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the VendorId, ProductId, UsageId, and UsagePage numbers
        /// </summary>
        /// <returns>VendorId:{0}\nProductId:{1}\nUsageID:{2}\nUsagePage:{3}</returns>
        public string getIDs()
        {
            try
            {
                return string.Format("VendorId:{0}\nProductId:{1}\nUsageID:{2}\nUsagePage:{3}", VendorID, ProductId, UsageID, UsagePage);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of all USB Devices that have friendly names starting with HID or XBOX
        /// </summary>
        /// <returns>A List of all HID Compliant USB devices detected by the system.</returns>
        public static List<USBDevice> getUSBDevices()
        {
            try
            {
                List<USBDevice> devices = new List<USBDevice>();

                ManagementObjectCollection collection;
                using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity WHERE Name like 'HID%'"))
                    collection = searcher.Get();

                foreach (var device in collection)
                {
                    devices.Add(new USBDevice(
                    (string)device.GetPropertyValue("Name"),
                    (string)device.GetPropertyValue("Description"),
                    (string[])device.GetPropertyValue("HardwareID")
                    ));
                }

                collection.Dispose();

                return devices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

    }
}
