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
            : this("Fade", i_AnimationLength)
        {}

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Opacity -= (float)i_GameTime.ElapsedGameTime.TotalSeconds*(float)(this.BoundSprite.Opacity / this.AnimationLength.TotalSeconds);

        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
