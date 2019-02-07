using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public static class SpaceInvadersConfig
    {
        public enum eLevel
        {
            One,
            Two,
            tree,
            Four,
            Five,
            Six
        }

        public enum eScoreValue
        {
            MotherShip = 850,
            Soul = -1100,
            PinkEnemy = 260,
            BlueEnemy = 140,
            YellowEnemy = 110
        }

        public enum eNumOfPlayers
        {
            OnePlayer = 1,
            TwoPlayers = 2
        }

        public static Vector2 k_DefaultWindowSize = new Vector2(800f, 600f);
        public const int k_NumOfWalls = 4;
        public const int k_NumOfEnemiesRows = 5;
        public const int k_NumOfEnemiesColumns = 9;
        public static eNumOfPlayers s_NumOfPlayers = eNumOfPlayers.OnePlayer;

        public const double k_sizeOfBulletHitEffect = 0.7;

        // Configurations for level up
        public static eLevel s_LogicLevel = eLevel.One;
        public static int m_Level = 0;
        public const float k_WallsVelocitiyAdditionPercent = (float)(-0.07);
        public const int k_EnemyScoreAddition = 120;
        public const int k_EnemyShootingFrequencyAddition = 5;
    }
}
