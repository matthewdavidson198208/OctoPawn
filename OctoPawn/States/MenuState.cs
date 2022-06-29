using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using OctoPawn.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace OctoPawn.States
{
    public class MenuState : State
    {
        private List<IEntity> _buttons;

        private int _selectedButton { get; set; }

        public MenuState(Game1 game1, ContentManager content1) : base(game1, content1)
        {
        }

       
        public override void LoadContent()
        {
            var buttonFont = content.Load<SpriteFont>("Basic");

            _buttons = new List<IEntity>()
            {
                new Button(this, (int)game.WidthX(200), (int)game.HeightY(50), buttonFont, Color.White)
                {
                  Text = "Play",
                  Position = new Vector2(500, game.HeightY(300)),
                  Click = new EventHandler(Button_Player_Clicked),
                  Layer = 0.1f
                },
                new Button(this, (int)game.WidthX(200), (int)game.HeightY(50), buttonFont, Color.White)
                {
                  Text = "How To",
                  Position = new Vector2(500, game.HeightY(360)),
                  Click = new EventHandler(Button_Instructions_Clicked),
                  Layer = 0.1f
                },
                new Button(this, (int)game.WidthX(200), (int)game.HeightY(50), buttonFont, Color.White)
                {
                  Text = "Quit",
                  Position = new Vector2(500, game.HeightY(420)),
                  Click = new EventHandler(Button_Quit_Clicked),
                  Layer = 0.1f
                },
            };
        }

        private void Button_Player_Clicked(object sender, EventArgs args)
        {
            game.ChangeState(new TransitionState(game, content));
        }

        private void Button_Instructions_Clicked(object sender, EventArgs args)
        {
            game.ChangeState(new InstructionsState(game, content));
        }

        private void Button_Quit_Clicked(object sender, EventArgs args)
        {
            game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in _buttons)
                button.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            var keyboardState = KeyboardExtended.GetState();
            if (keyboardState.WasKeyJustDown(Keys.Down) || keyboardState.WasKeyJustDown(Keys.S))
            {
                //game.Select.Play(game.SettingsManager.SFXManager.Volume, game.SettingsManager.SFXManager.Pitch, game.SettingsManager.SFXManager.Pan);
                if (_selectedButton < _buttons.Count - 1)
                    _selectedButton++;
            }

            else if (keyboardState.WasKeyJustDown(Keys.Up) || keyboardState.WasKeyJustDown(Keys.W))
            {
                //game.Select.Play(game.SettingsManager.SFXManager.Volume, game.SettingsManager.SFXManager.Pitch, game.SettingsManager.SFXManager.Pan);
                if (_selectedButton != 0)
                    _selectedButton--;
            }

            if (keyboardState.WasKeyJustDown(Keys.Enter))
            {
                //game.Select.Play(game.SettingsManager.SFXManager.Volume, game.SettingsManager.SFXManager.Pitch, game.SettingsManager.SFXManager.Pan);
                ((Button)_buttons[_selectedButton]).Click.Invoke(this, new EventArgs());
            }
        }

        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            var titlePosition = new Vector2(game.WidthX(425), game.HeightY(100));
            var titleFont = content.Load<SpriteFont>("Basic");
            spriteBatch.DrawString(titleFont, "OctoPawn", titlePosition, Color.White,
                    0, Vector2.Zero, new Vector2(game.WidthX(1.0f), game.HeightY(1.0f)), SpriteEffects.None, 0);

            var developerPosition = new Vector2(game.WidthX(325), game.HeightY(175));
            spriteBatch.DrawString(titleFont, "Programmed by matt D", developerPosition, Color.White,
                    0, Vector2.Zero, new Vector2(game.WidthX(1.0f), game.HeightY(1.0f)), SpriteEffects.None, 0);

            foreach (var button in _buttons)
                button.Draw(spriteBatch);

            //var width = ((Button)_buttons[_selectedButton]).Position.X - (((Button)_buttons[_selectedButton]).Rectangle.Width / 2) - game.WidthX(25);
            //var height = ((Button)_buttons[_selectedButton]).Position.Y;
            //spriteBatch.DrawCircle(new CircleF(new Point2(width, height),
            //    game.WidthX(10)), 6, Color.White);

            //var width2 = ((Button)_buttons[_selectedButton]).Position.X + (((Button)_buttons[_selectedButton]).Rectangle.Width / 2) + game.WidthX(25);
            //var height2 = ((Button)_buttons[_selectedButton]).Position.Y;
            //spriteBatch.DrawCircle(new CircleF(new Point2(width2, height2),
            //   game.WidthX(10)), 6, Color.White);

            var font = content.Load<SpriteFont>("Description");
            spriteBatch.DrawString(font,
                "               Use Mouse to select an option.\n" +
                "               Press Escape Key to quit at any time.\n",
                new Vector2(200, game.HeightY(480)),
                Color.White, 0, Vector2.Zero, new Vector2(game.WidthX(1.0f), game.HeightY(1.0f)), SpriteEffects.None, 0);

            spriteBatch.End();
        }

    }
}
