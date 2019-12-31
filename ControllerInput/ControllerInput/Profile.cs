using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerInput
{
    class Profile
    {
        //This class is going to store input profiles
        //Default profile should be included, additional profiles may be created by the user
        //this should be referenced when keyboard/mouse input is required

        //Look into Entity Framework
        public Profile selected { get; private set; }
        public Dictionary<int, byte> buttons { get; private set; }
        
        public static List<Profile> getProfiles()
        {
            return null;
        }

        public void setProfile(Profile prof)
        {
            try
            {
                selected = prof;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void setButton(int button, byte key)
        {
            buttons.Remove(button);
            buttons.Add(button, key);
        }

        public byte getButton(int button)
        {
            byte key;
            buttons.TryGetValue(button, out key);
            return key;
        }
    }
}
