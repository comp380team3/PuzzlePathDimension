using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  public interface IScreenList {
    void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer);
    void RemoveScreen(GameScreen screen);

    GameScreen[] GetScreens();
  }

  public class Scene : IScreenList {
    ScreenRenderer screenRenderer;

    // The list of screens that will receive Update and Draw events.
    List<GameScreen> screens = new List<GameScreen>();

    // The list of screens currently being updated.
    // This needs to be a field, not a local, because screens may be removed
    //   during the update process, and removed screens shouldn't be updated.
    List<GameScreen> screensToUpdate = new List<GameScreen>();

    public bool TraceEnabled { get; set; }

    public Scene(ScreenRenderer screenRenderer) {
      this.screenRenderer = screenRenderer;
    }

    public void LoadContent(ContentManager shared) {
      // Tell each of the screens to load their content.
      foreach (GameScreen screen in screens) {
        screen.LoadContent(shared);
      }
    }

    public void UnloadContent() {
      // Tell each of the screens to unload their content.
      foreach (GameScreen screen in screens) {
        screen.UnloadContent();
      }
    }

    public void Update(GameTime gameTime, VirtualController vtroller, bool hasFocus) {
      // Make a copy of the master screen list, to avoid confusion if
      // the process of updating one screen adds or removes others.
      screensToUpdate.Clear();
      screensToUpdate.AddRange(screens);

      bool otherScreenHasFocus = !hasFocus;
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
      if (TraceEnabled)
        TraceScreens();
    }

    /// <summary>
    /// Prints a list of all the screens, for debugging.
    /// </summary>
    void TraceScreens() {
      IEnumerable<string> screenNames = screens.Select((screen) => screen.GetType().Name);

      Debug.WriteLine(string.Join(", ", screenNames));
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      foreach (GameScreen screen in screens) {
        if (screen.ScreenState == ScreenState.Hidden)
          continue;

        screen.Draw(gameTime, spriteBatch);
      }
    }


    /// <summary>
    /// Adds a new screen to the screen manager.
    /// </summary>
    public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer) {
      screen.ControllingPlayer = controllingPlayer;
      screen.ScreenManager = screenRenderer;
      screen.IsExiting = false;

      // If we have a graphics device, tell the screen to load content.
      if (screenRenderer.HasDevice) {
        screen.LoadContent(screenRenderer.Game.Content);
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
      if (screenRenderer.HasDevice) {
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