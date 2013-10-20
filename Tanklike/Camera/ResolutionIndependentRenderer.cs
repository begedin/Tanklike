using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanklike.Camera
{
    public class ResolutionIndependentRenderer
    {
        private readonly Game _game;
        private Viewport _viewport;
        private float _ratioX, _ratioY;

        public bool _dirtyMatrix;
        private static Matrix _scaleMatrix;

        private Vector2 _virtualMousePosition = new Vector2();

        public Color BackgroundColor = Color.Orange;

        public int VirtualWidth { get; set; }
        public int VirtualHeight { get; set; }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        public bool RenderingToScreenIsFinished { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ResolutionIndependentRenderer(Game game)
        {
            _game = game;
            VirtualWidth = 1366;
            VirtualHeight = 768;

            ScreenWidth = 1024;
            ScreenHeight = 768;
        }

        /// <summary>
        /// Initializes the renderer
        /// </summary>
        public void Initialize()
        {
            SetupVirtualScreenViewport();

            _ratioX = (float)_viewport.Width / VirtualWidth;
            _ratioY = (float)_viewport.Height / VirtualHeight;

            _dirtyMatrix = true;
        }

        /// <summary>
        /// Sets up a virtual screen viewport based on the current width and height properties.
        /// </summary>
        private void SetupVirtualScreenViewport()
        {
            var targetAspectRatio = VirtualWidth / (float)VirtualHeight;

            //calculating width and height with the "stretch to fit" approach. We stretch or shrink the image to fit the screen 
            //constraints, but maintain the aspect ratio

            var width = ScreenWidth;
            var height = (int)(width / targetAspectRatio + .5f);

            if (height > ScreenHeight)
            {
                height = ScreenHeight;
                width = (int)(height / targetAspectRatio + .5f);
            }

            // create a new viewport based on determined values and center it on screen
            _viewport = new Viewport
            {
                X = (ScreenWidth / 2) - (width / 2),
                Y = (ScreenHeight / 2) - (height / 2),
                Width = width,
                Height = height
            };

            _game.GraphicsDevice.Viewport = _viewport;
        }

        /// <summary>
        /// Sets the view port to the standard values, without scaling
        /// </summary>
        private void SetupFullViewPort()
        {
            var vp = new Viewport
            {
                X = 0,
                Y = 0,
                Width = ScreenWidth,
                Height = ScreenHeight
            };

            _game.GraphicsDevice.Viewport = vp;
            _dirtyMatrix = true;

        }

        public Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix)
            {
                RecreateScaleMatrix();
            }
            return _scaleMatrix;
        }

        private void RecreateScaleMatrix()
        {
            Matrix.CreateScale((float)ScreenWidth / VirtualWidth, (float)ScreenWidth / VirtualWidth, 1f, out _scaleMatrix);
            _dirtyMatrix = false;
        }

        public Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition) 
        {
            var realX = screenPosition.X - _viewport.X;
            var realY = screenPosition.Y - _viewport.Y;

            _virtualMousePosition.X = realX / _ratioX;
            _virtualMousePosition.Y = realY / _ratioY;

            return _virtualMousePosition;
        }
    }
}
