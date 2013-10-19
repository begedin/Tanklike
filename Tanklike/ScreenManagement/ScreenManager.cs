using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanklike.ScreenManagement
{
    /// <summary>
    /// Component which manages one or more screen instances. It uses a stack based approach,
    /// handling screens from top of the stack to bottom until it reaches the first screen that isn't a popup.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        List<GameScreen> m_lScreens = new List<GameScreen>();
        List<GameScreen> m_lScreensToUpdate = new List<GameScreen>();

        bool m_bIsInitialized;
        bool m_bTraceEnabled;

        SpriteBatch m_SpriteBatch;

        #endregion

        #region Properties

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
        }

        /// <summary>
        /// Used for debugging, to see if we should output a list of screens while drawing.
        /// </summary>
        public bool TraceEnabled
        {
            get { return m_bTraceEnabled; }
            set { m_bTraceEnabled = value; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(Game game) : base(game)
        {
        }

        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            m_bIsInitialized = true;
        }

        /// <summary>
        /// Loads all screens in the stack (active or inactive)
        /// </summary>
        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (var screen in m_lScreens)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Unloads all screens in the stack(active or inactive)
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (var screen in m_lScreens)
            {
                screen.UnloadContent();
            }
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            //this is used to make a copy of the screens while updating them
            m_lScreensToUpdate.Clear();

            foreach (var screen in m_lScreens)
            {
                m_lScreensToUpdate.Add(screen);
            }

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (m_lScreensToUpdate.Count > 0)
            {
                //"Pop" the top screen
                var screen = m_lScreensToUpdate[m_lScreensToUpdate.Count - 1];
                m_lScreensToUpdate.RemoveAt(m_lScreensToUpdate.Count - 1);

                //Update the screen
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                //Give the screen a chance to handle input, if it should.
                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first screen, its input is handled. All subsequent screens should know that they aren't in focus.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput();

                        otherScreenHasFocus = true;
                    }

                    //If this screen is not a popup, all subsequent screens should know this.
                    if (!screen.IsPopup)
                    {
                        coveredByOtherScreen = true;
                    }
                }

                if (m_bTraceEnabled)
                {
                    TraceScreens();
                }
            } 
        }

        /// <summary>
        /// Output screen names to Trace
        /// </summary>
        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (var screen in m_lScreens)
            {
                screenNames.Add(screen.GetType().Name);
            }

#if WINDOWS
            Trace.WriteLine(string.Join(", ", screenNames.ToArray()));
#endif
        }

        /// <summary>
        /// Tells each screen to draw itself
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (var screen in m_lScreens)
            {
                // only active and transitioning screens should be drawn
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a screen to the ScreenManager's stack.
        /// </summary>
        /// <param name="screen">Screen to add.</param>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            if (m_bIsInitialized)
            {
                screen.LoadContent();
            }

            m_lScreens.Add(screen);
        }

        /// <summary>
        /// Remove a screen from the ScreenManager's stack. Normally, GameScreen.ExitScreen() should be used instead, to allow transitions.
        /// </summary>
        /// <param name="screen">Screen to remove.</param>
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to load content.
            if (m_bIsInitialized)
            {
                screen.UnloadContent();
            }

            m_lScreens.Remove(screen);
            m_lScreensToUpdate.Remove(screen);
        }

        /// <summary>
        /// Get all screens as an array. A copy is returned, because screens should always be added or removed by the Add and RemoveScreen methods.
        /// </summary>
        /// <returns>Array of screens.</returns>
        public GameScreen[] GetScreens()
        {
            return m_lScreens.ToArray();
        }

        #endregion
    }
}
