using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using OctoPawn.States;

namespace OctoPawn
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private State _currentState;
        private State _nextState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Change graphics size based on 700.0f Height Resolution
        /// </summary>
        /// <returns></returns>
        public float HeightY(float input)
        {
            //float h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            float h = _graphics.PreferredBackBufferHeight;
            return input * (h / 700.0f);
        }

        /// <summary>
        /// Change graphics size based on 1000.0f Width Resolution
        /// </summary>
        /// <returns></returns>
        public float WidthX(float input)
        {
            //float w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            float w = _graphics.PreferredBackBufferWidth;
            return input * (w / 1000.0f);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, Content);
            //_currentState = new GameState(this, Content);
            _currentState.LoadContent();
            _nextState = null;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;
                _currentState.LoadContent();

                _nextState = null;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            _currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
