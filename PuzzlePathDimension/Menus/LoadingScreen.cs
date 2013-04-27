//-----------------------------------------------------------------------------
// LoadingScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  /// <summary>
  /// The loading screen coordinates transitions between the menu system and the
  /// game itself. Normally one screen will transition off at the same time as
  /// the next screen is transitioning on, but for larger transitions that can
  /// take a longer time to load their data, we want the menu system to be entirely
  /// gone before we start loading the game. This is done as follows:
  /// 
  /// - Tell all the existing screens to transition off.
  /// - Activate a loading screen, which will transition on at the same time.
  /// - The loading screen watches the state of the previous screens.
  /// - When it sees they have finished transitioning off, it activates the real
  ///   next screen, which may take a long time to load its data. The loading
  ///   screen will be the only thing displayed while this load is taking place.
  /// </summary>
  class LoadingScreen : GameScreen {
    bool loadingIsSlow;
    bool otherScreensAreGone;

    GameScreen[] screensToLoad;

    SpriteFont font;

    /// <summary>
    /// The constructor is private: loading screens should
    /// be activated via the static Load method instead.
    /// </summary>
    private LoadingScreen(TopLevelModel topLevel, bool loadingIsSlow, GameScreen[] screensToLoad)
      : base(topLevel) {
      this.loadingIsSlow = loadingIsSlow;
      this.screensToLoad = screensToLoad;

      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);

      font = shared.Load<SpriteFont>("Font/menufont");
    }


    /// <summary>
    /// Activates the loading screen.
    /// </summary>
    public static void Load(TopLevelModel topLevel, bool loadingIsSlow, params GameScreen[] screensToLoad) {
      // Tell all the current screens to transition off.
      foreach (GameScreen screen in topLevel.Scene)
        screen.ExitScreen();

      // Create and activate the loading screen.
      topLevel.Scene.AddScreen(new LoadingScreen(topLevel, loadingIsSlow, screensToLoad));
    }


    /// <summary>
    /// Updates the loading screen.
    /// </summary>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                   bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      // If all the previous screens have finished transitioning
      // off, it is time to actually perform the load.
      if (otherScreensAreGone) {
        ScreenList.RemoveScreen(this);

        foreach (GameScreen screen in screensToLoad) {
          if (screen != null) {
            ScreenList.AddScreen(screen);
          }
        }

        // Once the load has finished, we use ResetElapsedTime to tell
        // the  game timing mechanism that we have just finished a very
        // long frame, and that it should not try to catch up.
        Game.ResetElapsedTime();
      }
    }


    /// <summary>
    /// Draws the loading screen.
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      // If we are the only active screen, that means all the previous screens
      // must have finished transitioning off. We check for this in the Draw
      // method, rather than in Update, because it isn't enough just for the
      // screens to be gone: in order for the transition to look good we must
      // have actually drawn a frame without them before we perform the load.
      bool onlyLoadingScreenLeft = ScreenList.All((screen) => screen == this);
      if ((ScreenState == ScreenState.Active) && onlyLoadingScreenLeft) {
        otherScreensAreGone = true;
      }

      // The gameplay screen takes a while to load, so we display a loading
      // message while that is going on, but the menus load very quickly, and
      // it would look silly if we flashed this up for just a fraction of a
      // second while returning from the game to the menus. This parameter
      // tells us how long the loading is going to take, so we know whether
      // to bother drawing the message.
      if (loadingIsSlow) {
        Viewport viewport = spriteBatch.GraphicsDevice.Viewport;

        const string message = "Loading...";

        // Center the text in the viewport.
        Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
        Vector2 textSize = font.MeasureString(message);
        Vector2 textPosition = (viewportSize - textSize) / 2;

        Color color = Color.White * TransitionAlpha;

        // Draw the text.
        spriteBatch.Begin();
        spriteBatch.DrawString(font, message, textPosition, color);
        spriteBatch.End();
      }
    }
  }
}
