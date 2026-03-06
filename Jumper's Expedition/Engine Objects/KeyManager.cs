using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jumper_s_Expedition
{
    public class KeyManager
    {
        public Keys key;

        public bool init_pressed_up = false;
        public bool init_pressed_down = false;

        public bool pressed = false;
        public bool pressed_down = false;
        public bool pressed_up = false;
        public KeyManager(Keys key)
        {
            this.key = key;
        }
    }
}