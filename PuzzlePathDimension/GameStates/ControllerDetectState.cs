using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  class ControllerDetectState : GameState {
    //Background texture for the screen
    Texture2D mControllerDetectScreenBackground;
    private MouseState oldState;
    private SpriteFont controlfont;

    TitleState mTitleState;

    Game1 game1;

    public ControllerDetectState(Game1 game1, SpriteBatch spriteBatch) {
      //Load the background texture for the screen
      mControllerDetectScreenBackground = game1.Content.Load<Texture2D>("PuzzlePathControllerScreen");
      controlfont = game1.Content.Load<SpriteFont>("MainMenuTitle");

      mTitleState = new TitleState(game1, spriteBatch);

      this.game1 = game1;
    }

    //Update all of the elements that need updating in the Controller Detect Screen
    public void Update(GameTime theTime) {
      MouseState newState = Mouse.GetState();
      int x = newState.X;
      int y = newState.Y;

      //Poll all the gamepads (and the keyboard) to check to see
      //which controller will be the player one controller. When the controlling
      //controller is detected, call the screen event associated with this screen
      if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed
         || Keyboard.GetState().IsKeyDown(Keys.A) == true) {
        game1.ReplaceState(mTitleState);
        return;
      }

      if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released) {
        game1.ReplaceState(mTitleState);
      }

      oldState = newState;
    }

    //Draw all of the elements that make up the Controller Detect Screen
    public void Draw(GameTime theTime, SpriteBatch theBatch) {
      theBatch.Draw(mControllerDetectScreenBackground, Vector2.Zero, Color.White);
      theBatch.DrawString(controlfont, "Press A or Click the Screen to Continue", new Vector2(25, 500), Color.Black);
    }
  }
}
