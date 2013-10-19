using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tanklike.ScreenManagement
{
    /// <summary>
    /// A base class for all game screens. When the game is drawing itself, it actually draws screens from the tap of the 
    /// screen stack, going towards the bottom, until it reaches a screen which is an opaque full screen.
    /// 
    /// Each screen has all basic game logic (draw, update, etc.)
    /// </summary>
    public abstract class GameScreen
    {
        #region Private members

        private bool m_bIsPopup = false;

        private TimeSpan m_tsTransitionOnTime = TimeSpan.Zero;
        private TimeSpan m_tsTransitionOffTime = TimeSpan.Zero;
        private float m_fTransitionProgress = 1;

        private ScreenState m_eScreenState = ScreenState.TransitionOn;

        private bool m_bIsExiting = false;

        private bool m_bOtherScreenHasFocus;

        private ScreenManager m_ScreenManager;

        #endregion

        #region Properties

        /// <summary>
        /// If this property is true, the current screen is a popup screen and we should also draw any screen below it.
        /// </summary>
        public bool IsPopup
        {
            get { return m_bIsPopup; }
            protected set { m_bIsPopup = value; }
        }

        /// <summary>
        /// The time it takes for this screen to transition into appearance.
        /// </summary>
        public TimeSpan TransitionOnTime 
        {
            get { return m_tsTransitionOnTime; }
            protected set { m_tsTransitionOnTime = value; } 
        }

        /// <summary>
        /// The time it takes for this screen to transition out of appearance.
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return m_tsTransitionOffTime; }
            protected set { m_tsTransitionOffTime = value; }
        }

        /// <summary>
        /// Current progress of the transition, ranging from 0 to 1
        /// </summary>
        public float TransitionProgress
        {
            get { return m_fTransitionProgress; }
            protected set { m_fTransitionProgress = value; }
        }

        /// <summary>
        /// Appropriate alpha value for the current transition progress. Used to draw the screen in a transparent state while it's transitioning.
        /// </summary>
        public byte TransitionAlpha
        {
            get { return (byte)(255 - TransitionProgress * 255); }
        }

        /// <summary>
        /// The current state of the screen. It's either active, inactive, transitioning on or transitioning off.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return m_eScreenState; }
            protected set { m_eScreenState = value; }
        }

        /// <summary>
        /// The screen might be transitioning off to be temporarily replaced by another screen, or it might be going away for good.
        /// Indicates if the screen is going away for good.
        /// </summary>
        public bool IsExiting
        {
            get { return m_bIsExiting; }
            set 
            { 
                bool fireEvent = !m_bIsExiting && value;
                m_bIsExiting = value;
                if (fireEvent && (Exiting != null))
                {
                    //fire an exiting event
                    Exiting(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Indicates if this screen is active and is able to respond to user input.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !m_bOtherScreenHasFocus && (m_eScreenState == ScreenState.TransitionOn || m_eScreenState == ScreenState.Active);
            }
        }

        /// <summary>
        /// The screen manager this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return m_ScreenManager; }
            internal set { m_ScreenManager = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Fired when the screen is exiting for good.
        /// </summary>
        public event EventHandler Exiting;

        #endregion

        #region Initalization

        /// <summary>
        /// Load graphic content for the screen
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Unload graphic content for the screen
        /// </summary>
        public virtual void UnloadContent() { }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Runs the screens logic, such as transition updates. 
        /// Unlike HandleInput, this method is always called as long as the screen exists in the stack.
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.m_bOtherScreenHasFocus = otherScreenHasFocus;

            if (IsExiting)
            {
                // If the screen is going away to die, it should transition off.
                m_eScreenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, m_tsTransitionOffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTransition(gameTime, m_tsTransitionOffTime, 1))
                {
                    // Still busy transitioning.
                    m_eScreenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    m_eScreenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTransition(gameTime, m_tsTransitionOnTime, -1))
                {
                    // Still busy transitioning.
                    m_eScreenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    m_eScreenState = ScreenState.Active;
                }
            }
        }

        /// <summary>
        /// Updates transition progress based on elapsed time.
        /// </summary>
        /// <param name="time">Current gameTime</param>
        /// <param name="duration">Duration of the transition (off or on)</param>
        /// <param name="direction">Direction of the transition. 1 for off, -1 for on.</param>
        /// <returns>True if transition should continue, false if it's over and the end state (Hidden, Active) is reached.</returns>
        bool UpdateTransition(GameTime time, TimeSpan duration, int direction)
        {
            float transitionDelta;

            if (duration == TimeSpan.Zero)
            {
                transitionDelta = 1;
            }
            else
            {
                transitionDelta = (float)(time.ElapsedGameTime.TotalMilliseconds / duration.TotalMilliseconds);
            }

            m_fTransitionProgress += transitionDelta * direction;

            // Did we reach the end of the transition?
            if ((m_fTransitionProgress <= 0) || (m_fTransitionProgress >= 1))
            {
                m_fTransitionProgress = MathHelper.Clamp(m_fTransitionProgress, 0, 1);
                return false;
            }

            //otherwise, transition should continue
            return true;
        }

        /// <summary>
        /// Allows the screen to respond to input. Unlike Update, this is called only when the screen is active.
        /// </summary>
        public virtual void HandleInput() { }

        /// <summary>
        /// Called when the screen should draw itself.
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }

        /// <summary>
        /// Tells the screen to go away. ScreenManager.RemoveScreen instantly removes it, but this method respects transition times.
        /// </summary>
        public void ExitScreen()
        {
            IsExiting = true;

            // If the screen has a zero transition time, remove it immediately.
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
            }
        }

        #endregion
    }
}
