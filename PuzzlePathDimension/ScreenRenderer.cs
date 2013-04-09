//-----------------------------------------------------------------------------
// ScreenManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Collections.Generic;
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

  public interface IScreenList {
    void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer);
    void RemoveScreen(GameScreen screen);

    GameScreen[] GetScreens();
  }

  /// <summary>
  /// The screen manager is a component which manages one or more GameScreen
  /// instances. It maintains a stack of screens, calls their Update and Draw
  /// methods at the appropriate times, and automatically routes input to the
  /// topmost active screen.
  /// </summary>
  public class ScreenRenderer : DrawableGameComponent, IScreenList {
    /* Fields */
    // The list of screens that will receive Update and Draw events.
    List<GameScreen> screens = new List<GameScreen>();

    // The list of screens currently being updated.
    // This needs to be a field, not a local, because screens may be removed
    //   during the update process, and removed screens shouldn't be updated.
    List<GameScreen> screensToUpdate = new List<GameScreen>();

    /// <summary>
    /// The VirtualController object that provides input to the Screen objects.
    /// </summary>
    VirtualController vtroller = new VirtualController(new KeyboardMouseAdapter());

    // The rendering device that all screens share.
    SpriteBatch spriteBatch;

    // Some shared fonts that all screens can use.
    SpriteFont font;
    SpriteFont textFont;

    bool hasDevice; // has the graphics device been initialized?
    bool traceEnabled; // do we want to output debugging information?

    /* Properties */
    /// <summary>
    /// A default font shared by all the screens. This saves
    /// each screen having to bother loading their own local copy.
    /// </summary>
    public SpriteFont Font {
      get { return font; }
    }

    /// <summary>
    /// A default font shared by all the screens. This is to have
    /// a font the can be used to write general information to each screen.
    /// </summary>
    public SpriteFont TextFont {
      get { return textFont; }
    }

    /// <summary>
    /// If true, the manager prints out a list of all the screens
    /// each time it is updated. This can be useful for making sure
    /// everything is being added and removed at the right times.
    /// </summary>
    public bool TraceEnabled {
      get { return traceEnabled; }
      set { traceEnabled = value; }
    }

    /* Initialization */
    /// <summary>
    /// Constructs a new screen manager component.
    /// </summary>
    public ScreenRenderer(Game game)
      : base(game) {
    }

    /// <summary>
    /// Initializes the screen manager component.
    /// </summary>
    public override void Initialize() {
      base.Initialize();

      hasDevice = true;
    }

    /// <summary>
    /// Load your graphics content.
    /// </summary>
    protected override void LoadContent() {
      // Load content belonging to the screen manager.
      ContentManager content = Game.Content;

      spriteBatch = new SpriteBatch(GraphicsDevice);
      font = content.Load<SpriteFont>("menufont");

      // Load the text font used for writing on the menu
      textFont = content.Load<SpriteFont>("textfont");

      // Tell each of the screens to load their content.
      foreach (GameScreen screen in screens) {
        screen.LoadContent(content);
      }
    }

    /// <summary>
    /// Unload your graphics content.
    /// </summary>
    protected override void UnloadContent() {
      // Tell each of the screens to unload their content.
      foreach (GameScreen screen in screens) {
        screen.UnloadContent();
      }
    }

    /* Update & Draw */
    /// <summary>
    /// Allows each screen to run logic.
    /// </summary>
    public override void Update(GameTime gameTime) {
      // Read the keyboard and gamepad.
      vtroller.Update();

      // Make a copy of the master screen list, to avoid confusion if
      // the process of updating one screen adds or removes others.
      screensToUpdate.Clear();
      foreach (GameScreen screen in screens)
        screensToUpdate.Add(screen);

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
          // If this is the first active screen we came across,
          // give it a chance to handle input.
          if (!otherScreenHasFocus) {
            screen.HandleInput(vtroller);
            otherScreenHasFocus = true;
          }

          // If this is an active non-popup, inform any subsequent
          // screens that they are covered by it.
          if (!screen.IsPopup) {
            coveredByOtherScreen = true;
          }
        }
      }

      // Print debug trace?
      if (traceEnabled)
        TraceScreens();
    }

    /// <summary>
    /// Prints a list of all the screens, for debugging.
    /// </summary>
    void TraceScreens() {
      List<string> screenNames = new List<string>();

      foreach (GameScreen screen in screens)
        screenNames.Add(screen.GetType().Name);

      Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
    }

    /// <summary>
    /// Tells each screen to draw itself.
    /// </summary>
    public override void Draw(GameTime gameTime) {
      foreach (GameScreen screen in screens) {
        if (screen.ScreenState == ScreenState.Hidden)
          continue;

        screen.Draw(gameTime, spriteBatch);
      }
    }

    /* Public Methods */
    /// <summary>
    /// Adds a new screen to the screen manager.
    /// </summary>
    public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer) {
      screen.ControllingPlayer = controllingPlayer;
      screen.ScreenManager = this;
      screen.IsExiting = false;

      // If we have a graphics device, tell the screen to load content.
      if (hasDevice) {
        screen.LoadContent(Game.Content);
      }

      screens.Add(screen);
    }

    /// <summary>
    /// Removes a screen from the screen manager. You should normally
    /// use GameScreen.ExitScreen instead of calling this directly, so
    /// the screen can gradually transition off rather than just being
    /// instantly removed.
    /// </summary>
    public void RemoveScreen(GameScreen screen) {
      // If we have a graphics device, tell the screen to unload content.
      if (hasDevice) {
        screen.UnloadContent();
      }

      screens.Remove(screen);
      screensToUpdate.Remove(screen); // in case a screen is removed during Update().
    }

    /// <summary>
    /// Expose an array holding all the screens. We return a copy rather
    /// than the real master list, because screens should only ever be added
    /// or removed using the AddScreen and RemoveScreen methods.
    /// </summary>
    public GameScreen[] GetScreens() {
      return screens.ToArray();
    }
  }
}
