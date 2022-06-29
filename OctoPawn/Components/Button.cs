using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctoPawn.States;

namespace OctoPawn.Components
{
    public class Button : IEntity
    {
        #region Fields

        private MouseState _currentMouse;

        private SpriteFont _font;

        private bool _isHovering;

        public bool IsHovering { get { return _isHovering; } }

        private MouseState _previousMouse;

        private Texture2D _texture;

        #endregion

        #region Properties

        public EventHandler Click;

        public bool Clicked { get; private set; }

        public float Layer { get; set; }

        public Vector2 Origin
        {
            get
            {
                return new Vector2(Width / 2, Height / 2);
            }
        }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                //return new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
                return new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - (int)Origin.Y, Width, Height);
            }
        }

        public IShapeF Bounds => throw new NotImplementedException();

        public string Text;

        public string Description;

        private State _gameState;

        #endregion

        #region Methods

        public Button(State gameState, int width, int height, SpriteFont font, Color color)
        {
            Width = width;

            Height = height;

            _font = font;

            PenColour = color;

            _gameState = gameState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var colour = PenColour;

            if (_isHovering)
                colour = Color.Gray;

            //spriteBatch.Draw(_texture, Position, null, colour, 0f, Origin, 1f, SpriteEffects.None, Layer);
            spriteBatch.DrawRectangle(Rectangle, colour);
            spriteBatch.FillRectangle(Rectangle, new Color(0,0,0,100));

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_gameState.game.WidthX(_font.MeasureString(Text).X) / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_gameState.game.HeightY(_font.MeasureString(Text).Y) / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour, 0f, new Vector2(0, 0),
                    new Vector2(_gameState.game.WidthX(1.0f), _gameState.game.HeightY(1.0f)), SpriteEffects.None, Layer + 0.01f);
            }
        }

        public void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    //_gameState.game.Select.Play(_gameState.game.SettingsManager.SFXManager.Volume,
                    //    _gameState.game.SettingsManager.SFXManager.Pitch, _gameState.game.SettingsManager.SFXManager.Pan);
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
