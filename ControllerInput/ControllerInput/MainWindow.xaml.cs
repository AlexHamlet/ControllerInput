using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Runtime.InteropServices;
using SlimDX.DirectInput;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace ControllerInput
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Used to handle all calls to the controller.
        /// </summary>
        MainWindowLogic mwl;

        private delegate void UpdateDisplayDelegate();
        BackgroundWorker background;

        Joystick inuse;
        CancellationTokenSource cancellationTokenSource;
        CancellationToken token;

        /// <summary>
        /// Constructor. Initializes Logic class and device list.
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                //Initialize Logic Class
                mwl = new MainWindowLogic();
                //Initialize Backgroundworker
                background = new BackgroundWorker();
                background.WorkerReportsProgress = true;
                background.DoWork += Background_DoWork;
                //Populate Device List
                cmbbxDevices.ItemsSource = mwl.getSticks();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Background_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new UpdateDisplayDelegate(UpdateDisplay));
        }

        private void UpdateDisplay()
        {
            lblButtons.Content = "It worked!";
            // poll hardware
            string output;
            List<bool> bstates;
            mwl.update();
            output = "";
            bstates = mwl.getButtons();
            foreach (bool b in bstates)
            {
                output += b + "\n";
            }
            lblButtons.Content = output;
            lblAxis.Content = DisplayHelper(mwl.getAxis());
            lblAccSlider.Content = DisplayHelper(mwl.getAccelerationSliders());
            lblForceSlider.Content = DisplayHelper(mwl.getForceSliders());
            lblPOV.Content = DisplayHelper(mwl.getPOV());
            lblSliders.Content = DisplayHelper(mwl.getSliders());
            lblVelSlider.Content = DisplayHelper(mwl.getVelocitySliders());
        }

        private string DisplayHelper(List<int> Given)
        {
            string output = "";
            foreach(int val in Given)
            {
                output += val + "\n";
            }
            return output;
        }

        /// <summary>
        /// Refreshes the Device List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbbxDevices.ItemsSource = mwl.getSticks();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Initializes Selected Device and begins polling.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbbxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Joystick selected = (Joystick)cmbbxDevices.SelectedItem;
                if (selected != null)
                {
                    if (inuse != null && !selected.Equals(inuse))
                    {
                        cancellationTokenSource.Cancel();
                    }
                    inuse = selected;
                    mwl.SelectStick(selected);
                    mwl.update();

                    int delay = 17;
                    cancellationTokenSource = new CancellationTokenSource();
                    token = cancellationTokenSource.Token;
                    //TroubleShooting
                    string output;
                    List<bool> bstates;
                    var listener = Task.Factory.StartNew(() =>
                    {
                        while (true)
                        {
                            // poll hardware
                            //mwl.update();
                            //output = "";
                            //bstates = mwl.getButtons();
                            //foreach (bool b in bstates)
                            //{
                            //    output += "b\n";
                            //}
                            //lblSelectedDevice.Content = output;
                            background.RunWorkerAsync();

                            Thread.Sleep(delay);
                            if (token.IsCancellationRequested)
                                break;
                        }

                        // cleanup, e.g. close connection
                    }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
                else
                {
                    cancellationTokenSource.Cancel();
                }
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
                System.Windows.MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}
