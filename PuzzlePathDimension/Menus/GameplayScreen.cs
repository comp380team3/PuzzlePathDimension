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
      // from the cache after the level ends.
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

      // Update the simulation's state
      simulation.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {
      // Look up inputs for the active player profile.
      int playerIndex = (int)ControllingPlayer.Value;

      Launcher launcher = simulation.Launcher;
      Ball ball = simulation.Ball;

      // Route user input to the approproate action
      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm) && simulation.Attempts > 0) {
        if (launcher.Movable) {
          launcher.LaunchBall();
        }
        // Stops the current attempt unless the ball hit the goal already
        else if (!simulation.Completed) {
          SubtractAttempt();
        }
      } else if (vtroller.Left == VirtualButtonState.Pressed) {
        launcher.AdjustAngle((float)Math.PI / 64);
      } else if (vtroller.Right == VirtualButtonState.Pressed) {
        launcher.AdjustAngle((float)-Math.PI / 64);
      } else if (Keyboard.GetState().IsKeyDown(Keys.Up)) {
        launcher.AdjustMagnitude(0.25f);
      } else if (Keyboard.GetState().IsKeyDown(Keys.Down)) {
        launcher.AdjustMagnitude(-0.25f);
      }

      // TODO: remove this test code
      if (Keyboard.GetState().IsKeyDown(Keys.F)) {
        Console.WriteLine(launcher);
      } else if (Keyboard.GetState().IsKeyDown(Keys.G)) {
        Console.WriteLine(ball);
      } else if (Keyboard.GetState().IsKeyDown(Keys.R)) { // Some crude restart mechanism
        Console.WriteLine("Completely restarted.");
        simulation.Restart();

        if (!launcher.Movable) {
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
      if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
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

      DrawWalls(spriteBatch);

      // Draw the goal on the canvas
      simulation.Goal.Draw(spriteBatch);

      // Draw the platform on the canvas
      foreach (Platform platform in simulation.Platforms) {
        platform.Draw(spriteBatch);
      }
      // Draw the treasures on the canvas
      foreach (Treasure treasure in simulation.Treasures) {
        treasure.Draw(spriteBatch);
      }
      // Draw the death traps on the canvas
      foreach (DeathTrap deathTrap in simulation.DeathTraps) {
        deathTrap.Draw(spriteBatch);
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

    private void DrawWalls(SpriteBatch spriteBatch) {
      Texture2D topBottom = content.Load<Texture2D>("TopBottom");
      Texture2D sideWall = content.Load<Texture2D>("SideWall");

      spriteBatch.Draw(topBottom, new Vector2(0, -5), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      spriteBatch.Draw(topBottom, new Vector2(0, 595), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      spriteBatch.Draw(sideWall, new Vector2(-5, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      spriteBatch.Draw(sideWall, new Vector2(795, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
  #endregion

  #region Test Level
    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    internal Simulation CreateTestLevel() {
      Simulation simulation = new Simulation(LevelLoader.Load("Content/TestLevel.xml", content));

      simulation.Background = content.Load<Texture2D>("GameScreen");

      Texture2D ballTex = content.Load<Texture2D>("ball_new");
      Ball ball = new Ball(ballTex);
      ball.InitBody(simulation.World);

      simulation.Ball = ball;

      simulation.Launcher.LoadBall(ball);

      return simulation;
    }
  #endregion

  /*#region Collision Detection
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

        bool collision = false;

        if (platform.Active) {
          collision = IntersectPixels(ballRectangle, ball.GetColorData(), platformRectangle, platform.GetColorData());
        }
        if (collision && platform.Breakable) {
          platform.Active = false;
        }
      }

      // I'm using rectangle collision for testing purposes; please replace this! - Jorenz
      foreach (Treasure treasure in simulation.Treasures) {
        Rectangle treasureRect = new Rectangle( (int)treasure.Position.X, (int)treasure.Position.Y,
          treasure.Width, treasure.Height);

        if (treasureRect.Intersects(ballRectangle)){
          treasure.Collect();
        }
      }

      foreach (DeathTrap deathTrap in simulation.DeathTraps) {
        Rectangle trapRect = new Rectangle((int)deathTrap.Position.X, (int)deathTrap.Position.Y,
          deathTrap.Width, deathTrap.Height);

        // Don't check if the player already ran out of balls, or else the attempts will
        // go into the negatives.
        if (trapRect.Intersects(ballRectangle) && simulation.Attempts > 0) {
          SubtractAttempt();
        }
      }

      Goal goal = simulation.Goal;
      Rectangle goalRect = new Rectangle((int)goal.Position.X, (int)goal.Position.Y,
        goal.Width, goal.Height);

      if (goalRect.Intersects(ballRectangle) && !simulation.Completed) {
        ball.Stop();
        simulation.Completed = true;
        Console.WriteLine("You win!");

        int treasures = 0;
        foreach (Treasure treasure in simulation.Treasures) {
          // Count every treasure that was collected.
          if (!treasure.Active) {
            treasures++;
          }
        }
        Console.WriteLine("Treasures obtained: " + treasures + "/" + simulation.Treasures.Count);
      }
    }
  #endregion*/

    private void SubtractAttempt() {
      simulation.Attempts -= 1;
      Console.WriteLine("Attempts left: " + simulation.Attempts);
      simulation.Ball.Stop();

      // Don't load a new ball if the player ran out of balls
      if (simulation.Attempts > 0) {
        simulation.Launcher.LoadBall(simulation.Ball);
      }
      else {
        Console.WriteLine("You lose!");
      }
    }

    /// <summary>
    /// Creates the level perimeter.
    /// </summary>
    /// <param name="theContent">The ContentManager that will be used to load the wall textures.</param>
    /*private void CreateBox(ContentManager theContent) {
      walls = new List<Platform>();
      Texture2D topBottom = theContent.Load<Texture2D>("TopBottom");
      Texture2D sideWall = theContent.Load<Texture2D>("SideWall");

      // The -5 offset is there because I prefer a 5-pixel thick wall, not a 10-pixel thick one. - Jorenz
      // It would probably be best to define some constants...
      Platform top = new Platform(topBottom, new Vector2(topBottom.Width, topBottom.Height), new Vector2(0, -5));
      Platform bottom = new Platform(topBottom, new Vector2(topBottom.Width, topBottom.Height), new Vector2(0, 595));
      Platform left = new Platform(sideWall, new Vector2(sideWall.Width, sideWall.Height), new Vector2(-5, 0));
      Platform right = new Platform(sideWall, new Vector2(sideWall.Width, sideWall.Height), new Vector2(795, 0));

      walls.Add(top);
      walls.Add(bottom);
      walls.Add(left);
      walls.Add(right);
    }*/
  }
}
