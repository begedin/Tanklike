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

        private string m_strText;
        private SpriteFont m_sfSpriteFont;
        private Rectangle m_rBindingRectangle;
        private string m_strDescription;
        private Texture2D m_t2Texture;

        #endregion

        #region Properties

        /// <summary>
        /// The text to be displayed for this menu entry
        /// </summary>
        public String Text
        {
            get { return m_strText; }
            set { m_strText = value; }
        }

        /// <summary>
        /// Sprite font used to draw this menu entry
        /// </summary>
        public SpriteFont Font
        {
            get { return m_sfSpriteFont; }
            set { m_sfSpriteFont = value; }
        }

        /// <summary>
        /// The binding rectangle for this menu entry. The menu entry is drawn to completely fit the binding rectangle.
        /// </summary>
        public Rectangle BindingRectangle
        {
            get { return m_rBindingRectangle; }
            set { m_rBindingRectangle = value; }
        }

        /// <summary>
        /// A description of the function of the button.
        /// </summary>
        public string Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }

        /// <summary>
        /// An optional texture drawn with the text.
        /// </summary>
        /// <remarks>If present, the text will be centered on the texture.</remarks>
        public Texture2D Texture
        {
            get { return m_t2Texture; }
            set { m_t2Texture = value; }
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
            this.m_strText = text;
        }

        #endregion

        #region Update and Draw

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime) { }

        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // Pick color based on selected status.
            Color color = isSelected ? Fonts.MENU_SELECTED_COLOR : Fonts.MENU_COLOR;

            // Draw text, centered on the middle of each line.
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;

            // Draw texture before the text
            if (m_t2Texture != null)
            {
                spriteBatch.Draw(m_t2Texture, m_rBindingRectangle, Color.White);
            }

            // Draw text, centered within the binding rectangle
            if ((m_sfSpriteFont != null) && !String.IsNullOrEmpty(m_strText))
            {
                Vector2 textSize = m_sfSpriteFont.MeasureString(m_strText);
                Vector2 textPosition = new Vector2(m_rBindingRectangle.Top, m_rBindingRectangle.Left) + new Vector2(
                    (float)Math.Floor((m_rBindingRectangle.Width - textSize.X) / 2),
                    (float)Math.Floor((m_rBindingRectangle.Height - textSize.Y) / 2));
                spriteBatch.DrawString(m_sfSpriteFont, m_strText, textPosition, color);     
            }
        }

        #endregion
    }
}
