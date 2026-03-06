using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition.General_Objects
{
    internal class FloatingPlatform : Sprite
    {
        float hsp = 0;
        float vsp = 0;
        tile_manager tiles;
        Form1 client;

        public FloatingPlatform(Form1 client, float x, float y, float hsp, float vsp, tile_manager tiles, int size)
        {
            this.x = x;
            this.y = y;
            this.hsp = hsp;
            this.vsp = vsp;
            this.tiles = tiles;
            this.width = size*16;
            this.height = 16;
            this.client = client;
            this.collider_list = this.client.colliders.collisions_movers;
            this.collider_list.Add(this);
            sprite_init_index(this.client,"moving_platform");
        }
        public override void Update()
        {
            if(!client.paused)
            {
                if (tiles.room_data[(int)Math.Floor(y / 16)][(int)Math.Floor(x / 16)] == "st")
                {
                    hsp *= -1;
                    vsp *= -1;
                }
                x += hsp;
                y += vsp;
            }
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Blue),x*2,y*2,this.width*2,this.height*2);
        }
    }
}