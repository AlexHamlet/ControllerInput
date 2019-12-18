using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.XInput;

namespace ControllerInput
{
    class Xbox360Controller
    {
        private readonly Controller controller;
        State state;
        public int NumButtons { get; private set; }
        private readonly List<string> names = new List<string>();
        private readonly List<Func<bool>> actions = new List<Func<bool>>();

        public Xbox360Controller(Controller c)
        {
            controller = c;
            InitializeButtons();
            Update();
        }

        public void Update()
        {
            if (controller.IsConnected == false)
                return;

            state = controller.GetState();
        }

        public IEnumerable<Tuple<string, float>> GetFloats()
        {
            var g = state.Gamepad;
            const float f = 3.2768f;
            yield return new Tuple<string, float>("LeftThumbX", g.LeftThumbX / f);
            yield return new Tuple<string, float>("LeftThumbY", g.LeftThumbY / f);
            yield return new Tuple<string, float>("RightThumbX", g.RightThumbX / f);
            yield return new Tuple<string, float>("RightThumbY", g.RightThumbY / f);
            yield break;
        }

        void InitializeButtons()
        {
            const int dzp = 9000;
            const int dzn = -9000;
            const int dzt = 40;

            AddItem("A", () => (state.Gamepad.Buttons & GamepadButtonFlags.A) != 0);
            AddItem("B", () => (state.Gamepad.Buttons & GamepadButtonFlags.B) != 0);
            AddItem("X", () => (state.Gamepad.Buttons & GamepadButtonFlags.X) != 0);
            AddItem("Y", () => (state.Gamepad.Buttons & GamepadButtonFlags.Y) != 0);

            AddItem("Start", () => (state.Gamepad.Buttons & GamepadButtonFlags.Start) != 0);
            AddItem("Back", () => (state.Gamepad.Buttons & GamepadButtonFlags.Back) != 0);
            AddItem("LeftThumb", () => (state.Gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0);
            AddItem("RightThumb", () => (state.Gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0);
            AddItem("LeftShoulder", () => (state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0);
            AddItem("RightShoulder", () => (state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0);

            AddItem("DpadUp", () => (state.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0);
            AddItem("DpadDown", () => (state.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0);
            AddItem("DpadLeft", () => (state.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0);
            AddItem("DpadRight", () => (state.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0);

            AddItem("LStickUp", () => state.Gamepad.LeftThumbY >= dzp);
            AddItem("LStickDown", () => state.Gamepad.LeftThumbY <= dzn);
            AddItem("LStickLeft", () => state.Gamepad.LeftThumbX <= dzn);
            AddItem("LStickRight", () => state.Gamepad.LeftThumbX >= dzp);

            AddItem("RStickUp", () => state.Gamepad.RightThumbY >= dzp);
            AddItem("RStickDown", () => state.Gamepad.RightThumbY <= dzn);
            AddItem("RStickLeft", () => state.Gamepad.RightThumbX <= dzn);
            AddItem("RStickRight", () => state.Gamepad.RightThumbX >= dzp);

            AddItem("LeftTrigger", () => state.Gamepad.LeftTrigger > dzt);
            AddItem("RightTrigger", () => state.Gamepad.RightTrigger > dzt);
        }

        void AddItem(string name, Func<bool> pressed)
        {
            names.Add(name);
            actions.Add(pressed);
            NumButtons++;
        }

        public string ButtonName(int index)
        {
            return names[index];
        }

        public bool Pressed(int index)
        {
            return actions[index]();
        }
    }
}
