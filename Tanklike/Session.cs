using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanklike.ScreenManagement;

namespace Tanklike
{
    class Session
    {
        #region Singleton


        /// <summary>
        /// The single Session instance that can be active at a time.
        /// </summary>
        private static Session singleton;

        #endregion

        #region User Interface Data

        /// <summary>
        /// The ScreenManager used to manage all UI in the game.
        /// </summary>
        private ScreenManager screenManager;

        /// <summary>
        /// The ScreenManager used to manage all UI in the game.
        /// </summary>
        public static ScreenManager ScreenManager
        {
            get { return (singleton == null ? null : singleton.screenManager); }
        }

        #endregion

        #region State Data

        /// <summary>
        /// Returns true if there is an active session.
        /// </summary>
        public static bool IsActive
        {
            get { return singleton != null; }
        }

        #endregion

        #region Initialization


        /// <summary>
        /// Private constructor of a Session object.
        /// </summary>
        /// <remarks>
        /// The lack of public constructors forces the singleton model.
        /// TODO: Implement gameplay screen
        /// </remarks>
        private Session(ScreenManager screenManager)
        {
            // check the parameter
            if (screenManager == null)
            {
                throw new ArgumentNullException("screenManager");
            }
            //if (gameplayScreen == null)
            //{
            //    throw new ArgumentNullException("gameplayScreen");
            //}

            // assign the parameter
            this.screenManager = screenManager;
        }

        #endregion

        #region Updating

        /// <summary>
        /// Update the session for this frame.
        /// </summary>
        /// <remarks>This should only be called if there are no menus in use.</remarks>
        public static void Update(GameTime gameTime)
        {
            // check the singleton
            if (singleton == null)
            {
                return;
            }
        }

        #endregion

        #region Drawing


        /// <summary>
        /// Draws the session environment to the screen
        /// </summary>
        public static void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = singleton.screenManager.SpriteBatch;

            spriteBatch.GraphicsDevice.Clear(Color.Red);
        }

        #endregion
    }
}
