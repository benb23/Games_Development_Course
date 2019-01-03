//*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;

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
}
