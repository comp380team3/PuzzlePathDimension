using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  class ControllerDetectScreen : Screen {
    //Background texture for the screen
    Texture2D mControllerDetectScreenBackground;
    private MouseState oldState;
    private SpriteFont controlfont;

    TitleScreen mTitleScreen;

    Game1 game1;

    public ControllerDetectScreen(Game1 game1)
      : base(new EventHandler((o, e) => {})) {
      //Load the background texture for the screen
      mControllerDetectScreenBackground = game1.Content.Load<Texture2D>("PuzzlePathControllerScreen");
      controlfont = game1.Content.Load<SpriteFont>("MainMenuTitle");

      mTitleScreen = new TitleScreen(game1);

      this.game1 = game1;
    }

    //Update all of the elements that need updating in the Controller Detect Screen
    public override void Update(GameTime theTime) {
      MouseState newState = Mouse.GetState();
      int x = newState.X;
      int y = newState.Y;

      //Poll all the gamepads (and the keyboard) to check to see
      //which controller will be the player one controller. When the controlling
      //controller is detected, call the screen event associated with this screen
      if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed
         || Keyboard.GetState().IsKeyDown(Keys.A) == true) {
        game1.ReplaceState(mTitleScreen);
        return;
      }

      if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released) {
        game1.ReplaceState(mTitleScreen);
      }

      oldState = newState;
    }

    //Draw all of the elements that make up the Controller Detect Screen
    public override void Draw(SpriteBatch theBatch) {
      theBatch.Draw(mControllerDetectScreenBackground, Vector2.Zero, Color.White);
      theBatch.DrawString(controlfont, "Press A or Click the Screen to Continue", new Vector2(25, 500), Color.Black);
    }
  }
}
