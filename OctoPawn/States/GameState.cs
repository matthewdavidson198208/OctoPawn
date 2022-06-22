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

namespace OctoPawn.States
{
    public class GameState : State
    {
        List<int> BoardState;
        private Dictionary<int,AnimatedSprite> whitePawns { get; set; }
        private Dictionary<int, AnimatedSprite> blackPawns { get; set; }

        public GameState(Game1 game1, ContentManager content1) : base(game1, content1)
        {
            BoardState = new List<int>() { 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 };
            whitePawns = new Dictionary<int, AnimatedSprite>();
            blackPawns = new Dictionary<int, AnimatedSprite>();
        }

        public override void LoadContent()
        {
            for(int i = 0; i < 4; i++)
            {
                var whitePawn = new AnimatedSprite(content.Load<SpriteSheet>("Sprites/white_pawn.sf", new JsonContentLoader()));
                whitePawn.Play("animation0");
                whitePawns.Add((12 + i),whitePawn);

                var blackPawn = new AnimatedSprite(content.Load<SpriteSheet>("Sprites/black_pawn.sf", new JsonContentLoader()));
                blackPawn.Play("animation0");
                blackPawns.Add(i, blackPawn);
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            foreach(var white in whitePawns)
            {
                white.Value.Play("animation0");
                white.Value.Update(gameTime);
            }
            foreach (var black in blackPawns)
            {
                black.Value.Play("animation0");
                black.Value.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            var scale = 0.4f;
            //var scale = 0.8f;
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

                    if(BoardState[(j + (4 * i))] == 1)
                    {
                        spriteBatch.Draw(whitePawns[(j + (4 * i))], new Vector2((125 * j + 295), (125 * i + 195)), 0, new Vector2(scale, scale));
                    }
                    else if(BoardState[(j + (4 * i))] == 2)
                    {
                        spriteBatch.Draw(blackPawns[(j + (4 * i))], new Vector2((125 * j + 295), (125 * i + 195)), 0, new Vector2(scale, scale));
                    }
                }
            }

            spriteBatch.End();
        }

    }
}
