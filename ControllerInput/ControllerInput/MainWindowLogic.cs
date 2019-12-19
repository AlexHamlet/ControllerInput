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
        Gamepad gpad;

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

        public void StickHandle()
        {
            try
            {
                if (gpad != null)
                {
                    int delay = 1;
                    var cancellationTokenSource = new CancellationTokenSource();
                    var token = cancellationTokenSource.Token;
                    var listener = Task.Factory.StartNew(() =>
                    {
                        while (true)
                        {
                            // poll hardware
                            gpad.stickHandle();
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
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

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

    }
}
