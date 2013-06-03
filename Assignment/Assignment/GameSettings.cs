using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment
{
    class GameSettings
    {
        public const int JUMP_TIME = 300; // miliseconds
        public const float PLAYER_SPEED_X = 0.0005f; // radians per frame
        public const int PLAYER_SPEED_Y = 4; // pixels per frame

        public const float TERRAIN_HEIGHT = 0.65f; // % of screen height 

        public const float EARTH_ROTATION_SPEED = 0.02f; // radians per frame
    }
}
