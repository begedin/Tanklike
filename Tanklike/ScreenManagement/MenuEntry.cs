using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanklike.ScreenManagement
{
    class MenuEntry
    {
        #region Fields

        private string _text;
        private SpriteFont _spriteFont;
        private Rectangle _bindingRectangle;
        private string _description;
        private Texture2D _textureNormal;
        private Texture2D _texturePushed;
        #endregion

        #region Properties

        /// <summary>
        /// The text to be displayed for this menu entry
        /// </summary>
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Sprite font used to draw this menu entry
        /// </summary>
        public SpriteFont Font
        {
            get { return _spriteFont; }
            set { _spriteFont = value; }
        }

        /// <summary>
        /// The binding rectangle for this menu entry. The menu entry is drawn to completely fit the binding rectangle.
        /// </summary>
        public Rectangle BindingRectangle
        {
            get { return _bindingRectangle; }
            set { _bindingRectangle = value; }
        }

        /// <summary>
        /// A description of the function of the button.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// An optional texture drawn with the text.
        /// </summary>
        /// <remarks>If present, the text will be centered on the texture.</remarks>
        public Texture2D TextureNormal
        {
            get { return _textureNormal; }
            set { _textureNormal = value; }
        }

        /// <summary>
        /// Optional texture drawn with the text, if the button is in the pushed/highlihted state.
        /// </summary>
        public Texture2D TexturePushed
        {
            get { return _texturePushed; }
            set { _texturePushed = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when the menu entry is selected
        /// </summary>
        public event EventHandler<EventArgs> Selected;

        /// <summary>
        /// Method to raise the selected event
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
            {
                Selected(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Creates a new menu entry.
        /// </summary>
        /// <param name="text">Text to display when drawing the entry.</param>
        public MenuEntry(string text)
        {
            this._text = text;
        }

        #endregion

        #region Update and Draw

        public virtual void Update(GameScreen screen, bool isSelected, GameTime gameTime) { }

        public virtual void Draw(GameScreen screen, bool isSelected, GameTime gameTime)
        {
            // Pick color based on selected status.
            Color color = isSelected ? Fonts.MENU_SELECTED_COLOR : Fonts.MENU_COLOR;

            // Draw text, centered on the middle of each line.
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;

            // Draw texture before the text
            if (isSelected)
            {
                if (_textureNormal != null)
                {
                    spriteBatch.Draw(_textureNormal, _bindingRectangle, Color.White);
                }
            }
            else
            {
                if (_texturePushed != null)
                {
                    spriteBatch.Draw(_texturePushed, _bindingRectangle, Color.White);
                }
            }


            // Draw text, centered within the binding rectangle
            if ((_spriteFont != null) && !String.IsNullOrEmpty(_text))
            {
                Vector2 textSize = _spriteFont.MeasureString(_text);
                //float scale = ((float)BindingRectangle.Width * 0.8f) / (float)textSize.X;
                //if ((textSize.Y * scale) > BindingRectangle.Height)
                //{
                //    scale = ((float)BindingRectangle.Height * 0.8f) / (float)textSize.Y;
                //}
                //Vector2 scaledTextSize = new Vector2(textSize.X * scale, textSize.Y * scale);

                //Vector2 textPosition = new Vector2(_bindingRectangle.Left, _bindingRectangle.Top) + new Vector2(
                //    (float)Math.Floor((_bindingRectangle.Width - scaledTextSize.X) / 2),
                //    (float)Math.Floor((_bindingRectangle.Height - scaledTextSize.Y) / 2));
                //spriteBatch.DrawString(_spriteFont, _text, textPosition, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);     


                Vector2 textPosition = new Vector2(_bindingRectangle.Left, _bindingRectangle.Top) + new Vector2(
                    (float)Math.Floor((_bindingRectangle.Width - textSize.X) / 2),
                    (float)Math.Floor((_bindingRectangle.Height - textSize.Y) / 2));
                spriteBatch.DrawString(_spriteFont, _text, textPosition, color);     
            }
        }

        #endregion
    }
}
