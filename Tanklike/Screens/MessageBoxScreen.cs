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
    /// <summary>
    /// A popup message box screen, used to display "are you sure" ok/cancel messages.
    /// </summary>
    class MessageBoxScreen : GameScreen
    {
        #region Fields

        private string _message;

        private Texture2D _textureBackground;

        private Texture2D _textureButton;
        private Texture2D _textureButtonSelected;

        private Rectangle _boundingRectangle;

        private MenuEntry _menuYes, _menuNo;
        private bool _cancelSelected;

        #endregion

        #region Initialization

        public MessageBoxScreen(string message)
        {
            _message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            _menuYes = new MenuEntry("Yes");
            _menuYes.Font = Fonts.MenuItemFont;
            _menuYes.Selected += OnAccepted;

            _menuNo = new MenuEntry("No");
            _menuNo.Font = Fonts.MenuItemFont;
            _menuNo.Selected += OnCancelled;
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            _textureBackground = content.Load<Texture2D>(@"Textures\messagebox_background");
            _textureButton = content.Load<Texture2D>(@"Textures\button_messagebox");
            _textureButtonSelected = content.Load<Texture2D>(@"Textures\button_messagebox_pushed");
            _menuNo.TextureNormal = _textureButton;
            _menuYes.TextureNormal = _textureButton;
            _menuNo.TexturePushed = _textureButtonSelected;
            _menuYes.TexturePushed = _textureButtonSelected;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _boundingRectangle = new Rectangle(0, (viewport.Height - _textureBackground.Height) / 2, viewport.Width, _textureBackground.Height);

            Point buttonMargin = new Point(_boundingRectangle.Width / 10, _boundingRectangle.Height / 10);

            _menuNo.BindingRectangle = new Rectangle(buttonMargin.X, _boundingRectangle.Bottom - buttonMargin.Y - _textureButton.Height, _textureButton.Width, _textureButton.Height);
            _menuYes.BindingRectangle = new Rectangle(_boundingRectangle.Right - buttonMargin.X - _textureButton.Width, _boundingRectangle.Bottom - buttonMargin.Y - _textureButton.Height, _textureButton.Width, _textureButton.Height);
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> Accepted;
        public event EventHandler<EventArgs> Cancelled;

        /// <summary>
        /// method to raise Accepted event
        /// </summary>
        private void OnAccepted(object sender, EventArgs e)
        {
            if (Accepted != null) Accepted(this, EventArgs.Empty);
        }

        /// <summary>
        /// method to raise Cancelled event
        /// </summary>
        private void OnCancelled(object sender, EventArgs e)
        {
            if (Cancelled != null) Cancelled(this, EventArgs.Empty);
        }

        #endregion

        #region Handle input

        /// <summary>
        /// Handles input, triggering Accepted or Canceled events
        /// </summary>
        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.NextEntry) || InputManager.IsActionTriggered(InputManager.Action.PreviousEntry))
            {
                _cancelSelected = !_cancelSelected;
            }

            if (InputManager.IsActionTriggered(InputManager.Action.Ok))
            {
                if (_cancelSelected) _menuNo.OnSelectEntry();
                else _menuYes.OnSelectEntry();

                ExitScreen();
            }

            if (InputManager.IsActionTriggered(InputManager.Action.Back))
            {
                _cancelSelected = true;
                _menuNo.OnSelectEntry();

                ExitScreen();
            }
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            //draw background texture
            spriteBatch.Draw(_textureBackground, _boundingRectangle, Color.White);

            //calculate message scaling and position
            Vector2 textSize = Fonts.MessageBoxFont.MeasureString(_message);
            float scale = 1.0f;
            if (textSize.X > _boundingRectangle.Width)
            {
                scale = (0.8f * _boundingRectangle.Width) / textSize.X;
                textSize = Vector2.Divide(textSize, 1f / scale);
            }

            //draw message
            Vector2 position = new Vector2((_boundingRectangle.Width - textSize.X )/ 2, _boundingRectangle.Top + ((_boundingRectangle.Height - textSize.Y) / 2));
            spriteBatch.DrawString(Fonts.MessageBoxFont, _message, position, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            if (_cancelSelected)
            {
                _menuYes.Draw(this, false, gameTime);
                _menuNo.Draw(this, true, gameTime);
            }
            else
            {
                _menuYes.Draw(this, true, gameTime);
                _menuNo.Draw(this, false, gameTime);
            }

            spriteBatch.End();
        }

        #endregion
    }
}
