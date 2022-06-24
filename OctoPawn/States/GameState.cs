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
        public List<List<int>> BoardState;
        public List<Tuple<int,Pawn>> whitePawns { get; set; }
        public List<Tuple<int, Pawn>> blackPawns { get; set; }
        public List<Tuple<Color, RectangleF>> BoardSquares { get; set; }
        public bool IsWhitesTurn { get; set; }

        private int _selectedSquare;
        private List<HighlightSquare> HighlightSquares { get; set; }


        public GameState(Game1 game1, ContentManager content1) : base(game1, content1)
        {
            
        }

        private void Reset()
        {
            IsWhitesTurn = IsValidMove = true;
            _selectedSquare = 12;
            _pawnID = -1;
            BoardState = new List<List<int>>() {
                new List<int>() { 2, 2, 2, 2 },
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 1, 1, 1, 1 }
            };
            whitePawns = new List<Tuple<int, Pawn>>();
            blackPawns = new List<Tuple<int, Pawn>>();
            BoardSquares = new List<Tuple<Color, RectangleF>>();
            HighlightSquares = new List<HighlightSquare>();
        }

        public override void LoadContent()
        {
            Reset();

            for(int i = 0; i < 4; i++)
            {
                var whitePawn = new Pawn(game.Services, new Vector2((125 * i + 295), 570), this, true, i + 4, 3, i)
                {
                    Click = new EventHandler(onHold),
                };
                whitePawns.Add(new Tuple<int, Pawn>((4 + i), whitePawn));

                var blackPawn = new Pawn(game.Services, new Vector2((125 * i + 295), 195), this, false, i, 0, i)
                {
                    Click = new EventHandler(onHold),
                };
                blackPawns.Add(new Tuple<int, Pawn>(i, blackPawn));
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Color color = (j + (4 * i)) % 2 == 0 ? Color.White : Color.Black;
                    if (i % 2 == 1)
                    {
                        if (color == Color.White)
                            color = Color.Black;
                        else if (color == Color.Black)
                            color = Color.White;
                    }

                    var rect = new RectangleF(new Point2((125 * j + 245), (125 * i + 125)),
                                new Size2(125, 125));
                    BoardSquares.Add(new Tuple<Color, RectangleF>(color, rect));

                    var highlight = new Rectangle(new Point((125 * j + 245), (125 * i + 125)),
                                new Size(125, 125));
                    HighlightSquares.Add(new HighlightSquare(this, highlight, Color.Transparent, i, j));
                }
            }

        }

        private MouseState _previousMouse;
        private MouseState _currentMouse;
        private int _pawnID;
        private void onHold(object sender, EventArgs e)
        {
            var pawn = sender as Pawn;
            if (pawn.IsWhite != IsWhitesTurn)
                return;
            if (pawn.ID != _pawnID && _pawnID != -1)
                return;
            _pawnID = pawn.ID;
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            pawn.Bounds = new RectangleF(_currentMouse.Position.X - 25, _currentMouse.Position.Y - 50, 50, 100);
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
            {
                if(_pawnID != -1)
                {
                    var isPlaced = false;
                    Pawn pawn;
                    if (IsWhitesTurn)
                    {
                        pawn = whitePawns.First(x => x.Item1 == _pawnID).Item2;
                    }
                    else
                    {
                        pawn = blackPawns.First(x => x.Item1 == _pawnID).Item2;
                    }
                    foreach (var highlight in HighlightSquares)
                    {
                        if (highlight.IsHovering)
                        {
                            IsValidMove = CheckIfValidMove(IsWhitesTurn, pawn, highlight);
                            if (IsValidMove)
                            {
                                pawn.Bounds.Position = highlight.SquarePosition.Location;
                                if(pawn.Row != highlight.Row || pawn.Column != highlight.Column)
                                {
                                    pawn.Row = highlight.Row;
                                    pawn.Column = highlight.Column;
                                    isPlaced = true;
                                    IsWhitesTurn = !IsWhitesTurn;
                                }
                                break;
                            }
                            else
                            {
                                var resetPosition = HighlightSquares.FirstOrDefault(x => 
                                    x.Row == pawn.Row && x.Column == pawn.Column);
                                isPlaced = true;
                                pawn.Bounds.Position = resetPosition.SquarePosition.Location;
                                break;
                            }
                        }
                    }
                    if (!isPlaced)
                    {
                        var resetPosition = HighlightSquares.FirstOrDefault(x =>
                                    x.Row == pawn.Row && x.Column == pawn.Column);
                        pawn.Bounds.Position = resetPosition.SquarePosition.Location;
                    }
                }
                _pawnID = -1;
            }

            foreach (var highlight in HighlightSquares)
            {
                highlight.Update(gameTime);
            }
            foreach (var white in whitePawns)
            {
                white.Item2.Update(gameTime);
            }
            foreach (var black in blackPawns)
            {
                black.Item2.Update(gameTime);
            }
        }

        public bool IsValidMove { get; set; }
        public bool CheckIfValidMove(bool isWhitePawn, Pawn pawn, HighlightSquare highlight)
        {
            //TODO: Fill logic here
            if (isWhitePawn)
            {
                var checkAllyWhitePawn = whitePawns.Exists(x => x.Item2.Row == highlight.Row && x.Item2.Column == highlight.Column);
                if (checkAllyWhitePawn)
                    return false;

                var checkEnemySpotInFront = blackPawns.Exists(x => x.Item2.Row == pawn.Row - 1 && x.Item2.Column == pawn.Column);
                var checkCaptureEnemy = whitePawns.Exists(x => x.Item2.Row == highlight.Row && Math.Abs(x.Item2.Column - highlight.Column) == 1);
                var columnDif = pawn.Row - highlight.Row;
                if (pawn.Row - 1 != highlight.Row)
                    return false;
                if ((pawn.Column != highlight.Column && checkEnemySpotInFront) ||
                    (Math.Abs(columnDif) != 1 && !checkCaptureEnemy))
                    return false;
            }
            else
            {
                var checkAllyBlackPawn = blackPawns.Exists(x => x.Item2.Row == highlight.Row && x.Item2.Column == highlight.Column);
                if (checkAllyBlackPawn)
                    return false;


                var checkEnemySpotInFront = whitePawns.Exists(x => x.Item2.Row == pawn.Row + 1 && x.Item2.Column == pawn.Column);
                var checkCaptureEnemy = whitePawns.Exists(x => x.Item2.Row == highlight.Row && Math.Abs(x.Item2.Column - highlight.Column) == 1);
                var columnDif = pawn.Row - highlight.Row;
                if (pawn.Row + 1 != highlight.Row)
                    return false;
                if ((pawn.Column != highlight.Column && checkEnemySpotInFront) ||
                    (Math.Abs(columnDif) != 1 && !checkCaptureEnemy))
                    return false;
            }
            
            return true;
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var font = content.Load<SpriteFont>("Description");
            var position = new Vector2(game.WidthX(425), game.HeightY(25));
            var text = IsWhitesTurn ? "White's Turn" : "Black's Turn";
            var color = IsWhitesTurn ? Color.White : Color.Black;
            spriteBatch.DrawString(font, text, position, color,
                    0, Vector2.Zero, new Vector2(game.WidthX(1.0f), game.HeightY(1.0f)), SpriteEffects.None, 0);

            if (!IsValidMove)
            {
                var position2 = new Vector2(game.WidthX(425), game.HeightY(75));
                spriteBatch.DrawString(font, "Invalid Move!", position2, color,
                    0, Vector2.Zero, new Vector2(game.WidthX(1.0f), game.HeightY(1.0f)), SpriteEffects.None, 0);
            }

            foreach (var square in BoardSquares)
            {
                spriteBatch.FillRectangle(square.Item2, square.Item1);
            }

            foreach(var highlight in HighlightSquares)
            {
                highlight.Draw(spriteBatch);
            }

            foreach(var pawn in whitePawns)
            {
                pawn.Item2.Draw(spriteBatch);
            }

            foreach(var pawn in blackPawns)
            {
                pawn.Item2.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

    }
}
