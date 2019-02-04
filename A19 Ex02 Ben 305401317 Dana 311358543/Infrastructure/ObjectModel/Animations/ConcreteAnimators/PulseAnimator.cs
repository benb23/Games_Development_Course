////*** Guy Ronen © 2008-2011 ***//
using System;
using Infrastructure;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class PulseAnimator : SpriteAnimator
    {
        protected float m_Scale;

        public float Scale
        {
            get { return this.m_Scale; }
            set { this.m_Scale = value; }
        }

        protected float m_PulsePerSecond;

        public float PulsePerSecond
        {
            get { return this.m_PulsePerSecond; }
            set { this.m_PulsePerSecond = value; }
        }

        private bool m_Shrinking;
        private float m_TargetScale;
        private float m_SourceScale;
        private float m_DeltaScale;

        public PulseAnimator(string i_Name, TimeSpan i_AnimationLength, float i_TargetScale, float i_PulsePerSecond)
            : base(i_Name, i_AnimationLength)
        {
            this.m_Scale = i_TargetScale;
            this.m_PulsePerSecond = i_PulsePerSecond;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = this.m_OriginalSpriteInfo.Scales;

            this.m_SourceScale = this.m_OriginalSpriteInfo.Scales.X;
            this.m_TargetScale = this.m_Scale;
            this.m_DeltaScale = this.m_TargetScale - this.m_SourceScale;
            this.m_Shrinking = this.m_DeltaScale < 0;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (this.m_Shrinking)
            {
                if (this.BoundSprite.Scales.X > this.m_TargetScale)
                {
                    this.BoundSprite.Scales -= new Vector2(totalSeconds * 2 * this.m_PulsePerSecond * this.m_DeltaScale);
                }
                else
                {
                    this.BoundSprite.Scales = new Vector2(this.m_TargetScale);
                    this.m_Shrinking = false;
                    this.m_TargetScale = this.m_SourceScale;
                    this.m_SourceScale = this.BoundSprite.Scales.X;
                }
            }
            else
            {
                if (this.BoundSprite.Scales.X < this.m_TargetScale)
                {
                    this.BoundSprite.Scales += new Vector2(totalSeconds * 2 * this.m_PulsePerSecond * this.m_DeltaScale);
                }
                else
                {
                    this.BoundSprite.Scales = new Vector2(this.m_TargetScale);
                    this.m_Shrinking = true;
                    this.m_TargetScale = this.m_SourceScale;
                    this.m_SourceScale = this.BoundSprite.Scales.X;
                }
            }
        }
    }
}
