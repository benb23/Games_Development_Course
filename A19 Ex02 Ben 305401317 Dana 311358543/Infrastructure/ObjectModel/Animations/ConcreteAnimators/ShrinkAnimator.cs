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
            get { return this.m_ShrinkLength; }
            set { this.m_ShrinkLength = value; }
        }

        public ShrinkAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
        }

        public ShrinkAnimator(TimeSpan i_AnimationLength)
            : base("Shrinker", i_AnimationLength)
        {
            this.m_ShrinkLength = i_AnimationLength;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            TimeSpan currSize = this.m_ShrinkLength;
            currSize -= i_GameTime.ElapsedGameTime;

            this.BoundSprite.Scales *= new Vector2((float)currSize.TotalSeconds / (float)this.m_ShrinkLength.TotalSeconds);
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = m_OriginalSpriteInfo.Scales;
        }
    }
}
