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

using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public interface ISpaceInvadersEngine
    {
        List<Player> Players { get; set; }

        void HandleHit(Wall i_wall, ICollidable i_Collidable);

        void HandleHit(Bullet i_Bullet, ICollidable i_Collidable);

        void HandleHit(SpaceShip i_SpaceShip, ICollidable i_Collidable);

        void HandleHit(Enemy i_Enemy, ICollidable i_Collidable);

        void HandleHit(MotherSpaceShip i_MotherSpaceShip, Bullet i_Bullet);

        void CreatePlayers(GameScreen i_GameScreen);

        void ShowGameOverMessage();
    }
}
