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

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Gun
    {
        void Shoot(Bullet.eBulletType i_eBulletType,Vector2 i_ShooterOrigin, Game i_Game )
        {
            Bullet bullet = new Bullet(i_Game, i_eBulletType);
            bullet.Initialize();
            bullet.Position = new Vector2(i_ShooterOrigin.X , i_ShooterOrigin.Y - bullet.Height / 2);
        }
    }
}
