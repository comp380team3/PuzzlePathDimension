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
  public class ScreenRenderer : DrawableGameComponent {
    /// <summary>
    /// The VirtualController object that provides input to the Screen objects.
    /// </summary>
    VirtualController vtroller = new VirtualController(new KeyboardMouseAdapter());

    // The rendering device that all screens share.
    SpriteBatch spriteBatch;

    // has the graphics device been initialized?
    public bool HasDevice { get; private set; }

    public Scene Scene { get; private set; }


    /// <summary>
    /// Constructs a new screen manager component.
    /// </summary>
    public ScreenRenderer(Game game)
        : base(game) {
      Scene = new Scene(this);
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

      Scene.LoadContent(Game.Content);
    }

    /// <summary>
    /// Unload your graphics content.
    /// </summary>
    protected override void UnloadContent() {
      Scene.UnloadContent();
    }

    /// <summary>
    /// Allows each screen to run logic.
    /// </summary>
    public override void Update(GameTime gameTime) {
      Scene.Update(gameTime, vtroller, Game.IsActive);
    }

    /// <summary>
    /// Tells each screen to draw itself.
    /// </summary>
    public override void Draw(GameTime gameTime) {
      Scene.Draw(gameTime, spriteBatch);
    }
  }
}
