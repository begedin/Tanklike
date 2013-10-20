using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanklike.ScreenManagement;

namespace Tanklike.Screens
{
    class GamePlayScreen : GameScreen
    {

        #region Initialization

        public GamePlayScreen()
            : base()
        {
            this.Exiting += new EventHandler(GameplayScreen_Exiting);
        }

        //TODO
        //private GamePlayScreen(object newGameObjectTODO);
        //private GamePlayScreen(object saveGameObjectTODO);

        /// <summary>
        /// Loads all the content for the gameplay, depending on if it's a new or existing game being loaded.
        /// </summary>
        public override void LoadContent()
        {
            Session.StartNewSession(/* newDataObject, */ScreenManager, this);

            //TODO: Start or load a session based on the constructor that was used
            if (true/*isNewGameFlag*/)
            {
                Session.StartNewSession(/* newDataObject, */ScreenManager, this);
            }
            else 
            {
                Session.LoadSession(/*loadDataObject, */ScreenManager, this);
            }
            base.LoadContent();

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game session, as long as this screen is in focus and should be updated
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive && !coveredByOtherScreen)
            {
                Session.Update(gameTime);
            }
        }

        /// <summary>
        /// Allows responding to player input.
        /// </summary>
        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.MainMenu))
            {
                ScreenManager.AddScreen(new MainMenuScreen());
                return;
            }

            if (InputManager.IsActionTriggered(InputManager.Action.ExitGame))
            {
                // add a confirmation message box
                const string message =
                    "Are you sure you want to exit?  All unsaved progress will be lost.";
                MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
                confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
                ScreenManager.AddScreen(confirmExitMessageBox);
                return;
            }
        }

        /// <summary>
        /// Draws the gameplay screen by calling the Session's Draw method.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Session.Draw(gameTime);
        }

        #endregion

        #region Events

        private void GameplayScreen_Exiting(object sender, EventArgs e)
        {
            Session.EndSession();
        }

        /// <summary>
        /// Event handler for when the user selects Yes 
        /// on the "Are you sure?" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        #endregion
    }
}
