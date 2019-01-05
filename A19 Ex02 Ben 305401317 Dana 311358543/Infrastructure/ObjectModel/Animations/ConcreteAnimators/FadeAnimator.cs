using System;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class FadeAnimator : SpriteAnimator
    {
        public FadeAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {}
        

        public FadeAnimator(TimeSpan i_AnimationLength)
            : this("Blink", i_AnimationLength)
        {}

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Opacity -=(float)(this.m_OriginalSpriteInfo.Opacity / this.AnimationLength.Seconds);

        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
