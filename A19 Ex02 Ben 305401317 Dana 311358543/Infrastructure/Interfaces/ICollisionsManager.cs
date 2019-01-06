//*** Guy Ronen © 2008-2011 ***//
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
    public interface ICollidable2D : ICollidable
    {
        Rectangle Bounds { get; }
        Vector2 Velocity { get; }
    }

    public interface ICollidable3D : ICollidable
    {
        BoundingBox Bounds { get; }
        Vector3 Velocity { get; }
    }

    public interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
    public interface IPixelBasedCollidable : ICollidable2D
    {
        Texture2D Texture { get; set; }
    }
}
