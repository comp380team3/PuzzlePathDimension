//-----------------------------------------------------------------------------
// ScreenManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The screen manager is a component which manages one or more GameScreen
  /// instances. It maintains a stack of screens, calls their Update and Draw
  /// methods at the appropriate times, and automatically routes input to the
  /// topmost active screen.
  /// </summary>
  public class RenderComponent : DrawableGameComponent {
    // The scene containing all current screens.
    public Scene Scene { get; private set; }

    // The rendering device that all screens share.
    private SpriteBatch spriteBatch;

    // has the graphics device been initialized?
    public bool HasDevice { get; private set; }

    // The list of screens currently being updated.
    // This needs to be a field, not a local, because screens may be removed
    //   during the update process, and removed screens shouldn't be updated.
    private List<GameScreen> screensToUpdate = new List<GameScreen>();


    /// <summary>
    /// Constructs a new screen manager component.
    /// </summary>
    public RenderComponent(Game game, Scene scene)
      : base(game) {
      Scene = scene;

      Scene.ScreenAdded += OnScreenAdded;
      Scene.ScreenRemoved += OnScreenRemoved;
    }

    /// <summary>
    /// Initializes the screen manager component.
    /// </summary>
    public override void Initialize() {
      base.Initialize();

      HasDevice = true;
    }

    /// <summary>
    /// Load your graphics content.
    /// </summary>
    protected override void LoadContent() {
      base.LoadContent();

      spriteBatch = new SpriteBatch(GraphicsDevice);

      foreach (GameScreen screen in Scene)
        screen.LoadContent(Game.Content);
    }

    /// <summary>
    /// Unload your graphics content.
    /// </summary>
    protected override void UnloadContent() {
      foreach (GameScreen screen in Scene)
        screen.UnloadContent();
    }

    /// <summary>
    /// Allows each screen to run logic.
    /// </summary>
    public override void Update(GameTime gameTime) {
      screensToUpdate.Clear();
      screensToUpdate.AddRange(Scene);

      bool otherScreenHasFocus = !Game.IsActive;
      bool coveredByOtherScreen = false;

      // Loop as long as there are screens waiting to be updated.
      while (screensToUpdate.Count > 0) {
        // Pop the topmost screen off the waiting list.
        GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];
        screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

        // Update the screen.
        screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        if (screen.ScreenState == ScreenState.TransitionOn ||
            screen.ScreenState == ScreenState.Active) {
          otherScreenHasFocus = true;

          // If this is an active non-popup, inform any subsequent
          // screens that they are covered by it.
          if (!screen.IsPopup)
            coveredByOtherScreen = true;
        }
      }
    }

    /// <summary>
    /// Tells each screen to draw itself.
    /// </summary>
    public override void Draw(GameTime gameTime) {
      foreach (GameScreen screen in Scene) {
        if (screen.ScreenState == ScreenState.Hidden)
          continue;

        screen.Draw(gameTime, spriteBatch);
      }
    }


    private void OnScreenAdded(GameScreen screen) {
      // If we have a graphics device, tell the screen to load content.
      if (HasDevice)
        screen.LoadContent(Game.Content);
    }

    private void OnScreenRemoved(GameScreen screen) {
      // If we have a graphics device, tell the screen to unload content.
      if (HasDevice)
        screen.UnloadContent();

      screensToUpdate.Remove(screen);
    }
  }
}
