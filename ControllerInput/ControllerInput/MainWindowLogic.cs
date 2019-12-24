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

        /// <summary>
        /// Gets a list of Axis from the gamepad object
        /// </summary>
        /// <returns>A list<int> of axis values</returns>
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

        /// <summary>
        /// Gets a list of Button values from the gamepad object
        /// </summary>
        /// <returns>A List<bool> of button values</returns>
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
        /// Gets the values of Acceleration Sliders on the gamepad device
        /// </summary>
        /// <returns>List<int> of Slider values</returns>
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

        /// <summary>
        /// Gets the values of Force Sliders on the gamepad device
        /// </summary>
        /// <returns>List<int> of Slider values</returns>
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

        /// <summary>
        /// Gets the values of POV on the gamepad device
        /// </summary>
        /// <returns>List<int> of POV values</returns>
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

        /// <summary>
        /// Gets the values of Sliders on the gamepad device
        /// </summary>
        /// <returns>List<int> of Slider values</returns>
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

        /// <summary>
        /// Gets the values of Velocity Sliders on the gamepad device
        /// </summary>
        /// <returns>List<int> of Slider values</returns>
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
