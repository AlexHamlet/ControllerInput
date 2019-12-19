using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControllerInput
{
    class Gamepad
    {
        //SlimDX initialization
        DirectInput input = new DirectInput();
        Joystick stick;
        public JoystickState state { get; private set; }

        //Array of joysticks to use
        Joystick[] sticks;
        //Joystick deadband value
        int deadBandValue = 20;
        //Access all of the axis of xbox 360 controller
        int xVal, yVal, ltVal, rtVal, x2Val, y2Val;
        //Access all of the buttons of xbox 360 controller
        const int aButton = 0, bButton = 1, xButton = 2, yButton = 3, lbButton = 4, rbButton = 5, selectButton = 6, startButton = 7, lsButton = 8, rsButton = 9;
        const int dpadUp = 0, dpadUpRight = 4500, dpadRight = 9000, dpadDownRight = 13500, dpadDown = 18000, dpadDownLeft = 22500, dpadLeft = 27000, dpadUpLeft = 31500;
        //click flags
        bool rtPressed = false;
        bool ltPressed = false;
        bool povPressed = false;
        bool arrowsHorizontalPressed = false;
        bool arrowsVerticalPressed = false;
        bool[] buttonCheck = new bool[10];

        //Mouse Events & Keyboard Events
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern void mouse_event(uint flag, uint _x, uint _y, uint btn, uint exInfo);
        private const int MOUSEEVENT_LEFTDONW = 0x02;
        private const int MOUSEEVENT_LEFTUP = 0x04;
        private const int MOUSEEVENT_RIGHTDOWN = 0x08;
        private const int MOUSEEVENT_RIGHTUP = 0x10;

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        //private static extern void Keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        private const byte KEYEVENTF_KEYUP = 0x0002;
        private const byte KEYBOARDEVENT_A = 0x41;
        private const byte KEYBOARDEVENT_B = 0x42;
        private const byte KEYBOARDEVENT_1 = 0x31;
        private const byte KEYBOARDEVENT_2 = 0x32;
        private const byte KEYBOARDEVENT_3 = 0x33;
        private const byte KEYBOARDEVENT_4 = 0x34;
        private const byte KEYBOARDEVENT_5 = 0x35;
        private const byte KEYBOARDEVENT_6 = 0x36;
        private const byte KEYBOARDEVENT_7 = 0x37;
        private const byte KEYBOARDEVENT_8 = 0x38;
        private const byte KEYBOARDEVENT_ESC = 0x1B;
        private const byte KEYBOARDEVENT_F10 = 0x79;
        private const byte KEYBOARDEVENT_BACKSPACE = 0x08;
        private const byte KEYBOARDEVENT_TAB = 0x09;
        private const byte KEYBOARDEVENT_SHIFT = 0x10;
        private const byte KEYBOARDEVENT_CTRL = 0x11;
        private const byte KEYBOARDEVENT_SPACEBAR = 0x20;
        private const byte KEYBOARDEVENT_LEFT = 0x25;
        private const byte KEYBOARDEVENT_UP = 0x26;
        private const byte KEYBOARDEVENT_RIGHT = 0x27;
        private const byte KEYBOARDEVENT_DOWN = 0x28;
        private byte[] keybdArray = new byte[] { KEYBOARDEVENT_A, KEYBOARDEVENT_B, KEYBOARDEVENT_ESC, KEYBOARDEVENT_SPACEBAR, KEYBOARDEVENT_BACKSPACE, KEYBOARDEVENT_TAB, KEYBOARDEVENT_ESC, KEYBOARDEVENT_F10, KEYBOARDEVENT_SHIFT, KEYBOARDEVENT_CTRL };

        public Gamepad(Joystick stick)
        {
            this.stick = stick;
            state = new JoystickState();
        }

        //Initializes the variable sticks with all of the found Joysticks
        public static List<Joystick> getSticks()
        {
            DirectInput input = new DirectInput();
            Joystick stick;
            List<SlimDX.DirectInput.Joystick> sticks = new List<SlimDX.DirectInput.Joystick>();

            foreach (DeviceInstance device in input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    stick = new SlimDX.DirectInput.Joystick(input, device.InstanceGuid);
                    stick.Acquire();
                    foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                    {
                        if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                        {
                            stick.GetObjectPropertiesById(((int)deviceObject.ObjectType)).SetRange(-100, 100);
                        }
                    }
                    sticks.Add(stick);
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }
            }
            return sticks;
        }
        //Handles the state of the Joysticks
        public void stickHandle()
        {
            state = stick.GetCurrentState();
            //Checks for axis states
            xVal = state.X;
            yVal = state.Y;
            //Seperates the Z value between the trigger buttons
            if (state.Z < 0)
            {
                rtVal = state.Z;
                ltVal = 0;
            }
            else
            {
                rtVal = 0;
                ltVal = state.Z;
            }
            //Checks for the second joystick on the controller
            x2Val = state.RotationX;
            y2Val = state.RotationY;
            //If the first joystick is actually moved, move the mouse accordingly
            if (xVal > deadBandValue || xVal < -deadBandValue || yVal > deadBandValue || yVal < -deadBandValue)
            {
                MouseMove(xVal, yVal);
            }
            //If the second joystick is actually moved, press the arrow keys accordingly
            if (!arrowsHorizontalPressed)
            {
                if (x2Val > 50)
                {
                    SendKeys.Send("{RIGHT}");
                    //Keybd_event(KEYBOARDEVENT_RIGHT, 0, 0, 0);
                    arrowsHorizontalPressed = true;
                }
                if (x2Val < -50)
                {
                    SendKeys.Send("{LEFT}");
                    //Keybd_event(KEYBOARDEVENT_LEFT, 0, 0, 0);
                    arrowsHorizontalPressed = true;
                }
            }
            else if (arrowsHorizontalPressed && Math.Abs(x2Val) < 50)
            {
                //Keybd_event(KEYBOARDEVENT_RIGHT, 0, KEYEVENTF_KEYUP, 0);
                //Keybd_event(KEYBOARDEVENT_LEFT, 0, KEYEVENTF_KEYUP, 0);
                arrowsHorizontalPressed = false;
            }

            if (!arrowsVerticalPressed)
            {
                if (y2Val > 50)
                {
                    SendKeys.Send("{DOWN}");
                    //Keybd_event(KEYBOARDEVENT_DOWN, 0, 0, 0);
                    arrowsVerticalPressed = true;
                }
                if (y2Val < -50)
                {
                    SendKeys.Send("{UP}");
                    //Keybd_event(KEYBOARDEVENT_UP, 0, 0, 0);
                    arrowsVerticalPressed = true;
                }
            }
            else if (arrowsVerticalPressed && Math.Abs(y2Val) < 50)
            {
                //Keybd_event(KEYBOARDEVENT_DOWN, 0, KEYEVENTF_KEYUP, 0);
                //Keybd_event(KEYBOARDEVENT_UP, 0, KEYEVENTF_KEYUP, 0);
                arrowsVerticalPressed = false;
            }

            //Check for button states.
            bool[] buttons = state.GetButtons();
            int[] pov = state.GetPointOfViewControllers();


            //Left Click
            if (Math.Abs(ltVal) > 50)
            {
                if (ltPressed == false)
                {
                    mouse_event(MOUSEEVENT_LEFTDONW, 0, 0, 0, 0);
                    ltPressed = true;
                }
            }
            else
            {
                if (ltPressed == true)
                {
                    mouse_event(MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
                    ltPressed = false;
                }
            }
            //Right Click
            if (Math.Abs(rtVal) > 50)
            {
                if (rtPressed == false)
                {
                    mouse_event(MOUSEEVENT_RIGHTDOWN, 0, 0, 0, 0);
                    rtPressed = true;
                }
            }
            else
            {
                if (rtPressed == true)
                {
                    mouse_event(MOUSEEVENT_RIGHTUP, 0, 0, 0, 0);
                    rtPressed = false;
                }
            }

            //Handles Buttons
            for (int p = 0; p < keybdArray.Length; p++)
            {
                checkButton(p, keybdArray[p], buttons[p]);
            }


            //POV Buttons
            if (!povPressed && pov[0] != -1)
            {
                switch (pov[0])
                {
                    case dpadUp:
                        SendKeys.Send("{A}");
                        //Keybd_event(KEYBOARDEVENT_1, 0, 0, 0);
                        povPressed = true;
                        break;
                    case dpadUpRight:
                        //Keybd_event(KEYBOARDEVENT_2, 0, 0, 0);
                        povPressed = true;
                        break;
                    case dpadRight:
                        //Keybd_event(KEYBOARDEVENT_3, 0, 0, 0);
                        break;
                    case dpadDownRight:
                        //Keybd_event(KEYBOARDEVENT_4, 0, 0, 0);
                        povPressed = true;
                        break;
                    case dpadDown:
                        // Keybd_event(KEYBOARDEVENT_5, 0, 0, 0);
                        povPressed = true;
                        break;
                    case dpadDownLeft:
                        // Keybd_event(KEYBOARDEVENT_6, 0, 0, 0);
                        povPressed = true;
                        break;
                    case dpadLeft:
                        //Keybd_event(KEYBOARDEVENT_7, 0, 0, 0);
                        povPressed = true;
                        break;
                    case dpadUpLeft:
                        //Keybd_event(KEYBOARDEVENT_8, 0, 0, 0);
                        povPressed = true;
                        break;
                }
            }
            else if (povPressed && pov[0] == -1)
            {
                //Keybd_event(KEYBOARDEVENT_1, 0, KEYEVENTF_KEYUP, 0);
                //Keybd_event(KEYBOARDEVENT_2, 0, KEYEVENTF_KEYUP, 0);
                //Keybd_event(KEYBOARDEVENT_3, 0, KEYEVENTF_KEYUP, 0);
                //Keybd_event(KEYBOARDEVENT_4, 0, KEYEVENTF_KEYUP, 0);
                //Keybd_event(KEYBOARDEVENT_5, 0, KEYEVENTF_KEYUP, 0);
                //Keybd_event(KEYBOARDEVENT_6, 0, KEYEVENTF_KEYUP, 0);
                //Keybd_event(KEYBOARDEVENT_7, 0, KEYEVENTF_KEYUP, 0);
                //Keybd_event(KEYBOARDEVENT_8, 0, KEYEVENTF_KEYUP, 0);
                povPressed = false;
            }
        }

        //handles controller to keyboard action
        private void checkButton(int button, byte key, bool isPressed)
        {

            if (!buttonCheck[button] && isPressed)
            {
                //Keybd_event(key, 0, 0, 0);
                buttonCheck[button] = true;
            }
            else if (buttonCheck[button] && !isPressed)
            {
                // Keybd_event(key, 0, KEYEVENTF_KEYUP, 0);
                buttonCheck[button] = false;
            }
        }

        //Moves the cursor on the screen
        public void MouseMove(int xpos, int ypos)
        {
            var pos = System.Windows.Forms.Cursor.Position;
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(pos.X + (xpos - deadBandValue), pos.Y + (ypos - deadBandValue));
        }


    }
}
