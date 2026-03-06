using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    internal class TestPlatform : Sprite
    {
        Form1 client;
        public TestPlatform(Form1 client)
        {
            this.client = client;
            this.width = 400;
            this.height = 16;
            this.x = 0;
            this.y = 128;

            this.set_sprite(@"..\..\sprites\grazz.png",400,16);

            this.client.colliders.collisions_obstacles.Add(this);
        }
    }
}