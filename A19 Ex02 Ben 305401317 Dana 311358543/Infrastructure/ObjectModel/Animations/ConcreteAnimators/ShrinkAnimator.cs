using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Infrastructure
{
    public class ShrinkAnimator : SpriteAnimator
    {
        private TimeSpan m_ShrinkLength;

        public TimeSpan ShrinkLength
        {
            get { return m_ShrinkLength; }
            set { m_ShrinkLength = value; }
        }

        // CTORs
        public ShrinkAnimator(TimeSpan i_AnimationLength)
            : base("Shrinker", i_AnimationLength)
        {
            m_ShrinkLength = i_AnimationLength;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            TimeSpan currSize = m_ShrinkLength;
            currSize -= i_GameTime.ElapsedGameTime;

            this.BoundSprite.Scales *= new Vector2((float)currSize.TotalSeconds / (float)m_ShrinkLength.TotalSeconds);

        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Rotation = m_OriginalSpriteInfo.Rotation;
        }

    }
}
