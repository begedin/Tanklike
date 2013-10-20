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
        private Texture2D _textureBackground, _textureButton, _textureButtonPushed;
        private Vector2 _positionBackground;

        private MenuEntry _menuNewGame, _menuExitGame;

        #region Initialization

        public MainMenuScreen()
            : base()
        {
            _menuNewGame = new MenuEntry("New game");
            _menuNewGame.Font = Fonts.MenuItemFont;
            _menuNewGame.Selected += m_menuNewGame_Selected;
            MenuEntries.Add(_menuNewGame);

            _menuExitGame = new MenuEntry("Quit");
            _menuExitGame.BindingRectangle = new Rectangle(20, 100, 64, 64);
            _menuExitGame.Font = Fonts.MenuItemFont;
            _menuExitGame.Selected += OnCancel;
            MenuEntries.Add(_menuExitGame);

            AudioManager.PushMusic("MainTheme");
        }

        public override void LoadContent()
        {
            // load the textures
            ContentManager content = ScreenManager.Game.Content;
            _textureBackground = content.Load<Texture2D>(@"Textures\logo");
            _textureButton = content.Load<Texture2D>(@"Textures\button");
            _textureButtonPushed = content.Load<Texture2D>(@"Textures\button_pushed");

            //set menu entry textures
            _menuNewGame.TextureNormal = _textureButton;
            _menuNewGame.TexturePushed = _textureButtonPushed;
            _menuExitGame.TextureNormal = _textureButton;
            _menuExitGame.TexturePushed = _textureButtonPushed;

            // calculate the texture positions
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _positionBackground = new Vector2((viewport.Width - _textureBackground.Width) / 2, (viewport.Height - _textureBackground.Height) / 2);

            _menuNewGame.BindingRectangle = new Rectangle(viewport.Width / 10, viewport.Height / 2 - 128, 256, 256);
            _menuExitGame.BindingRectangle = new Rectangle(viewport.Width - (viewport.Width / 10) - 256, viewport.Height / 2 - 128, 256, 256);
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

            ScreenManager.AddScreen(new GamePlayScreen());

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
            // add a confirmation message box
            string message = String.Empty;
            if (Session.IsActive)
            {
                message =
                    "Are you sure you want to exit?  All unsaved progress will be lost.";
            }
            else
            {
                message = "Are you sure you want to exit?";
            }
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
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

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            // draw the background images
            spriteBatch.Draw(_textureBackground, _positionBackground, Color.White);
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
