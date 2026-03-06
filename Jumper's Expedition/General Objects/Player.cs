using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    public class Player : Sprite
    {
        float hsp = 0;
        float vsp = 0;
        float target_horizontal = 0;
        new float speed = 4;
        float jump_force = 8;
        float previous_x = 0;
        float previous_y = 0;

        public float currency = 0;
        public float health = 10;

        int jump_balance = 2;

        Form1 client;

        Collider collider;

        Alarm i_frames;
        bool can_get_hit = true;
        public Player(Form1 client)
        {
            this.x = 128;
            this.y = 1;
            this.y_offset = -8;
            this.x_offset = -2;
            this.collider = new Collider(this);
            this.client = client;
            set_sprite(@"..\..\sprites\jumpy.png",12,8);
            this.client.sprites["obj_player"] = this;

            this.i_frames = new Alarm(() =>
            {
                this.can_get_hit = true;
            });
        }

        public override void Update()
        {
            if (!this.client.paused)
            {
                this.i_frames.Update();
                if(this.vsp < 5)
                {
                    this.vsp += 0.8f;
                }

                Sprite inst = collider.instance_meeting(x, y + vsp, this.client.colliders.collisions_movers);
                if (inst != null)
                {
                    if(constraint == null)
                    {
                        constraint = inst;
                        save_constraint_x = constraint.x;
                        save_constraint_y = constraint.y;
                    }
                    this.vsp = 0;
                }
                else
                {
                    constraint = null;
                }

                this.target_horizontal = (Convert.ToSingle(this.client.keyManager.possible_keys[1].pressed) - Convert.ToSingle(this.client.keyManager.possible_keys[0].pressed)) * this.speed;
                this.hsp = GameMath.lerp(this.hsp, this.target_horizontal, 0.25f);

                if (this.collider.place_meeting(this.x, this.y + 2, this.client.tiles) || this.collider.place_meeting(this.x, this.y + 2, this.client.colliders.collisions_movers))
                {
                    this.jump_balance = 2;
                }

                if (this.jump_balance > 0 && this.client.keyManager.possible_keys[5].pressed_down)
                {
                    this.vsp = -jump_force;
                    this.jump_balance--;
                }

                Enemy enemy_collider = (Enemy)this.collider.instance_meeting(this.x, this.y, this.client.colliders.collisions_damage);
                if (this.can_get_hit && enemy_collider != null)
                {
                    this.health -= enemy_collider.damage;
                    this.can_get_hit = false;
                    this.i_frames.Start(60);
                    this.hsp *= -1;
                    this.vsp *= -1;
                }

                if(!can_get_hit)
                {
                    this.image_alpha = 0.5f + (float)Math.Abs((Math.Sin(this.client.curr_frame*0.5d)*0.5d));
                }
                else
                {
                    this.image_alpha = 1;
                }

                if (this.collider.place_meeting(this.x, this.y + this.vsp, this.client.tiles) || this.y + this.vsp > 216)
                {
                    while (!(this.collider.place_meeting(this.x, this.y + Math.Sign(this.vsp), this.client.tiles) || this.y + Math.Sign(this.vsp) > 216))
                    {
                        this.y += Math.Sign(this.vsp);
                    }
                    this.vsp = 0;
                }
                if (this.collider.place_meeting(this.x + this.hsp, this.y, this.client.tiles) || this.x + this.hsp > 384 || this.x + this.hsp < 0)
                {
                    while (!(this.collider.place_meeting(this.x + Math.Sign(this.hsp), this.y, this.client.tiles) || this.x + Math.Sign(this.hsp) > 384 || this.x + Math.Sign(this.hsp) < 0))
                    {
                        this.x += Math.Sign(this.hsp);
                    }
                    this.hsp = 0;
                }

                this.x += this.hsp;
                this.y += this.vsp;

                this.previous_x = this.x;
                this.previous_y = this.y;

                float loop_magnitude = 1;
                float iterator = 0;
                while (this.collider.place_meeting(this.x, this.y, this.client.tiles) || this.collider.place_meeting(this.x, this.y, this.client.colliders.collisions_movers))
                {
                    this.hsp = 0;
                    this.vsp = 0;
                    double loop_angle = (Math.PI/2)*iterator;
                    this.x = this.previous_x + ((float)Math.Cos(loop_angle)*loop_magnitude);
                    this.y = this.previous_y + ((float)Math.Sin(loop_angle)*loop_magnitude);
                    if(iterator >= 3)
                    {
                        iterator = 0;
                    }
                    else
                    {
                        iterator++;
                    }
                    loop_magnitude++;
                }
            }
        }
    }
}