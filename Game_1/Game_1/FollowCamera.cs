using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_1
{
    internal class FollowCamera
    {
        public Vector2 Position { get; set; }

        public FollowCamera(Vector2 position)
        {
            Position = position;
        }

        public void Follow(Rectangle target, Vector2 screenSize) {
            
            Position = new Vector2(
                -target.X + (screenSize.X /2),
                -target.Y + (screenSize.Y /2)
            );
        }

    }
}
