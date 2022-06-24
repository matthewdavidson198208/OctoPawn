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
    public class HighlightSquare : IEntity
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

        public Color PenColor { get; set; }

        public Rectangle Rectangle{ get; set; }

        public IShapeF Bounds => throw new NotImplementedException();

        public string Description;

        private State _gameState;

        public int Width { get; set; }
        public int Height { get; set; }

        public Vector2 Origin
        {
            get
            {
                return new Vector2(Width / 4 - 5, Height / 4 - 11);
            }
        }

        public Rectangle SquarePosition
        {
            get
            {
                return new Rectangle((int)Rectangle.X + ((int)Origin.X), (int)Rectangle.Y + (int)Origin.Y, Width, Height);
            }
        }

        public int Row { get; set; }
        public int Column { get; set; }
        #endregion

        #region Methods

        public HighlightSquare(State gameState, Rectangle rectangle, Color color, int row, int column)
        {
            Rectangle = rectangle;

            Width = rectangle.Width;

            Height = rectangle.Height;

            Row = row;

            Column = column;

            PenColor = color;

            _gameState = gameState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isHovering)
            {
                if (_currentMouse.LeftButton == ButtonState.Pressed)
                    PenColor = Color.Red;
                else
                    PenColor = Color.Blue;
            }
            else
            {
                PenColor = Color.Transparent;
            }

            spriteBatch.DrawRectangle(Rectangle, PenColor, 5.0f, 0);
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
            }
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
