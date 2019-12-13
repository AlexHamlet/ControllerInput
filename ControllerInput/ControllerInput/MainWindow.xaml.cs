using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControllerInput
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cmbbxDevices.ItemsSource = USBDevice.getUSBDevices();
        }

        private void cmbbxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                USBDevice usb = (USBDevice)(cmbbxDevices.SelectedItem);
                int devId, ProdId;
                //Int32.TryParse(usb.DeviceID, out devId);
                //Int32.TryParse(usb.PnpDeviceId, out ProdId);
                devId = 9610;
                ProdId = 4102;

                UsbLibrary.SpecifiedDevice selected = UsbLibrary.SpecifiedDevice.FindSpecifiedDevice(devId, ProdId);
                if (selected != null)
                {
                    lblSelectedDevice.Content = selected.ToString();
                }
                else
                {
                    lblSelectedDevice.Content = "Device not found";
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}
