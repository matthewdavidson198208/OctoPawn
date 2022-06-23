using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended.Content;
using MonoGame.Extended.Input;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using OctoPawn.Components;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace OctoPawn.States
{
    public class GameState : State
    {
        List<int> BoardState;
        private List<Tuple<int,Pawn>> whitePawns { get; set; }
        private List<Tuple<int, Pawn>> blackPawns { get; set; }

        public GameState(Game1 game1, ContentManager content1) : base(game1, content1)
        {
            BoardState = new List<int>() { 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 };
            whitePawns = new List<Tuple<int, Pawn>>();
            blackPawns = new List<Tuple<int, Pawn>>();
        }

        public override void LoadContent()
        {
            for(int i = 0; i < 4; i++)
            {
                var whitePawn = new Pawn(game.Services, new Vector2((125 * i + 295), 570), this, true, i + 4)
                {
                    Click = new EventHandler(onHold),
                };
                whitePawns.Add(new Tuple<int, Pawn>((12 + i), whitePawn));

                var blackPawn = new Pawn(game.Services, new Vector2((125 * i + 295), 195), this, false, i)
                {
                    Click = new EventHandler(onHold),
                };
                blackPawns.Add(new Tuple<int, Pawn>(i, blackPawn));
            }
            
        }

        private MouseState _currentMouse;
        private int _pawnID;
        private void onHold(object sender, EventArgs e)
        {
            var pawn = sender as Pawn;
            if (pawn.ID != _pawnID && _pawnID != -1)
                return;
            _pawnID = pawn.ID;
            _currentMouse = Mouse.GetState();
            pawn.Bounds = new RectangleF(_currentMouse.Position.X - 25, _currentMouse.Position.Y - 50, 50, 100);
        }

        public override void Update(GameTime gameTime)
        {
            _currentMouse = Mouse.GetState();
            if (_currentMouse.LeftButton == ButtonState.Released)
                _pawnID = -1;

            foreach (var white in whitePawns)
            {
                white.Item2.Update(gameTime);
            }
            foreach (var black in blackPawns)
            {
                black.Item2.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    Color color = (j + (4 * i)) % 2 == 0 ? Color.White : Color.Black;
                    if(i % 2 == 1)
                    {
                        if (color == Color.White)
                            color = Color.Black;
                        else if (color == Color.Black)
                            color = Color.White;
                    }
                    spriteBatch.FillRectangle(new RectangleF(new Point2((125 * j + 245), (125 * i + 125)),
                                new Size2(125, 125)), color);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var pos = j + (4 * i);
                    if (BoardState[pos] == 1)
                    {
                        whitePawns.FirstOrDefault(x => x.Item1 == pos).Item2.Draw(spriteBatch);
                        //spriteBatch.Draw(whitePawns[(j + (4 * i))], new Vector2((125 * j + 295), (125 * i + 195)), 0, new Vector2(scale, scale));
                    }
                    else if (BoardState[pos] == 2)
                    {
                        blackPawns.FirstOrDefault(x => x.Item1 == pos).Item2.Draw(spriteBatch);
                        //spriteBatch.Draw(blackPawns[(j + (4 * i))], new Vector2((125 * j + 295), (125 * i + 195)), 0, new Vector2(scale, scale));
                    }
                }
            }

                    spriteBatch.End();
        }

    }
}
