using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
    public abstract class Sprite : DrawableGameComponent
    {
        protected SpriteBatch m_SpriteBatch;
        protected Texture2D m_Texture;
        protected Vector2 m_Position;
        protected Color m_Tint;
        protected string m_AssetName;
        
        public Sprite(Game i_Game) : base(i_Game)
        {
        }

        public SpriteBatch SpriteBatch
        {
            get { return this.m_SpriteBatch; }
            set { this.m_SpriteBatch = value; }
        }

        public Texture2D Texture
        {
            get { return this.m_Texture; }
            set { this.m_Texture = value; }
        }

        public Color Tint
        {
            get { return this.m_Tint; }
            set { this.m_Tint = value; }
        }

        public Vector2 Position
        {
            get { return this.m_Position; }
            set { this.m_Position = value; }
        }

        public string AssetName
        {
            get { return this.m_AssetName; }
            set { this.m_AssetName = value; }
        }

        protected override void LoadContent()
        {
            this.m_SpriteBatch = SpaceInvaders.s_GameUtils.SpriteBatch;
            this.Texture = this.Game.Content.Load<Texture2D>(this.m_AssetName);
            this.InitPosition();
            base.LoadContent();
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (this.Visible)
            {
                this.SpriteBatch.Draw(this.Texture, this.Position, this.Tint);
            }
           
            base.Draw(i_GameTime);
        }

        public virtual void AddComponent()
        {
            Game.Components.Add(this);
        }

        public virtual void RemoveComponent()
        {
            this.Visible = false;
            Game.Components.Remove(this);
        }

        public abstract void InitPosition();
    }
}
