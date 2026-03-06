using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    public class CollisionGroups
    {
        public List<Sprite> collisions_damage = new List<Sprite>();
        public List<Sprite> collisions_player_damage = new List<Sprite>();
        public List<Sprite> collisions_obstacles = new List<Sprite>();
        public List<Sprite> collisions_movers = new List<Sprite>();
        public CollisionGroups() { }
    }
}