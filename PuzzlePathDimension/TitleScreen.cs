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

    Game1 game1;
    GameScreen mGameScreen;

    public TitleScreen(Game1 game1)
      : base(new EventHandler((o, e) => { })) {
      //Load the background texture for the screen
      mTitleScreenBackground = game1.Content.Load<Texture2D>("PuzzlePathMenu");
      menuFont = game1.Content.Load<SpriteFont>("MainMenuTitle");
      selectFont = game1.Content.Load<SpriteFont>("SelectFont");
      Position = new Vector2(200, 200);

      this.game1 = game1;
      this.mGameScreen = new GameScreen(game1);
    }

    //Update all of the elements that need updating in the Title Screen
    public override void Update(GameTime theTime) {
      //Get the current state of the mouse
      MouseState newState = Mouse.GetState();
      int x = newState.X;
      int y = newState.Y;

      //Check to see if the Player one controller has pressed the "B" button, if so, then
      //call the screen event associated with this screen
      if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter) == true) {
        game1.PushState(mGameScreen);
      } else if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released
                && x > Position.X + 100 && x < menuFont.MeasureString("Start Game").Length() + Position.X + 50
                && y > Position.Y + 100 && y < menuFont.MeasureString("Start Game").Y + Position.Y + 100) {
        game1.PushState(mGameScreen);
      }
      oldState = newState;
    }

    //Draw all of the elements that make up the Title Screen
    public override void Draw(SpriteBatch theBatch) {
      theBatch.Draw(mTitleScreenBackground, Vector2.Zero, Color.White);
      theBatch.DrawString(menuFont, "Puzzle Path Dimension", Position, Color.Black);
      theBatch.DrawString(selectFont, "Start Game", new Vector2(Position.X + 100, Position.Y + 100), Color.Black);
    }
  }
}
