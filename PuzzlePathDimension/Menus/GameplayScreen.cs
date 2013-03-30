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
using FarseerPhysics.Dynamics;
using System.Collections.Generic;
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

    float pauseAlpha;

    World world;
    public const float unitToPixel = 100.0f;
    public const float pixelToUnit = 1 / unitToPixel;

    List<Platform> walls;
  #endregion

  #region Initialization
    /// <summary>
    /// Constructor.
    /// </summary>
    public GameplayScreen() {
      TransitionOnTime = TimeSpan.FromSeconds(1.5);
      TransitionOffTime = TimeSpan.FromSeconds(0.5);

      world = new World(new Vector2(0, 9.8f));
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

      world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

      // Update the launcher's state
      simulation.Launcher.Update();
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public override void HandleInput(InputState input) {
      if (input == null)
        throw new ArgumentNullException("input");

      // Look up inputs for the active player profile.
      int playerIndex = (int)ControllingPlayer.Value;

      Launcher launcher = simulation.Launcher;
      Ball ball = simulation.Ball;

      // Route user input to the approproate action
      if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
        launcher.LaunchBall();
      } else if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
        launcher.AdjustAngle((float)Math.PI / 64);
      } else if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
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
      }

      //Check to see if the Player one controller has pressed the "B" button, if so, then
      //call the screen event associated with this screen
      if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.B) == true) {
        ExitScreen();
        ScreenManager.AddScreen(new MainMenuScreen(), null);
      }
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

      foreach (Platform wall in walls) {
        wall.Draw(spriteBatch);
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
      Simulation simulation = new Simulation(LevelLoader.Load("Content/TestLevel.xml", content, world));

      simulation.Background = content.Load<Texture2D>("GameScreen");

      // Add a ball to the level
      Texture2D texture2 = content.Load<Texture2D>("ball_new");
      Ball ball = new Ball(world, texture2, new Vector2(texture2.Width, texture2.Height), 1);
      CreateBox(content);
      simulation.Ball = ball;

      // Load the ball into the launcher
      simulation.Launcher.LoadBall(ball);

      return simulation;
    }
  #endregion

    private void CreateBox(ContentManager theContent) {
      walls = new List<Platform>();

      Texture2D texture = theContent.Load<Texture2D>("TopBottom");
      Platform temp = new Platform(world, texture, new Vector2(texture.Width, texture.Height), 1, new Vector2(0, -5));
      walls.Add(temp);

      Texture2D texture2 = theContent.Load<Texture2D>("TopBottom");
      Platform temp2 = new Platform(world, texture, new Vector2(texture2.Width, texture2.Height), 1, new Vector2(0, 595));
      walls.Add(temp2);

      Texture2D texture3 = theContent.Load<Texture2D>("SideWall");
      Platform temp3 = new Platform(world, texture, new Vector2(texture3.Width, texture3.Height), 1, new Vector2(-5, 0));
      walls.Add(temp3);

      Texture2D texture4 = theContent.Load<Texture2D>("SideWall");
      Platform temp4 = new Platform(world, texture, new Vector2(texture4.Width, texture4.Height), 1, new Vector2(795, 0));
      walls.Add(temp4);
    }
  }
}
