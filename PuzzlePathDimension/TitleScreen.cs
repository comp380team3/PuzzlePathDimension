using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  class TitleScreen : Screen {
    //Background texture for the Title screen
    Texture2D mTitleScreenBackground;
    Vector2 Position;
    private MouseState oldState;
    private SpriteFont menuFont;
    private SpriteFont selectFont;

    public TitleScreen(ContentManager theContent, EventHandler theScreenEvent)
      : base(theScreenEvent) {
      //Load the background texture for the screen
      mTitleScreenBackground = theContent.Load<Texture2D>("PuzzlePathMenu");
      menuFont = theContent.Load<SpriteFont>("MainMenuTitle");
      selectFont = theContent.Load<SpriteFont>("SelectFont");
      Position = new Vector2(200, 200);
    }

    //Update all of the elements that need updating in the Title Screen
    public override void Update(GameTime theTime) {
      //Get the current state of the mouse
      MouseState newState = Mouse.GetState();
      int x = newState.X;
      int y = newState.Y;

      //Check to see if the Player one controller has pressed the "B" button, if so, then
      //call the screen event associated with this screen
      if (GamePad.GetState(PlayerOne).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter) == true) {
        ScreenEvent.Invoke(this, new EventArgs());
      } else if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released &&
            x > Position.X + 100 && x < menuFont.MeasureString("Start Game").Length() + Position.X + 50 &&
            y > Position.Y + 100 && y < menuFont.MeasureString("Start Game").Y + Position.Y + 100) {
        ScreenEvent.Invoke(this, new EventArgs());
      }
      oldState = newState;

      base.Update(theTime);
    }

    //Draw all of the elements that make up the Title Screen
    public override void Draw(SpriteBatch theBatch) {
      theBatch.Draw(mTitleScreenBackground, Vector2.Zero, Color.White);
      theBatch.DrawString(menuFont, "Puzzle Path Dimension", Position, Color.Black);
      theBatch.DrawString(selectFont, "Start Game", new Vector2(Position.X + 100, Position.Y + 100), Color.Black);

      base.Draw(theBatch);
    }
  }
}
