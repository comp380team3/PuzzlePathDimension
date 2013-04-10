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
  public static class SpriteBatchExtensions {
    /// <summary>
    /// Helper draws a translucent black fullscreen sprite, used for fading
    /// screens in and out, and for darkening the background behind popups.
    /// </summary>
    public static void FadeBackBufferToBlack(this SpriteBatch spriteBatch, float alpha) {
      GraphicsDevice device = spriteBatch.GraphicsDevice;
      Viewport viewport = device.Viewport;

      Texture2D blankTexture = new Texture2D(device, 1, 1);
      blankTexture.SetData<Color>(new Color[] { Color.White });

      spriteBatch.Begin();

      spriteBatch.Draw(blankTexture,
                       new Rectangle(0, 0, viewport.Width, viewport.Height),
                       Color.Black * alpha);

      spriteBatch.End();
    }
  }

  /// <summary>
  /// The screen manager is a component which manages one or more GameScreen
  /// instances. It maintains a stack of screens, calls their Update and Draw
  /// methods at the appropriate times, and automatically routes input to the
  /// topmost active screen.
  /// </summary>
  public class ScreenRenderer : DrawableGameComponent, IScreenList {
    Scene scene;

    /// <summary>
    /// The VirtualController object that provides input to the Screen objects.
    /// </summary>
    VirtualController vtroller = new VirtualController(new KeyboardMouseAdapter());

    // The rendering device that all screens share.
    SpriteBatch spriteBatch;

    // has the graphics device been initialized?
    public bool HasDevice { get; private set; }


    /// <summary>
    /// Constructs a new screen manager component.
    /// </summary>
    public ScreenRenderer(Game game)
        : base(game) {
      scene = new Scene(this);
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

      scene.LoadContent(Game.Content);
    }

    /// <summary>
    /// Unload your graphics content.
    /// </summary>
    protected override void UnloadContent() {
      scene.UnloadContent();
    }

    /// <summary>
    /// Allows each screen to run logic.
    /// </summary>
    public override void Update(GameTime gameTime) {
      // Read the keyboard and gamepad.
      vtroller.Update();

      scene.Update(gameTime, vtroller, Game.IsActive);
    }

    /// <summary>
    /// Tells each screen to draw itself.
    /// </summary>
    public override void Draw(GameTime gameTime) {
      scene.Draw(gameTime, spriteBatch);
    }

    /// <summary>
    /// Adds a new screen to the screen manager.
    /// </summary>
    public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer) {
      scene.AddScreen(screen, controllingPlayer);
    }

    /// <summary>
    /// Removes a screen from the screen manager. You should normally
    /// use GameScreen.ExitScreen instead of calling this directly, so
    /// the screen can gradually transition off rather than just being
    /// instantly removed.
    /// </summary>
    public void RemoveScreen(GameScreen screen) {
      scene.RemoveScreen(screen);
    }

    /// <summary>
    /// Expose an array holding all the screens. We return a copy rather
    /// than the real master list, because screens should only ever be added
    /// or removed using the AddScreen and RemoveScreen methods.
    /// </summary>
    public GameScreen[] GetScreens() {
      return scene.GetScreens();
    }
  }
}
