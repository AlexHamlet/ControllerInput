using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.XInput;
using SlimDX.DirectInput;

namespace ControllerInput
{
    class Xbox360Controller
    {
        private readonly Controller controller;
        public State state { get; private set; }

        private Action<object> getState;
        private Guid Guid;
        private string Name;

        public Xbox360Controller(Controller c)
        {
            controller = c;
            getState = (object obj) =>
            {
                Update();
            };
        }

        public void Update()
        {
            if (controller.IsConnected == false)
                return;

            state = controller.GetState();
        }

        public static List<Xbox360Controller> Available()
        {
            List<Xbox360Controller> result = new List<Xbox360Controller>();
            DirectInput dinput = new DirectInput();
            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                //Xbox360Controller dev = new Xbox360Controller();
                //dev.Guid = di.InstanceGuid;
                //dev.Name = di.InstanceName;
                //result.Add(dev);
            }
            return result;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
