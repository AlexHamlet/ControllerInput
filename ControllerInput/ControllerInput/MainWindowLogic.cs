using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerInput
{
    class MainWindowLogic
    {
        /// <summary>
        /// Gamepad object
        /// </summary>
        Gamepad gpad;
        /// <summary>
        /// State of the Gamepad object
        /// </summary>
        JoystickState state;

        /// <summary>
        /// Get a list of valid Input Devices
        /// </summary>
        /// <returns>List<JoyStick> of valid input devices</JoyStick></returns>
        public List<Joystick> getSticks()
        {
            try
            {
                return Gamepad.getSticks();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Currently Performs all input functionality based on the JoystickState
        /// </summary>
        public void update()
        {
            try
            {
                if (gpad != null)
                {
                    //TODO: Allow the user to change later.
                    //17ms delay operates at approx 60fps
                    //int delay = 17;
                    //var cancellationTokenSource = new CancellationTokenSource();
                    //var token = cancellationTokenSource.Token;
                    //var listener = Task.Factory.StartNew(() =>
                    //{
                    //    while (true)
                    //    {
                    //        // poll hardware
                    //        //gpad.stickHandle();
                    //        gpad.update();
                    //        Thread.Sleep(delay);
                    //        if (token.IsCancellationRequested)
                    //            break;
                    //    }

                    //    // cleanup, e.g. close connection
                    //}, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    gpad.update();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Initializes the input device.
        /// </summary>
        /// <param name="stick"></param>
        public void SelectStick(Joystick stick)
        {
            try
            {
                gpad = new Gamepad(stick);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the State of the input device.
        /// </summary>
        /// <returns>JoystickState of the selected device</returns>
        public JoystickState getStickState()
        {
            try
            {
                return state;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public List<int> getAxis()
        {
            try
            {
                return gpad.getAxis();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public List<bool> getButtons()
        {
            try
            {
                return gpad.getButtons();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the values of all axis on the device
        /// </summary>
        /// <returns>List<int> of axis values</returns>
        public List<int> getAccelerationSliders()
        {
            try
            {
                return gpad.getAccelerationSliders();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public List<int> getForceSliders()
        {
            try
            {
                return gpad.getForceSliders();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public List<int> getPOV()
        {
            try
            {
                return gpad.getPOV();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public List<int> getSliders()
        {
            try
            {
                return gpad.getSliders();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public List<int> getVelocitySliders()
        {
            try
            {
                return gpad.getVelocitySliders();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
