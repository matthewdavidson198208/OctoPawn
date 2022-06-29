using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OctoPawn.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace OctoPawn.States
{
    public class TransitionState : State
    {
        private List<IEntity> _buttons;

        private bool _isComputerPlaying;

        public TransitionState(Game1 game1, ContentManager content1) : base(game1, content1)
        {
        }

        public override void LoadContent()
        {
            var buttonFont = content.Load<SpriteFont>("Basic");

            _buttons = new List<IEntity>()
            {
                new Button(this, (int)game.WidthX(300), (int)game.HeightY(50), buttonFont, Color.White)
                {
                  Text = "Player vs Computer",
                  Position = new Vector2(500, 250),
                  Click = new EventHandler(Button_PvC_Clicked),
                  Layer = 0.1f
                },
                new Button(this, (int)game.WidthX(300), (int)game.HeightY(50), buttonFont, Color.White)
                {
                  Text = "Player vs Player",
                  Position = new Vector2(500, 410),
                  Click = new EventHandler(Button_PvP_Clicked),
                  Layer = 0.1f
                },
                new Button(this, (int)game.WidthX(300), (int)game.HeightY(50), buttonFont, Color.White)
                {
                  Text = "Computer First",
                  Position = new Vector2(500, 250),
                  Click = new EventHandler(Button_CFirst_Clicked),
                  Layer = 0.1f
                },
                new Button(this, (int)game.WidthX(300), (int)game.HeightY(50), buttonFont, Color.White)
                {
                  Text = "Player First",
                  Position = new Vector2(500, 410),
                  Click = new EventHandler(Button_PFirst_Clicked),
                  Layer = 0.1f
                },
            };
        }

        private void Button_PvC_Clicked(object sender, EventArgs args)
        {
            _isComputerPlaying = true;
        }

        private void Button_PvP_Clicked(object sender, EventArgs args)
        {
            game.ChangeState(new GameState(game, content)
            {
                IsComputerPlaying = false,
                IsComputerFirst = false
            });
        }

        private void Button_CFirst_Clicked(object sender, EventArgs args)
        {
            game.ChangeState(new GameState(game, content)
            {
                IsComputerPlaying = true,
                IsComputerFirst = true
            });
        }

        private void Button_PFirst_Clicked(object sender, EventArgs args)
        {
            game.ChangeState(new GameState(game, content)
            {
                IsComputerPlaying = true,
                IsComputerFirst = false
            });
        }

        public override void Update(GameTime gameTime)
        {
            if (!_isComputerPlaying)
            {
                _buttons[0].Update(gameTime);
                _buttons[1].Update(gameTime);
            }
            else
            {
                _buttons[2].Update(gameTime);
                _buttons[3].Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }
       
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            if (!_isComputerPlaying)
            {
                _buttons[0].Draw(spriteBatch);
                _buttons[1].Draw(spriteBatch);
            }
            else
            {
                _buttons[2].Draw(spriteBatch);
                _buttons[3].Draw(spriteBatch);
            }

            spriteBatch.End();
        }

    }
}
