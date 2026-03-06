using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    internal class Alarm
    {
        public Action method;
        public int value = -1;
        public Alarm(Action method)
        {
            this.method = method;
        }
        public void Update()
        {
            if (this.value > 0)
            {
                this.value--;
            }
            else if (this.value == 0)
            {
                this.value = -1;
                this.method();
            }
        }
        public void Start(int frames)
        {
            this.value = frames;
        }
    }
}