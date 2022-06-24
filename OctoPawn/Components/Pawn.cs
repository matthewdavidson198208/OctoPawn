using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using OctoPawn.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace OctoPawn.Components
{
    public class Pawn : IEntity
    {
        public IShapeF Bounds { get; set; }
        private AnimatedSprite _spriteSheet;
        private Vector2 _spritePosition;
        private SoundEffect _moveSound;
        public bool IsDragged { get; set; }

        private readonly GameState _gameState;
        private readonly Game1 _game;

        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        private MouseState _currentMouse;
        private MouseState _previousMouse;
        public EventHandler Click;
        public bool Clicked { get; private set; }
        public int ID { get; private set; }
        public bool IsWhite { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Pawn(IServiceProvider serviceProvider, Vector2 position, GameState gameState, bool isWhite, int id, int row, int column)
        {
            content = new ContentManager(serviceProvider, "Content");
            _gameState = gameState;
            _game = gameState.game;

            Bounds = new RectangleF(position.X - 25, position.Y - 50, 50, 100);
            ID = id;
            Row = row;
            Column = column;

            SpriteSheet ss = null;
            IsWhite = isWhite;
            if(isWhite)
                ss = Content.Load<SpriteSheet>("Sprites/white_pawn.sf", new JsonContentLoader());
            else 
                ss = Content.Load<SpriteSheet>("Sprites/black_pawn.sf", new JsonContentLoader());

            _spriteSheet = new AnimatedSprite(ss);
            _spriteSheet.Play("animation0");
        }

        public void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            var rect = new Rectangle((int)Bounds.Position.X, (int)Bounds.Position.Y, 50, 100);
            if (mouseRectangle.Intersects(rect))
            {
                if (_currentMouse.LeftButton == ButtonState.Pressed)
                {
                    //_gameState.game.Select.Play(_gameState.game.SettingsManager.SFXManager.Volume,
                    //    _gameState.game.SettingsManager.SFXManager.Pitch, _gameState.game.SettingsManager.SFXManager.Pan);
                    Click?.Invoke(this, new EventArgs());
                    IsDragged = true;
                }
                else
                {
                    IsDragged = false;
                }
            }

            _spriteSheet.Play("animation0");
            _spriteSheet.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Currently for testing purposes
            //spriteBatch.FillRectangle((RectangleF)Bounds, Color.Blue, 0.5f);

            var position = new Vector2(Bounds.Position.X + 25, Bounds.Position.Y + 50);
            spriteBatch.Draw(_spriteSheet, position, 0, new Vector2(0.4f, 0.4f));
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            //throw new NotImplementedException();
        }
    }
}
