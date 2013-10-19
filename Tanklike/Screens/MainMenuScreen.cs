using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanklike.ScreenManagement;

namespace Tanklike.Screens
{
    class MainMenuScreen : MenuScreen
    {
        private Texture2D m_BackgroundTexture;
        private Vector2 m_BackgroundPosition;

        private MenuEntry m_menuNewGame, m_menuExitGame;

        #region Initialization

        public MainMenuScreen()
            : base()
        {
            m_menuNewGame = new MenuEntry("New game");
            m_menuNewGame.BindingRectangle = new Rectangle(20, 20, 50, 50);
            m_menuNewGame.Font = Fonts.MenuItemFont;
            m_menuNewGame.Selected += m_menuNewGame_Selected;
            MenuEntries.Add(m_menuNewGame);

            m_menuExitGame = new MenuEntry("Quit");
            m_menuExitGame.BindingRectangle = new Rectangle(20, 100, 50, 50);
            m_menuExitGame.Font = Fonts.MenuItemFont;
            m_menuExitGame.Selected += OnCancel;
            MenuEntries.Add(m_menuExitGame);

            AudioManager.PushMusic("MainTheme");
        }

        public override void LoadContent()
        {
            // load the textures
            ContentManager content = ScreenManager.Game.Content;
            m_BackgroundTexture = content.Load<Texture2D>(@"Textures\logo");

            // calculate the texture positions
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            m_BackgroundPosition = new Vector2(300, 300);
        }

        #endregion

        #region Updating

        /// <summary>
        /// Handles user input.
        /// </summary>
        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.Back) &&
                            Session.IsActive)
            {
                AudioManager.PopMusic();
                ExitScreen();
                return;
            }

            base.HandleInput();
        }

        

        private void m_menuNewGame_Selected(object sender, EventArgs e)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }

            //ContentManager content = ScreenManager.Game.Content;
            //LoadingScreen.Load(ScreenManager, true, new GameplayScreen(
            //    content.Load<GameStartDescription>("MainGameDescription")));
        }

        /// <summary>
        /// When the user cancels the main menu,
        /// or when the Exit Game menu entry is selected.
        /// </summary>
        protected override void OnCancel()
        {
            //// add a confirmation message box
            //string message = String.Empty;
            //if (Session.IsActive)
            //{
            //    message =
            //        "Are you sure you want to exit?  All unsaved progress will be lost.";
            //}
            //else
            //{
            //    message = "Are you sure you want to exit?";
            //}
            //MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            //confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            //ScreenManager.AddScreen(confirmExitMessageBox);

            ScreenManager.Game.Exit();
        }

        #endregion

        #region Drawing


        /// <summary>
        /// Draw this screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // draw the background images
            spriteBatch.Draw(m_BackgroundTexture, m_BackgroundPosition, Color.White);
            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];
                bool isSelected = IsActive && (i == m_nSelectedEntryIndex);
                menuEntry.Draw(this, isSelected, gameTime);
            }

            spriteBatch.End();
        }

        #endregion
    }
}
