 ///*** Guy Ronen © 2008-2011 ***//
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }

    public interface IPixelsCollidable : ICollidable
    {
        Color[] Pixels { get; }

        Vector2 Position { get; set; }  
    }

    public interface IRectangleCollidable : ICollidable
    {       
    }
}
