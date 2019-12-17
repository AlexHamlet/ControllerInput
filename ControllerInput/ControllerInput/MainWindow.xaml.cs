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
            try
            {
            InitializeComponent();
            cmbbxDevices.ItemsSource = USBDevice.getUSBDevices();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Attempt to make an object out of the selected Device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbbxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                USBDevice selected = (USBDevice)cmbbxDevices.SelectedItem;
                lblSelectedDevice.Content = selected.getIDs();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Hanldes errors thrown by the program by printing the error to the screen
        /// </summary>
        /// <param name="sClass">Class throwing the error</param>
        /// <param name="sMethod">Method throwing the error</param>
        /// <param name="sMessage">Error message</param>
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
