using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper_s_Expedition
{
    internal static class GameMath
    {
        public static float lerp(float x, float y, float degree)
        {
            return (x + degree * (y - x));
        }

        public static float point_direction(float x1, float x2, float y1, float y2)
        {
            return ((float)Math.Atan2(y2 - y1, x2 - x1));
        }

        public static float rad_to_deg(float rad)
        {
            return (rad * (180f / (float)Math.PI));
        }
        public static float deg_to_rad(float deg)
        {
            return (deg * ((float)Math.PI / 180f));
        }
    }
}