using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Infrastructure
{
    public class RegisteredComponent : GameComponent
    {
        public RegisteredComponent(Game i_Game, int i_UpdateOrder)
        : base(i_Game)
        {
            this.UpdateOrder = i_UpdateOrder;
            Game.Components.Add(this); // self-register as a coponent
        }

        public RegisteredComponent(Game i_Game)
            : this(i_Game, int.MaxValue)
        { }
    }
}
