using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Infrastructure
{
    public class ShrinkerAnimator : SpriteAnimator
    {
        private TimeSpan m_ShrinkLength;

        public TimeSpan ShrinkLength
        {
            get { return m_ShrinkLength; }
            set { m_ShrinkLength = value; }
        }

        // CTORs
        public ShrinkerAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_ShrinkLength = i_AnimationLength;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            TimeSpan currSize = m_ShrinkLength;
            currSize -= i_GameTime.ElapsedGameTime;

            this.BoundSprite.Scales *= new Vector2((float)currSize.TotalSeconds / (float)m_ShrinkLength.TotalSeconds);
        }

        protected override void OnFinished()
        {
            base.OnFinished();
            this.BoundSprite.Visible = false;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Rotation += m_OriginalSpriteInfo.Rotation;
        }

    }
}
