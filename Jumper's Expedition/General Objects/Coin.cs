using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition.General_Objects
{
    internal class Coin : Sprite
    {
        Collider collision;
        Form1 client;
        public Coin(Form1 client, float x, float y)
        {
            this.x = x;
            this.y = y;

            this.client = client;
            collision = new Collider(this);
            sprite_init_index(this.client, "coin");
            set_sprite(@"..\..\sprites\coin.png", 12, 12);
        }
        public override void Update()
        {
            if (collision.place_meeting(x, y, this.client.sprites["obj_player"]))
            {
                Player player_inst = (Player)client.sprites["obj_player"];
                player_inst.currency++;
                instance_destroy(client,name);
            }
        }
    }
}