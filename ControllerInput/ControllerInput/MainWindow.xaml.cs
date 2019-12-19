﻿using System;
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

namespace ControllerInput
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainWindowLogic mwl;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                mwl = new MainWindowLogic();
                cmbbxDevices.ItemsSource = mwl.getSticks();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cmbbxDevices.ItemsSource = mwl.getSticks();
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
                Joystick selected = (Joystick)cmbbxDevices.SelectedItem;
                if (selected != null)
                {
                    mwl.SelectStick(selected);
                    mwl.StickHandle();

                    int delay = 1;
                    var cancellationTokenSource = new CancellationTokenSource();
                    var token = cancellationTokenSource.Token;
                    var listener = Task.Factory.StartNew(() =>
                    {
                        while (true)
                        {
                            // poll hardware
                            Thread.Sleep(delay);
                            if (token.IsCancellationRequested)
                                break;
                        }

                        // cleanup, e.g. close connection
                    }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
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
