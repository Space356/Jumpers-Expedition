using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    internal class Collider
    {
        public Sprite obj;
        public Collider(Sprite obj)
        {
            this.obj = obj;
        }

        public bool place_meeting(float x, float y, List<Sprite> collisions)
        {
            bool val = false;
            for (int i = 0; i < collisions.Count; i++)
            {
                Sprite inst = collisions[i];
                if (x + this.obj.width > inst.x &&
                x < inst.x + inst.width &&
                y + this.obj.height > inst.y &&
                y < inst.y + inst.height)
                {
                    val = true;
                }
            }
            return (val);
        }
        public bool place_meeting(float x, float y, Sprite collision)
        {
            bool val = false;
            if (x + this.obj.width > collision.x &&
            x < collision.x + collision.width &&
            y + this.obj.height > collision.y &&
            y < collision.y + collision.height)
            {
                val = true;
            }
            return (val);
        }

        public bool place_meeting(float x, float y, tile_manager tiles)
        {
            bool val = false;
            int new_x = (int)Math.Floor(x / 16);
            int new_y = (int)Math.Floor(y / 16);
            int new_width = (this.obj.width / 16)+2;
            int new_height = (this.obj.height / 16)+2;

            if(new_y >= 0 && new_x >= 0 && new_y+new_height <= tiles.room_data.Count && new_x + new_width <= tiles.room_data[0].Count)
            {
                for (int i = new_y; i < new_y + new_height; i++)
                {
                    for (int j = new_x; j < new_x + new_width; j++)
                    {
                        int t = 0;
                        int.TryParse(tiles.room_data[i][j], out t);

                        if (t > 0)
                        {
                            bool x_overlaps = (x < (j * 16) + 16) && (x + obj.width > j * 16);
                            bool y_overlaps = (y < (i * 16) + 16) && (y + obj.height > i * 16);
                            if (x_overlaps && y_overlaps)
                            {
                                val = true;
                            }
                        }
                    }
                }
            }
            else
            {
                val = true;
            }
                return (val);
        }

        public bool point_meeting(float x, float y, tile_manager tiles)
        {
            bool val = false;
            int new_x = (int)Math.Floor(x / 16);
            int new_y = (int)Math.Floor(y / 16);

            if (new_y >= 0 && new_x >= 0 && new_y <= tiles.room_data.Count && new_x <= tiles.room_data[0].Count)
            {
                int t = 0;
                int.TryParse(tiles.room_data[new_y][new_x], out t);

                if (t > 0)
                {
                    val = true;
                }
            }
            return (val);
        }

        public bool point_meeting(float x, float y, Sprite collision)
        {
            bool val = false;
            if (x > collision.x &&
            x < collision.x + collision.width &&
            y > collision.y &&
            y < collision.y + collision.height)
            {
                val = true;
            }
            return (val);
        }
        public bool point_meeting(float x, float y, List<Sprite> collisions)
        {
            bool val = false;
            for (int i = 0; i < collisions.Count; i++)
            {
                Sprite inst = collisions[i];
                if (x > inst.x &&
                x < inst.x + inst.width &&
                y > inst.y &&
                y < inst.y + inst.height)
                {
                    val = true;
                }
            }
            return (val);
        }

        public Sprite instance_meeting(float x, float y, List<Sprite> collisions)
        {
            Sprite val = null;
            for (int i = 0; i < collisions.Count; i++)
            {
                Sprite inst = collisions[i];
                if (x + this.obj.width > inst.x &&
                x < inst.x + inst.width &&
                y + this.obj.height > inst.y &&
                y < inst.y + inst.height)
                {
                    val = inst;
                }
            }
            return (val);
        }

        public void draw_instances(Graphics g, List<Sprite> collisions)
        {
            using (SolidBrush pen = new SolidBrush(Color.Green))
            {
                g.DrawString(collisions.Count.ToString(), new Font("Arial", 12), pen, 256, 108);
                for (int i = 0; i < collisions.Count; i++)
                {
                    Sprite inst = collisions[i];
                    g.DrawString(inst.x.ToString() + " " + inst.y.ToString(), new Font("Arial", 12), pen, 256, 200 + (i * 16));
                }
            }
        }
    }
}