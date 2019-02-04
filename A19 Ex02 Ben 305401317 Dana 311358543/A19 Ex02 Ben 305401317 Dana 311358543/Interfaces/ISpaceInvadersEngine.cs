using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public interface ISpaceInvadersEngine
    {
        List<Player> Players { get; set; }

        bool IsGameOver { get; set; }

        PlayerIndex? getWinner();

        void InitGameEngineForNewGame();

        void ChangeNumOfPlayers(GameScreen i_GameScreen);

        void InitGameEngineForNextLevel();

        void HandleHit(Wall i_wall, ICollidable i_Collidable);

        void HandleHit(Bullet i_Bullet, ICollidable i_Collidable);

        void HandleHit(SpaceShip i_SpaceShip, ICollidable i_Collidable);

        void HandleHit(Enemy i_Enemy, ICollidable i_Collidable);

        void HandleHit(MotherSpaceShip i_MotherSpaceShip, Bullet i_Bullet);

        void CreatePlayers(GameScreen i_GameScreen);
    }
}
