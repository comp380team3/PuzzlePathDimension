//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace PuzzlePathDimension {
  class GameplayScreen : GameScreen {
    ContentManager content;
    Simulation simulation;

    SpriteFont font;

    public string LevelName { get; set; }

    float pauseAlpha;

    public GameplayScreen(TopLevelModel topLevel, string levelName)
      : base(topLevel) {
      LevelName = levelName;

      base.TransitionOnTime = TimeSpan.FromSeconds(1.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }


    /// <summary>
    /// Initializes the GamePlayScreen with a simulation already built.
    /// </summary>
    /// <param name="sim"></param>
    public GameplayScreen(TopLevelModel topLevel, Simulation sim)
      : base(topLevel) {
      base.TransitionOnTime = TimeSpan.FromSeconds(1.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
      simulation = sim;
    }

    /// <summary>
    /// Load graphics content for the game.
    /// </summary>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);

      // Create a new ContentManager so that all level data is flushed
      // from the cache after the level ends.
      if (content == null)
        content = new ContentManager(shared.ServiceProvider, "Content");

      font = shared.Load<SpriteFont>("Font/textfont");

      // Create the hard-coded level.
      if (simulation == null) {
        simulation = CreateTestLevel();
      }

      //Initialize the bodies in the simulation
      simulation.InitWorld();

      // Set up the sounds.
      SetupSoundEvents();

      // once the load has finished, we use ResetElapsedTime to tell the game's
      // timing mechanism that we have just finished a very long frame, and that
      // it should not try to catch up.
      Game.ResetElapsedTime();
    }

    /// <summary>
    /// Assign sounds to various events.
    /// </summary>
    private void SetupSoundEvents() {
      // The sounds actually take enough time to load that there's a delay when
      // the ball is launched, so cache them first.
      content.Load<SoundEffect>("Sound/launch");
      content.Load<SoundEffect>("Sound/bounce");

      // Assign the sound effects to the proper places.
      foreach (Platform plat in simulation.Platforms) {
        plat.OnPlatformCollision += PlayBounce;
      }
      simulation.OnWallCollision += PlayBounce;
      simulation.Launcher.OnBallLaunch += PlayLaunch;
    }

    /// <summary>
    /// Unload graphics content used by the game.
    /// </summary>
    public override void UnloadContent() {
      content.Unload();
    }


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

      if (simulation.CurrentState == SimulationState.Completed) {
        MessageBoxScreen completedMessageBox = new MessageBoxScreen(TopLevel, "Congratulations, Level Completed!",
                                                                    "Retry", "Main Menu", "Level Select");
        completedMessageBox.MiddleButton += MainMenuMessageBoxAccepted;
        completedMessageBox.LeftButton += ConfirmRetryBoxAccepted;
        completedMessageBox.RightButton += ConfirmLevelMessageBoxAccepted;

        ScreenList.AddScreen(completedMessageBox);
      }

      if (simulation.CurrentState == SimulationState.Failed) {
        MessageBoxScreen failedMessageBox = new MessageBoxScreen(TopLevel, "Level Failed. Please try again.",
                                                                 "Retry", "Main Menu", "Level Select");
        failedMessageBox.MiddleButton += MainMenuMessageBoxAccepted;
        failedMessageBox.LeftButton += ConfirmRetryBoxAccepted;
        failedMessageBox.RightButton += ConfirmLevelMessageBoxAccepted;

        ScreenList.AddScreen(failedMessageBox);
      }
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public override void HandleInput(VirtualController Controller) {
      base.HandleInput(Controller);

      Launcher launcher = simulation.Launcher;

      // Route user input to the appropriate action
      if (Controller.IsButtonPressed(VirtualButtons.Left)) {
        launcher.AdjustAngle((float)Math.PI / 64);
      }
      if (Controller.IsButtonPressed(VirtualButtons.Right)) {
        launcher.AdjustAngle((float)-Math.PI / 64);
      }
      if (Controller.IsButtonPressed(VirtualButtons.Up)) {
        launcher.AdjustMagnitude(0.25f);
      }
      if (Controller.IsButtonPressed(VirtualButtons.Down)) {
        launcher.AdjustMagnitude(-0.25f);
      }

      // TODO: Replace this restart mechanism
      if (Keyboard.GetState().IsKeyDown(Keys.R) ||
        GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X)) {
        Console.WriteLine("Completely restarted.");
        simulation.Restart();
      }
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    protected override void OnButtonReleased(VirtualButtons button) {
      // The game pauses either if the user presses the pause button, or if
      // they unplug the active gamepad. This requires us to keep track of
      // whether a gamepad was ever plugged in, because we don't want to pause
      // on PC if they are playing with a keyboard and have no gamepad at all!

      // Route user input to the appropriate action
      switch (button) {
      case VirtualButtons.Back:
        ScreenList.AddScreen(new PauseMenuScreen(TopLevel, simulation));
        break;
      case VirtualButtons.Confirm:
        simulation.HandleConfirm();
        break;
      }
    }

    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      spriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0, 0);

      spriteBatch.Begin();

      // Draw the background.
      spriteBatch.Draw(simulation.Background, Vector2.Zero, Color.White);
      // Draw the walls.
      DrawWalls(spriteBatch);
      // Draw all the level objects.
      DrawLevelObjects(spriteBatch);
      // Draw any informational text.
      DrawText(spriteBatch);

      spriteBatch.End();

      // If the game is transitioning on or off, fade it out to black.
      if (TransitionPosition > 0 || pauseAlpha > 0) {
        float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

        spriteBatch.FadeBackBufferToBlack(alpha);
      }
    }


    /// <summary>
    /// Draw the hard-coded walls.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the walls.</param>
    private void DrawWalls(SpriteBatch spriteBatch) {
      Texture2D wallTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
      wallTexture.SetData<Color>(new Color[] { Color.White });

      spriteBatch.Draw(wallTexture, new Rectangle(0, 0, 800, 5), null, Color.Black);
      spriteBatch.Draw(wallTexture, new Rectangle(0, 595, 800, 5), null, Color.Black);
      spriteBatch.Draw(wallTexture, new Rectangle(0, 0, 5, 600), null, Color.Black);
      spriteBatch.Draw(wallTexture, new Rectangle(795, 0, 5, 600), null, Color.Black);
    }

    /// <summary>
    /// Draws the level objects.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the level objects.</param>
    private void DrawLevelObjects(SpriteBatch spriteBatch) {
      // Draw the goal onto the canvas.
      simulation.Goal.Draw(spriteBatch);

      // Draw the platforms onto the canvas.
      foreach (Platform platform in simulation.Platforms) {
        platform.Draw(spriteBatch);
      }
      foreach (Platform platform in simulation.MoveablePlatforms) {
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

      // Draw the launcher onto the canvas.
      simulation.Launcher.Draw(spriteBatch);
      // Draw the ball onto the canvas.
      simulation.Ball.Draw(spriteBatch);
    }

    /// <summary>
    /// Draws any relevant text on the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the text.</param>
    private void DrawText(SpriteBatch spriteBatch) {
      // Draw the number of balls left.
      string attemptsText = "Balls left: " + simulation.AttemptsLeft;
      spriteBatch.DrawString(font, attemptsText, new Vector2(10f, 570f), Color.Black);
    }

    /// <summary>
    /// Plays the launcher's sound effect.
    /// </summary>
    private void PlayLaunch() {
      if (base.Prefs.PlaySounds) {
        SoundEffect launch = content.Load<SoundEffect>("Sound/launch");
        launch.Play();
      }
    }

    /// <summary>
    /// Plays the bouncing sound.
    /// </summary>
    private void PlayBounce() {
      PlayBounce(false);
    }

    /// <summary>
    /// Plays the bouncing sound. This particular overload of the method is for
    /// the PlatformTouched delegate.
    /// </summary>
    private void PlayBounce(bool breakable) {
      if (base.Prefs.PlaySounds) {
        SoundEffect bounce = content.Load<SoundEffect>("Sound/bounce");
        bounce.Play();
      }
    }


    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    internal Simulation CreateTestLevel() {
      Simulation simulation = new Simulation(LevelLoader.Load("Content/Level/" + LevelName.Replace(" ", "") + ".xml", content), content);
      simulation.Background = content.Load<Texture2D>("Texture/GameScreen");

      return simulation;
    }


    /// <summary>
    /// Event handler for when the user selects ok on the level select
    /// button on the message box. This uses the loading screen to
    /// transition from the game back to the level select screen.
    /// </summary>
    void ConfirmLevelMessageBoxAccepted() {
      LoadingScreen.Load(TopLevel, false, null, new BackgroundScreen(TopLevel), new MainMenuScreen(TopLevel), new LevelSelectScreen(TopLevel, content));
    }

    void MainMenuMessageBoxAccepted() {
      LoadingScreen.Load(TopLevel, false, null, new BackgroundScreen(TopLevel),
                                                     new MainMenuScreen(TopLevel));
    }

    /// <summary>
    /// Event handler for when the user selects the Retry menu entry.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ConfirmRetryBoxAccepted() {
      simulation.Restart();
    }
  }
}
