//*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    // -- end of TODO 06

    // TODO 07: Define the 2D specific interface for 2D collidable objects:
    public interface ICollidable2D : ICollidable
    {
        Rectangle Bounds { get; }
        Vector2 Velocity { get; }
    }
    // -- end of TODO 07

    // TODO 08: Define the 3D specific interface for 3D collidable objects:
    public interface ICollidable3D : ICollidable
    {
        BoundingBox Bounds { get; }
        Vector3 Velocity { get; }
    }
    // -- end of TODO 08

    // TODO 09: Define the collisions manager service interface:
    public interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
    // -- end of TODO 09
}
