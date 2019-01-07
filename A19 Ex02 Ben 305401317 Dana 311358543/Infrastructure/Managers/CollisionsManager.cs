﻿//*** Guy Ronen © 2008-2011 ***//
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Infrastructure
{
    // TODO 10: Implement the collisions manager service:
    public class CollisionsManager : GameService, ICollisionsManager
    {
        protected readonly List<ICollidable> m_Collidables = new List<ICollidable>();

        public CollisionsManager(Game i_Game) :
            base(i_Game, int.MaxValue)
        { }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ICollisionsManager), this);
        }

        public void AddObjectToMonitor(ICollidable i_Collidable)
        {
            if (!this.m_Collidables.Contains(i_Collidable))
            {
                this.m_Collidables.Add(i_Collidable);
                i_Collidable.PositionChanged += collidable_Changed;
                i_Collidable.SizeChanged += collidable_Changed;
                i_Collidable.VisibleChanged += collidable_Changed;
                i_Collidable.Disposed += collidable_Disposed;
            }
        }

        private void collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;

            if (collidable != null
                &&
                this.m_Collidables.Contains(collidable))
            {
                collidable.PositionChanged -= collidable_Changed;
                collidable.SizeChanged -= collidable_Changed;
                collidable.VisibleChanged -= collidable_Changed;
                collidable.Disposed -= collidable_Disposed;

                m_Collidables.Remove(collidable);
            }
        }

        private void collidable_Changed(object sender, EventArgs e)
        {
            if (sender is ICollidable)
            {// to be on the safe side :)
                checkCollision(sender as ICollidable);
            }
        }

        private void checkCollision(ICollidable i_Source)
        {
            if (i_Source.Visible)
            {
                List<ICollidable> collidedComponents = new List<ICollidable>();

                // finding who collided with i_Source:
                foreach (ICollidable target in m_Collidables)
                {
                    if (i_Source != target && target.Visible)
                    {
                        if (target is IPixelsCollidable && target is IRectangleCollidable && i_Source is IRectangleCollidable)
                        {
                            IRectangleCollidable targetAsIRec = target as IRectangleCollidable;
                            if (targetAsIRec.CheckCollision(i_Source))
                            {
                                collidedComponents.Add(target);
                            }
                        }
                        else if (target is IPixelsCollidable && target is IRectangleCollidable && i_Source is IPixelsCollidable)
                        {
                            IPixelsCollidable targetAsIPix = target as IPixelsCollidable;
                            if (targetAsIPix.CheckCollision(i_Source))
                            {
                                collidedComponents.Add(target);
                            }
                        }
                        else
                        {
                            if (target.CheckCollision(i_Source))
                            {
                                collidedComponents.Add(target);
                            }
                        }
                    }
                }

                foreach (ICollidable target in collidedComponents)
                {
                    target.Collided(i_Source);
                    i_Source.Collided(target);
                }
            }
        }
    }
}
