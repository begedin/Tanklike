using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tanklike.Camera;
using Tanklike.ScreenManagement;
using Tanklike.Screens;

namespace Tanklike
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TanklikeGame : Game
    {
        GraphicsDeviceManager _graphics;
        ScreenManager screenManager;
        ResolutionIndependentRenderer _resolutionIndependence;
        Camera2D _camera;

        public TanklikeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _resolutionIndependence = new ResolutionIndependentRenderer(this);
            _camera = new Camera2D(_resolutionIndependence);
            
            InitializeResolutionIndependence(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            Content.RootDirectory = "Content";
            
            // add the screen manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InputManager.Initialize();
            // add the audio manager
            AudioManager.Initialize(this);

            base.Initialize();

            screenManager.AddScreen(new MainMenuScreen());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Fonts.LoadContent(Content);
            AudioManager.LoadContent(Content);

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Fonts.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }


        #region Helpers

        private void InitializeResolutionIndependence(int realScreenWidth, int realScreenHeight)
        {
            _resolutionIndependence.VirtualWidth = 1366;
            _resolutionIndependence.VirtualHeight = 768;
            _resolutionIndependence.ScreenWidth = realScreenWidth;
            _resolutionIndependence.ScreenHeight = realScreenHeight;
            _resolutionIndependence.Initialize();

            _camera.RecalculateTransformationMatrices();
        }

        #endregion
    }
}
