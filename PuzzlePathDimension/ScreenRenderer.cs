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
  public class ScreenRenderer : DrawableGameComponent {
    private TopLevelModel TopLevel { get; set; }

    // The rendering device that all screens share.
    SpriteBatch spriteBatch;

    // has the graphics device been initialized?
    public bool HasDevice { get; private set; }

    public Scene Scene { get; private set; }


    /// <summary>
    /// Constructs a new screen manager component.
    /// </summary>
    public ScreenRenderer(Game game, TopLevelModel toplevel)
      : base(game) {
      TopLevel = toplevel;
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
      Scene.Update(gameTime, TopLevel.Controller, Game.IsActive);
    }

    /// <summary>
    /// Tells each screen to draw itself.
    /// </summary>
    public override void Draw(GameTime gameTime) {
      Scene.Draw(gameTime, spriteBatch);
    }
  }
}
