using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Infrastructure
{
    public class RectangleCollidableSprite : Sprite
    {
        public RectangleCollidableSprite(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game)
        { }

        public virtual bool CheckCollision(ICollidable i_Source)
        {
            {
                bool collided = false;
                IPixelsCollidable source = i_Source as IPixelsCollidable;
                if (source != null)
                {
                    collided = source.Bounds.Intersects(this.Bounds);
                }

                if (collided)
                {
                    collided = true;
                }

                return collided;
            }
        }
    }
}
