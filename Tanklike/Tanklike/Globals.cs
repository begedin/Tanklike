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
    public class Globals
    {
        public ContentManager Content { get; set; }
        public GraphicsDevice Graphics { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public Rectangle FullScreenRectangle { get; set; }
        public float ScreenWidth { get; set; }
        public float ScreenHeight { get; set; }

        public Globals();

        public void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            this.Content = content;
            this.Graphics = graphics;
            this.SpriteBatch = new SpriteBatch(graphics);

            this.ScreenHeight = Graphics.Viewport.Height;
            this.ScreenWidth = Graphics.Viewport.Width;

            this.FullScreenRectangle = new Rectangle(0, 0, (int)this.ScreenWidth, (int)this.ScreenHeight);
        }

        private static Globals _instance;
        public static Globals Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Globals();
                return _instance;
            }
        }
    }
}
