using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OctoPawn.Components;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended.Content;
using MonoGame.Extended.Input;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Input;

namespace OctoPawn.States
{
    public class InstructionsState : State
    {
        private Button returnButton { get; set; }
        private AnimatedSprite whitePawn { get; set; }
        private AnimatedSprite blackPawn { get; set; }

        public InstructionsState(Game1 game1, ContentManager content1) : base(game1, content1)
        {
        }

        public override void LoadContent()
        {
            var buttonFont = content.Load<SpriteFont>("Basic");
            returnButton = new Button(this, (int)game.WidthX(200), (int)game.HeightY(50), buttonFont, Color.White)
            {
                Text = "Return",
                Description = "Return to Main Menu",
                Position = new Vector2(1000.0f / 3.0f + game.WidthX(170), game.HeightY(640)),
                Click = new EventHandler(Button_Exit_Clicked),
                Layer = 0.1f
            };

            whitePawn = new AnimatedSprite(content.Load<SpriteSheet>("Sprites/white_pawn.sf", new JsonContentLoader()));
            whitePawn.Play("animation0");

            blackPawn = new AnimatedSprite(content.Load<SpriteSheet>("Sprites/black_pawn.sf", new JsonContentLoader()));
            blackPawn.Play("animation0");
        }

        private void Button_Exit_Clicked(object sender, EventArgs args)
        {
            game.ChangeState(new MenuState(game, content));
        }

        public override void Update(GameTime gameTime)
        {
            returnButton.Update(gameTime);
            whitePawn.Play("animation0");
            whitePawn.Update(gameTime);
            blackPawn.Play("animation0");
            blackPawn.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            var keyboardState = KeyboardExtended.GetState();
            if (keyboardState.WasKeyJustDown(Keys.Enter))
            {
                returnButton.Click.Invoke(returnButton, new EventArgs());
                //game.Select.Play(game.SettingsManager.SFXManager.Volume, game.SettingsManager.SFXManager.Pitch, game.SettingsManager.SFXManager.Pan);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            var font = content.Load<SpriteFont>("Description");
            var x = 1000.0f / 3.0f - game.WidthX(565);

            returnButton.Draw(spriteBatch);

            var scale = 0.5f;
            spriteBatch.Draw(whitePawn, new Vector2(x + game.WidthX(650), game.HeightY(450)), 0, new Vector2(scale, scale));
            spriteBatch.Draw(blackPawn, new Vector2(x + game.WidthX(800), game.HeightY(450)), 0, new Vector2(scale, scale));

            spriteBatch.DrawString(font, "The objective of the game is to get one pawn to the opposite side of the board.\n" +
                "Pawns move how the normally would in chess,\n" +
                "but are not allowed their double step move option.\n" +
                "En passant is not allowed.\n" +
                "If a player has no legal moves, they lose.\n" +
                "The first player to get one of their pawns to the opposite of the board wins.\n" +
                "To move a pawn with the mouse, drag and drop the pawn you want to move.\n",
                new Vector2(x + game.WidthX(300), game.HeightY(125)), Color.White, 0,
                Vector2.Zero, new Vector2(game.WidthX(1.0f), game.HeightY(1.0f)), SpriteEffects.None, 0);

            spriteBatch.DrawString(font, "Press Enter or click Return to go to Main Menu",
               new Vector2(x + game.WidthX(500), game.HeightY(570)),
               Color.White, 0, Vector2.Zero, new Vector2(game.WidthX(1.0f), game.HeightY(1.0f)), SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
