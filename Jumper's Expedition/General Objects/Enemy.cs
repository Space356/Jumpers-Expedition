using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    internal class Enemy : Sprite
    {
        Form1 client;
        float hsp = 2;
        Collider collider;
        public int damage = 1;
        int first_frame = 2;
        public Enemy(Form1 client, float x, float y)
        {
            this.client = client;
            this.x = x;
            this.y = y+4;
            this.sprite_init_index(this.client,"enemy");

            this.collider_list = this.client.colliders.collisions_damage;
            this.collider_list.Add(this);

            set_sprite(@"..\..\sprites\jumpy.png", 12, 12);
            this.collider = new Collider(this);
            this.y_offset = -4;
            this.x_offset = -2;
        }

        public override void Update()
        {
            if (!this.client.paused)
            {
                if (first_frame > 0)
                {
                    first_frame--;
                }
                else if (first_frame == 0)
                {
                    Sprite inst = collider.instance_meeting(x, y + 2, this.client.colliders.collisions_movers);
                    if (inst != null)
                    {
                        if (constraint == null)
                        {
                            constraint = inst;
                            save_constraint_x = constraint.x;
                            save_constraint_y = constraint.y;
                        }
                    }
                    else
                    {
                        constraint = null;
                    }
                    first_frame = -1;
                }

                if (constraint == null)
                {
                    if (!collider.point_meeting(this.x - 1, this.y + this.height + 1, this.client.tiles) || collider.point_meeting(this.x - 1, this.y + 4, this.client.tiles)/* ||
                    !collider.point_meeting(this.x - 1, this.y + this.height + 1, this.client.colliders.collisions_movers)*/)
                    {
                        this.hsp = 1;
                    }
                    if (!collider.point_meeting(this.x + this.width + 1, this.y + this.height + 1, this.client.tiles) || collider.point_meeting(this.x + this.width + 1, this.y + 4, this.client.tiles)
                      /*|| collider.point_meeting(this.x + this.width + 1, this.y + this.height + 1, this.client.colliders.collisions_movers)*/)
                    {
                        this.hsp = -1;
                    }
                }
                else
                {
                    if (!collider.point_meeting(this.x - 1, this.y + this.height + 1, this.constraint))
                    {
                        this.hsp = 1;
                    }
                    if (!collider.point_meeting(this.x + this.width + 1, this.y + this.height + 1, this.constraint))
                    {
                        this.hsp = -1;
                    }
                }
                this.x += this.hsp;
            }
        }
    }
}