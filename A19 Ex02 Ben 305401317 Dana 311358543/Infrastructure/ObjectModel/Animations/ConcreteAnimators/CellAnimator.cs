//*** Guy Ronen � 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class CellAnimator : SpriteAnimator
    {
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrCellIdx = 0;
        private int m_StartingSquareIndex = 0;
        private readonly int r_NumOfCells = 1;
        private int m_Direction = 1;
        private bool isFliper = false;


        // CTORs
        public CellAnimator(TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength, int i_StartingSquareIndex, bool i_IsFlipper, int i_toggleDIrection)
            : base("CellAnimation", i_AnimationLength)
        {
            m_Direction = i_toggleDIrection;
            isFliper = i_IsFlipper;
            m_CurrCellIdx = i_StartingSquareIndex;
            this.m_StartingSquareIndex = i_StartingSquareIndex;
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            this.r_NumOfCells = i_NumOfCells;

            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }


        public TimeSpan CellTime
        {
            get { return m_CellTime; }
            set { m_CellTime = value; }
        }

        private void goToNextFrame()
        {

            if (isFliper)
            {
                m_CurrCellIdx += m_Direction;
                m_Direction *= -1;
            }
            else
            {
                m_CurrCellIdx++;

                if (m_CurrCellIdx >= r_NumOfCells)
                {
                    if (m_Loop)
                    {
                        m_CurrCellIdx = m_StartingSquareIndex;
                    }
                    else
                    {
                        m_CurrCellIdx = r_NumOfCells - 1; /// lets stop at the last frame
                        this.IsFinished = true;
                    }
                }
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    /// we have elapsed, so blink
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
                m_CurrCellIdx * this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Top,
                this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Height);
        }
    }
}
