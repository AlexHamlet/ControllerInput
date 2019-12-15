using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;

namespace ControllerInput
{
    class USBDevice
    {
        public string VendorID { get; private set; }
        public string ProductId { get; private set; }
        public string UsagePage { get; private set; }
        public string UsageID { get; private set; }
        public string FriendlyName { get; private set; }
        public string Description { get; private set; }
        public string[] HardwareId { get; private set; }

        public USBDevice(string friendlyName, string description, string[] hardwareID)
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

        public string getIDs()
        {
            return string.Format("VendorId:{0}\nProductId:{1}\nUsageID:{2}\nUsagePage:{3}", VendorID, ProductId, UsageID, UsagePage);
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
                (string)device.GetPropertyValue("Name"),
                (string)device.GetPropertyValue("Description"),
                (string[])device.GetPropertyValue("HardwareID")
                ));
            }

            collection.Dispose();

            return devices;
        }

        //// Find HID devices.
        //private async void EnumerateHidDevices(USBDevice usb)
        //{
        //    // Microsoft Input Configuration Device.
        //    ushort vendorId, productId, usagePage, usageId;

        //    ushort.TryParse(usb.VendorID, out vendorId);
        //    ushort.TryParse(usb.ProductId, out productId);
        //    ushort.TryParse(usb.UsagePage, out usagePage);
        //    ushort.TryParse(usb.UsageID, out usageId);

        //    // Create the selector.
        //    string selector =
        //        HIDDevice.GetDeviceSelector(usagePage, usageId, vendorId, productId);

        //    // Enumerate devices using the selector.
        //    var devices = await DeviceInformation.FindAllAsync(selector);

        //    if (devices.Any())
        //    {
        //        // At this point the device is available to communicate with
        //        // So we can send/receive HID reports from it or 
        //        // query it for control descriptions.
        //        info.Text = "HID devices found: " + devices.Count;

        //        // Open the target HID device.
        //        HidDevice device =
        //            await HidDevice.FromIdAsync(devices.ElementAt(0).Id,
        //            FileAccessMode.ReadWrite);

        //        if (device != null)
        //        {
        //            // Input reports contain data from the device.
        //            device.InputReportReceived += async (sender, args) =>
        //            {
        //                HidInputReport inputReport = args.Report;
        //                IBuffer buffer = inputReport.Data;

        //                // Create a DispatchedHandler as we are interracting with the UI directly and the
        //                // thread that this function is running on might not be the UI thread; 
        //                // if a non-UI thread modifies the UI, an exception is thrown.

        //                await this.Dispatcher.RunAsync(
        //                    CoreDispatcherPriority.Normal,
        //                    new DispatchedHandler(() =>
        //                    {
        //                        info.Text += "\nHID Input Report: " + inputReport.ToString() +
        //                        "\nTotal number of bytes received: " + buffer.Length.ToString();
        //                    }));
        //            };
        //        }

        //    }
        //    else
        //    {
        //        // There were no HID devices that met the selector criteria.
        //        info.Text = "HID device not found";
        //    }
        //}
    }
}
