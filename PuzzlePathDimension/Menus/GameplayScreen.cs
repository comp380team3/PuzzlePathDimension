#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace PuzzlePathDimension {
  /// <summary>
  /// This screen implements the actual game logic. It is just a
  /// placeholder to get the idea across: you'll probably want to
  /// put some more interesting gameplay in here!
  /// </summary>
  class GameplayScreen : GameScreen {
  #region Fields
    ContentManager content;
    Simulation simulation;
    Vector2 playerPosition = new Vector2(100, 100);

    float pauseAlpha;
  #endregion

  #region Initialization
    /// <summary>
    /// Constructor.
    /// </summary>
    public GameplayScreen() {
      TransitionOnTime = TimeSpan.FromSeconds(1.5);
      TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    /// <summary>
    /// Load graphics content for the game.
    /// </summary>
    public override void LoadContent() {
      // Create a new ContentManager so that all level data is flushed
      //   from the cache after the level ends.
      if (content == null)
        content = new ContentManager(ScreenManager.Game.Services, "Content");

      simulation = CreateTestLevel();

      // A real game would probably have more content than this sample, so
      // it would take longer to load. We simulate that by delaying for a
      // while, giving you a chance to admire the beautiful loading screen.
      Thread.Sleep(1000);

      // once the load has finished, we use ResetElapsedTime to tell the game's
      // timing mechanism that we have just finished a very long frame, and that
      // it should not try to catch up.
      ScreenManager.Game.ResetElapsedTime();
    }

    /// <summary>
    /// Unload graphics content used by the game.
    /// </summary>
    public override void UnloadContent() {
      content.Unload();
    }
  #endregion

  #region Update and Draw
    /// <summary>
    /// Updates the state of the game. This method checks the GameScreen.IsActive
    /// property, so the game will stop updating when the pause menu is active,
    /// or if you tab away to a different application.
    /// </summary>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                   bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, false);

      // Gradually fade in or out depending on whether we are covered by the pause screen.
      if (coveredByOtherScreen)
        pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
      else
        pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

      // Bail early if this isn't the active screen.
      if (!IsActive)
        return;

      // Update the launcher's state
      simulation.Launcher.Update();

      // Update the balls position
      simulation.Ball.Update();

      // Update the collision
      UpdateCollision();
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {
      /*if (input == null)
        throw new ArgumentNullException("input");*/

      // Look up inputs for the active player profile.
      int playerIndex = (int)ControllingPlayer.Value;
      /*
      KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
      GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

      // The game pauses either if the user presses the pause button, or if
      // they unplug the active gamepad. This requires us to keep track of
      // whether a gamepad was ever plugged in, because we don't want to pause
      // on PC if they are playing with a keyboard and have no gamepad at all!
      bool gamePadDisconnected = !gamePadState.IsConnected &&
                                 input.GamePadWasConnected[playerIndex];
      */
      if (vtroller.CheckForRecentRelease(VirtualButtons.Back)/*|| gamePadDisconnected*/) {
        ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
      } 

      Launcher launcher = simulation.Launcher;
      Ball ball = simulation.Ball;

      // Route user input to the approproate action
      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        launcher.LaunchBall();
      } else if (vtroller.Left == VirtualButtonState.Pressed) {
        launcher.AdjustAngle((float)Math.PI / 64);
      } else if (vtroller.Right == VirtualButtonState.Pressed) {
        launcher.AdjustAngle((float)-Math.PI / 64);
      }

      // TODO: remove this test code
      if (Keyboard.GetState().IsKeyDown(Keys.F)) {
        Console.WriteLine(launcher);
      } else if (Keyboard.GetState().IsKeyDown(Keys.G)) {
        Console.WriteLine(ball);
      } else if (Keyboard.GetState().IsKeyDown(Keys.R)) {
        if (!launcher.Active) { // Some crude restart mechanism
          ball.Stop();
          launcher.LoadBall(ball);
        }
      }

      MouseState mouse = Mouse.GetState();
      if (mouse.LeftButton == ButtonState.Pressed) {
        Console.WriteLine("Mouse click at: " + mouse.X + ", " + mouse.Y);
      }/*
<<<<<<< HEAD
=======

      //Check to see if the Player one controller has pressed the "B" button, if so, then
      //call the screen event associated with this screen
      if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        ExitScreen();
        ScreenManager.AddScreen(new MainMenuScreen(), null);
      }
>>>>>>> origin/master*/
    }

    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>
    public override void Draw(GameTime gameTime) {
      ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0, 0);

      SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

      spriteBatch.Begin();

      spriteBatch.Draw(simulation.Background, Vector2.Zero, Color.White);

      // Draw the goal on the canvas
      simulation.Goal.Draw(spriteBatch);

      // Draw the platform on the canvas
      foreach (Platform platform in simulation.Platforms) {
        platform.Draw(spriteBatch);
      }

      // Draw the ball onto the canvas
      simulation.Ball.Draw(spriteBatch);

      // Draw the launcher on the canvas
      simulation.Launcher.Draw(spriteBatch);

      spriteBatch.End();

      // If the game is transitioning on or off, fade it out to black.
      if (TransitionPosition > 0 || pauseAlpha > 0) {
        float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

        ScreenManager.FadeBackBufferToBlack(alpha);
      }
    }
  #endregion

  #region Test Level
    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    internal Simulation CreateTestLevel() {
      Simulation simulation = new Simulation(LevelLoader.Load("Content/TestLevel.xml", content));

      simulation.Background = content.Load<Texture2D>("GameScreen");

      // Add a ball to the level
      Ball ball = new Ball();
      Vector2 ballPos = new Vector2(400f, 300f);
      ball.Initialize(ScreenManager.Game.GraphicsDevice.Viewport, content.Load<Texture2D>("ball_new"), ballPos);
      simulation.Ball = ball;

      // Load the ball into the launcher
      simulation.Launcher.LoadBall(ball);

      return simulation;
    }
  #endregion

  #region Collision Detection
    private bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB) {
      Ball ball = simulation.Ball;

      // Check if the two objects are near each other.
      // If they are not then return false for no intersection.
      if (!rectangleA.Intersects(rectangleB)) {
        return false;
      }

      // Find the bounds of the rectangle intersection
      int top = Math.Max(rectangleA.Top, rectangleB.Top);
      int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
      int left = Math.Max(rectangleA.Left, rectangleB.Left);
      int right = Math.Min(rectangleA.Right, rectangleB.Right);

      // Check every point within the intersection bounds
      for (int y = top; y < bottom; y++) {
        for (int x = left; x < right; x++) {
          // Get the color of both pixels at this point
          //Console.WriteLine("Index to fetch: " + (((x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width) % 400));
          Console.WriteLine("Length of dataB: " + dataB.Length);
          Color colorA = dataA[((x - rectangleA.Left) +
                               (y - rectangleA.Top) * rectangleA.Width) % dataA.Length];
          Color colorB = dataB[((x - rectangleB.Left) +
                               (y - rectangleB.Top) * rectangleB.Width) % dataB.Length]; // lol maybe?

          // If both pixels are not completely transparent,
          if (colorA.A != 0 && colorB.A != 0) {
            if (y == top || y == bottom - 1)
              ball.FlipYDirection();
            if (x == left || x == right - 1)
              ball.FlipXDirection();
            // then an intersection has been found
            return true;
          }
        }
      }

      // No intersection found
      return false;
    }

    private void UpdateCollision() {
      Ball ball = simulation.Ball;

      Rectangle ballRectangle = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, ball.Width, ball.Height);

      foreach (Platform platform in simulation.Platforms) {
        Rectangle platformRectangle = new Rectangle(
            (int)platform.Position.X,
            (int)platform.Position.Y,
            platform.Width,
            platform.Height);

        IntersectPixels(ballRectangle, ball.GetColorData(), platformRectangle, platform.GetColorData());
      }
    }
  #endregion
  }
}
