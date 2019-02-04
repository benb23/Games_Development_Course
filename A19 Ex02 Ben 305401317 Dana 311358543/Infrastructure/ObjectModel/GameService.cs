using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public abstract class GameService : RegisteredComponent
    {
        public GameService(Game i_Game, int i_UpdateOrder)
          : base(i_Game, i_UpdateOrder)
        {
            this.RegisterAsService(); 
        }

        public GameService(Game i_Game)
            : base(i_Game)
        {
            this.RegisterAsService();
        }

        protected virtual void RegisterAsService()
        {
            this.Game.Services.AddService(this.GetType(), this);
        }
    }
}
