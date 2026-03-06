using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jumper_s_Expedition
{
    public class GlobalKeyManager
    {
        public List<KeyManager> possible_keys = new List<KeyManager>();
        public GlobalKeyManager()
        {
            possible_keys.Add(new KeyManager(Keys.A));
            possible_keys.Add(new KeyManager(Keys.D));
            possible_keys.Add(new KeyManager(Keys.Left));
            possible_keys.Add(new KeyManager(Keys.Right));
            possible_keys.Add(new KeyManager(Keys.Escape));
            possible_keys.Add(new KeyManager(Keys.Space));
        }
        public void Update()
        {
            foreach (KeyManager key in possible_keys)
            {
                //the init_... variables are set to true on the key down event, which can be called at any point in the frame causing inconsistency, so it sets another variable to true to be updated on time on the next frame.
                if (key.pressed_up) {key.pressed_up = false;}
                if (key.pressed_down) { key.pressed_down = false;}

                if (key.init_pressed_down && !key.pressed)
                {
                    key.pressed_down = true;
                    key.pressed = true;
                    key.init_pressed_down = false;
                }
                if (key.init_pressed_up && key.pressed)
                {
                    key.pressed_up = true;
                    key.pressed = false ;
                    key.init_pressed_up = false;
                }
            }
        }
    }
}