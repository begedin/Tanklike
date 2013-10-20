using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanklike
{
    /// <summary>
    /// Static storage for SpriteFonts and font colors to use throughout the game
    /// </summary>
    static class Fonts
    {
        public static readonly Color MENU_SELECTED_COLOR = new Color(248, 218, 127);
        public static readonly Color MENU_COLOR = new Color(59, 18, 6);

        #region Fonts

        private static SpriteFont _menuItemFont;
        public static SpriteFont MenuItemFont { get { return _menuItemFont; } }

        private static SpriteFont _messageBoxFont;
        public static SpriteFont MessageBoxFont { get { return _messageBoxFont; } }

        #endregion

        #region Initialization

        /// <summary>
        /// Load the fonts from the content pipeline.
        /// </summary>
        public static void LoadContent(ContentManager contentManager)
        {
            // check the parameters
            if (contentManager == null)
            {
                throw new ArgumentNullException("Content manager is missing");
            }

            _menuItemFont = contentManager.Load<SpriteFont>(@"Fonts\SegoeUIMono");
            _messageBoxFont = contentManager.Load<SpriteFont>(@"Fonts\SegoeUIMono");
        }


        /// <summary>
        /// Release all references to the fonts.
        /// </summary>
        public static void UnloadContent()
        {
            _menuItemFont = null;
        }

        #endregion

        
    }
}
