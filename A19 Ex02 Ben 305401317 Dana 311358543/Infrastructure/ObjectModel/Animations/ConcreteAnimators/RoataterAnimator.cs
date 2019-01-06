using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Infrastructure
{
    public class RoataterAnimator : SpriteAnimator
    {
        private TimeSpan m_RotateLength;
        private int m_NumOfRoundsPerSec;

        public TimeSpan RotateLength
        {
            get { return m_RotateLength; }
            set { m_RotateLength = value; }
        }

        // CTORs
        public RoataterAnimator(int NumOfRoundsPerSec, TimeSpan i_AnimationLength)
            : base("Rotator", i_AnimationLength)
        {
            m_NumOfRoundsPerSec = NumOfRoundsPerSec;
            m_RotateLength = i_AnimationLength;
        }

        protected override void OnFinished()
        {
            base.OnFinished();
            this.BoundSprite.Visible = false;
        }


        protected override void DoFrame(GameTime i_GameTime)
        {
            float currentTime = (float)m_RotateLength.TotalSeconds;
            float rotationVelocity = m_NumOfRoundsPerSec * MathHelper.TwoPi;
            currentTime -= (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            this.BoundSprite.Rotation += rotationVelocity * currentTime;

        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = this.m_OriginalSpriteInfo.Scales;
        }

    }
}
