 ///*** Guy Ronen � 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class CellAnimator : SpriteAnimator
    {
        private readonly int r_NumOfCells = 1;
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrCellIdx = 0;
        private int m_StartingSquareIndex = 0;
        private int m_Direction = 1;
        private int m_OriginalDirection = 1;
        private bool m_IsFliper = false;
        private bool m_IsIncreasingProgression = true;

        public bool IsIncreasingProgression
        {
            set { this.m_IsIncreasingProgression = value; }
        }

        // CTORs
        public CellAnimator(TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength, int i_StartingSquareIndex)
            : base("CellAnimation", i_AnimationLength)
        {
            this.m_CurrCellIdx = i_StartingSquareIndex;
            this.m_StartingSquareIndex = i_StartingSquareIndex;
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            this.r_NumOfCells = i_NumOfCells;
            this.m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        public CellAnimator(TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength, int i_StartingSquareIndex, bool i_IsFlipper, int i_toggleDIrection)
            : base("CellAnimation", i_AnimationLength)
        {
            this.m_OriginalDirection = i_toggleDIrection;
            this.m_Direction = i_toggleDIrection;
            this.m_IsFliper = i_IsFlipper;
            this.m_CurrCellIdx = i_StartingSquareIndex;
            this.m_StartingSquareIndex = i_StartingSquareIndex;
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            this.r_NumOfCells = i_NumOfCells;
            this.m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        public TimeSpan CellTime
        {
            get { return this.m_CellTime; }
            set { this.m_CellTime = value; }
        }

        private void goToNextFrame()
        {
            if (this.m_IsFliper)
            {
                this.m_CurrCellIdx += this.m_Direction;
                this.m_Direction *= -1;
            }
            else
            {
                if (this.m_IsIncreasingProgression)
                {
                    this.m_CurrCellIdx++;
                }
                else
                {
                    this.m_CurrCellIdx--;
                }

                if (this.m_CurrCellIdx >= this.r_NumOfCells)
                {
                    if (this.m_Loop)
                    {
                        this.m_CurrCellIdx = this.m_StartingSquareIndex;
                    }
                    else
                    {
                        this.m_CurrCellIdx = this.r_NumOfCells - 1; /// lets stop at the last frame
                        this.IsFinished = true;
                    }
                }
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = this.m_OriginalSpriteInfo.SourceRectangle;
            this.m_TimeLeftForCell = TimeSpan.Zero;
            this.m_Direction = this.m_OriginalDirection;
            this.m_CurrCellIdx = this.m_StartingSquareIndex;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (this.m_CellTime != TimeSpan.Zero)
            {
                this.m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (this.m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    /// we have elapsed, so blink
                    this.goToNextFrame();
                    this.m_TimeLeftForCell = this.m_CellTime;
                }
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
                                                            this.m_CurrCellIdx * this.BoundSprite.SourceRectangle.Width,
                                                            this.BoundSprite.SourceRectangle.Top,
                                                            this.BoundSprite.SourceRectangle.Width,
                                                            this.BoundSprite.SourceRectangle.Height);
        }
    }
}
