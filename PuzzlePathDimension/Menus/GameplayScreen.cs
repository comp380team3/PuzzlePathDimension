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

      // Create the hard-coded level.
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

      // Update the simulation's state.
      simulation.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {
      Launcher launcher = simulation.Launcher;

      // Route user input to the appropriate action
      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        simulation.HandleConfirm();
      } else if (vtroller.Left == VirtualButtonState.Pressed) {
        launcher.AdjustAngle((float)Math.PI / 64);
      } else if (vtroller.Right == VirtualButtonState.Pressed) {
        launcher.AdjustAngle((float)-Math.PI / 64);
      } else if (Keyboard.GetState().IsKeyDown(Keys.Up)) {
        launcher.AdjustMagnitude(0.25f);
      } else if (Keyboard.GetState().IsKeyDown(Keys.Down)) {
        launcher.AdjustMagnitude(-0.25f);
      }

      // TODO: Replace this restart mechanism
      if (Keyboard.GetState().IsKeyDown(Keys.R)) { // Some crude restart mechanism
        Console.WriteLine("Completely restarted.");
        simulation.Restart();
      }

      // Go back to the main menu
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

      // Draw the background.
      spriteBatch.Draw(simulation.Background, Vector2.Zero, Color.White);
      // Draw the walls.
      DrawWalls(spriteBatch);
      // Draw all the level objects.
      DrawLevelObjects(spriteBatch);

      spriteBatch.End();

      // If the game is transitioning on or off, fade it out to black.
      if (TransitionPosition > 0 || pauseAlpha > 0) {
        float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

        ScreenManager.FadeBackBufferToBlack(alpha);
      }
    }

    /// <summary>
    /// Draw the hard-coded walls.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the walls.</param>
    private void DrawWalls(SpriteBatch spriteBatch) {
      Texture2D topBottom = content.Load<Texture2D>("TopBottom");
      Texture2D sideWall = content.Load<Texture2D>("SideWall");

      // I'd rather have 5-pixel thick walls then 10-pixel thick walls, so I offset each wall
      // by 5 pixels. I could change the image... - Jorenz
      spriteBatch.Draw(topBottom, new Vector2(0, -5), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      spriteBatch.Draw(topBottom, new Vector2(0, 595), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      spriteBatch.Draw(sideWall, new Vector2(-5, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      spriteBatch.Draw(sideWall, new Vector2(795, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Draws the level objects.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch objects to use when drawing the level objects.</param>
    private void DrawLevelObjects(SpriteBatch spriteBatch) {
      // Draw the goal onto the canvas.
      simulation.Goal.Draw(spriteBatch);

      // Draw the platforms onto the canvas.
      foreach (Platform platform in simulation.Platforms) {
        platform.Draw(spriteBatch);
      }
      // Draw the treasures onto the canvas.
      foreach (Treasure treasure in simulation.Treasures) {
        treasure.Draw(spriteBatch);
      }
      // Draw the death traps onto the canvas
      foreach (DeathTrap deathTrap in simulation.DeathTraps) {
        deathTrap.Draw(spriteBatch);
      }

      // Draw the ball onto the canvas.
      simulation.Ball.Draw(spriteBatch);
      // Draw the launcher onto the canvas.
      simulation.Launcher.Draw(spriteBatch);
    }
  #endregion

  #region Test Level
    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    internal Simulation CreateTestLevel() {
      Simulation simulation = new Simulation(LevelLoader.Load("Content/TestLevel.xml", content), content);
      simulation.Background = content.Load<Texture2D>("GameScreen");

      return simulation;
    }
  #endregion 
  }
}
