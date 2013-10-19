using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanklike
{

    /// <summary>
    /// Represents a tile piece in a square map. Each tile is defined by its position on the map and texture.
    /// </summary>
    public class Tile
    {
        private const int TILE_SIZE = 32;

        private Point mPosition;
        private Texture2D mTexture;

        /// <summary>
        /// Creates a new tile instance
        /// </summary>
        /// <param name="position">Position of the tile in the level.</param>
        /// <param name="type">Type of the tile. Type determines texture.</param>
        public Tile(Point position, ETileType type)
        {
            switch (type)
            {
                case ETileType.FLOOR:

                    break;
            }
        }

        /// <summary>
        /// Draws the tile on the screen
        /// </summary>
        /// <param name="spriteBatch">Shared SpriteBatch to use for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, new Vector2(TILE_SIZE * mPosition.X, TILE_SIZE * mPosition.Y), Color.White);
        }
    }
}
