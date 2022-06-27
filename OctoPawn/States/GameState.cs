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
        public CPU AI { get; set; }
        public bool IsWhitesTurn { get; set; }
        public bool IsComputerPlaying { get; set; }
        public bool IsComputerFirst { get; set; }

        private int _selectedSquare;
        private List<HighlightSquare> HighlightSquares { get; set; }

        public enum WhoWins
        {
            None,
            White,
            Black
        }

        public WhoWins WhoWon { get; set; }

        private List<IEntity> _buttons;

        private int _selectedButton { get; set; }

        public GameState(Game1 game1, ContentManager content1) : base(game1, content1)
        {
            //TESTING
            IsComputerPlaying = true;
        }

        private void Reset()
        {
            if (IsComputerPlaying)
            {
                AI = new CPU();
            }
            else
            {
                AI = null;
            }
            WhoWon = WhoWins.None;
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

            var buttonFont = content.Load<SpriteFont>("Basic");
            _buttons = new List<IEntity>()
            {
                new Button(this, (int)game.WidthX(200), (int)game.HeightY(50), buttonFont, Color.ForestGreen)
                {
                  Text = "Retry",
                  Position = new Vector2(500, 300),
                  Click = new EventHandler(Button_Retry_Clicked),
                  Layer = 0.1f
                },
                new Button(this, (int)game.WidthX(200), (int)game.HeightY(50), buttonFont, Color.ForestGreen)
                {
                  Text = "Return",
                  Position = new Vector2(500, 360),
                  Click = new EventHandler(Button_Return_Clicked),
                  Layer = 0.1f
                },
                new Button(this, (int)game.WidthX(200), (int)game.HeightY(50), buttonFont, Color.ForestGreen)
                {
                  Text = "Quit",
                  Position = new Vector2(500, 420),
                  Click = new EventHandler(Button_Quit_Clicked),
                  Layer = 0.1f
                },
            };

            for (int i = 0; i < 4; i++)
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

        private void Button_Retry_Clicked(object sender, EventArgs args)
        {
            LoadContent();
        }

        private void Button_Return_Clicked(object sender, EventArgs args)
        {
            game.ChangeState(new MenuState(game, content));
        }

        private void Button_Quit_Clicked(object sender, EventArgs args)
        {
            game.Exit();
        }

        private MouseState _previousMouse;
        private MouseState _currentMouse;
        private int _pawnID;
        private void onHold(object sender, EventArgs e)
        {
            if ((IsComputerFirst && IsWhitesTurn) || (!IsComputerFirst && !IsWhitesTurn))
                return;

            if (WhoWon != WhoWins.None)
                return;

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

        public List<List<int>> UpdateBoard()
        {
            var output = new List<List<int>>(){
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 0, 0, 0, 0 }
            };
            foreach(var white in whitePawns)
            {
                output[white.Item2.Row][white.Item2.Column] = 1;
            }
            foreach (var black in blackPawns)
            {
                output[black.Item2.Row][black.Item2.Column] = 2;
            }
            return output;
        }

        public void UpdatePawns()
        {

            var i = 0;
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if(BoardState[j][k] == 1)
                    {
                        var highlight = HighlightSquares.FirstOrDefault(x => x.Row == j && x.Column == k);
                        whitePawns[i].Item2.Row = j;
                        whitePawns[i].Item2.Column = k;
                        whitePawns[i].Item2.Bounds.Position = highlight.SquarePosition.Location;
                        i++;
                    }
                }
            }

            i = 0;
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (BoardState[j][k] == 2)
                    {
                        var highlight = HighlightSquares.FirstOrDefault(x => x.Row == j && x.Column == k);
                        blackPawns[i].Item2.Row = j;
                        blackPawns[i].Item2.Column = k;
                        blackPawns[i].Item2.Bounds.Position = highlight.SquarePosition.Location;
                        i++;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var updateBoard = new List<List<int>>();
            if (CheckIfCantMoveAnymore() || WhoWon != WhoWins.None)
            {
                //continue onward
            }
            else if (IsComputerPlaying && (IsComputerFirst && IsWhitesTurn) || (!IsComputerFirst && !IsWhitesTurn))
            {
                BoardState = AI.MakeMove(BoardState);
                UpdatePawns();
                IsWhitesTurn = !IsWhitesTurn;
            }
            else if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
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
                    BoardState = UpdateBoard();
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

        public bool CheckIfCantMoveAnymore()
        {
            var cantMove = true;
            if (IsWhitesTurn)
            {
                foreach(var white in whitePawns)
                {
                    var checkBlackInFront = blackPawns.Exists(x => x.Item2.Row == white.Item2.Row - 1 && x.Item2.Column == white.Item2.Column);
                    var checkBlackOnSides = blackPawns.Exists(x => x.Item2.Row == white.Item2.Row - 1 && 
                        (x.Item2.Column == white.Item2.Column - 1 || x.Item2.Column == white.Item2.Column + 1));
                    if (!checkBlackInFront || checkBlackOnSides)
                    {
                        cantMove = false;
                        break;
                    }
                }
            }
            else
            {
                foreach (var black in blackPawns)
                {
                    var checkWhiteInFront = blackPawns.Exists(x => x.Item2.Row == black.Item2.Row + 1 && x.Item2.Column == black.Item2.Column);
                    var checkWhiteOnSides = blackPawns.Exists(x => x.Item2.Row == black.Item2.Row + 1 &&
                        (x.Item2.Column == black.Item2.Column - 1 || x.Item2.Column == black.Item2.Column + 1));
                    if (checkWhiteInFront || !checkWhiteOnSides)
                    {
                        cantMove = false;
                        break;
                    }
                }
            }

            if (cantMove && IsWhitesTurn)
                WhoWon = WhoWins.Black;
            else if (cantMove && !IsWhitesTurn)
                WhoWon = WhoWins.White;

            return cantMove;
        }

        public bool IsValidMove { get; set; }
        public bool CheckIfValidMove(bool isWhitePawn, Pawn pawn, HighlightSquare highlight)
        {
            if (isWhitePawn)
            {
                var checkAllyWhitePawn = whitePawns.Exists(x => x.Item2.Row == highlight.Row && x.Item2.Column == highlight.Column);
                if (checkAllyWhitePawn)
                    return false;

                if (pawn.Row - 1 != highlight.Row)
                    return false;

                var columnDif = pawn.Column - highlight.Column;
                if (Math.Abs(columnDif) > 1)
                    return false;

                var checkEnemyBlackPawn = blackPawns.Exists(x => x.Item2.Row == highlight.Row && x.Item2.Column == highlight.Column);
                if (pawn.Row - 1 == highlight.Row && Math.Abs(columnDif) == 1 && !checkEnemyBlackPawn)
                    return false;

                if (pawn.Row - 1 == highlight.Row && Math.Abs(columnDif) == 0 && checkEnemyBlackPawn)
                    return false;

                //Capture enemy piece if able
                if (checkEnemyBlackPawn)
                {
                    var captureEnemyBlackPawn = blackPawns.FirstOrDefault(x => x.Item2.Row == highlight.Row && x.Item2.Column == highlight.Column);
                    blackPawns.Remove(captureEnemyBlackPawn);
                    BoardState[highlight.Row][highlight.Column] = 1;
                    BoardState[pawn.Row][pawn.Column] = 0;
                }

                //Check winning condition
                if (highlight.Row == 0)
                    WhoWon = WhoWins.White;
            }
            else
            {
                var checkAllyBlackPawn = blackPawns.Exists(x => x.Item2.Row == highlight.Row && x.Item2.Column == highlight.Column);
                if (checkAllyBlackPawn)
                    return false;

                if (pawn.Row + 1 != highlight.Row)
                    return false;

                var columnDif = pawn.Column - highlight.Column;
                if (Math.Abs(columnDif) > 1)
                    return false;

                var checkEnemyWhitePawn = whitePawns.Exists(x => x.Item2.Row == highlight.Row && x.Item2.Column == highlight.Column);
                if (pawn.Row + 1 == highlight.Row && Math.Abs(columnDif) == 1 && !checkEnemyWhitePawn)
                    return false;

                if (pawn.Row + 1 == highlight.Row && Math.Abs(columnDif) == 0 && checkEnemyWhitePawn)
                    return false;

                //Capture enemy piece if able
                if (checkEnemyWhitePawn)
                {
                    var captureEnemyWhitePawn = whitePawns.FirstOrDefault(x => x.Item2.Row == highlight.Row && x.Item2.Column == highlight.Column);
                    whitePawns.Remove(captureEnemyWhitePawn);
                    BoardState[highlight.Row][highlight.Column] = 2;
                    BoardState[pawn.Row][pawn.Column] = 0;
                }

                //Check winning condition
                if (highlight.Row == 3)
                    WhoWon = WhoWins.Black;
            }
            
            return true;
        }

        public override void PostUpdate(GameTime gameTime)
        {
            if (WhoWon != WhoWins.None)
            {
                foreach (var button in _buttons)
                    button.Update(gameTime);
            }
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

            if (WhoWon != WhoWins.None)
            {
                var position3 = new Vector2(game.WidthX(425), game.HeightY(75));
                if (WhoWon == WhoWins.White)
                {
                    spriteBatch.DrawString(font, "White Wins!", position3, Color.White,
                        0, Vector2.Zero, new Vector2(game.WidthX(1.0f), game.HeightY(1.0f)), SpriteEffects.None, 0);
                }

                else if (WhoWon == WhoWins.Black)
                {
                    spriteBatch.DrawString(font, "Black Wins!", position3, Color.Black,
                        0, Vector2.Zero, new Vector2(game.WidthX(1.0f), game.HeightY(1.0f)), SpriteEffects.None, 0);
                }

                foreach (var button in _buttons)
                    button.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

    }
}
