using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  public interface IScreenList : IEnumerable<GameScreen> {
    void AddScreen(GameScreen screen);
    void RemoveScreen(GameScreen screen);
  }

  public class Scene : IScreenList {
    public event Action<GameScreen> ScreenAdded;
    public event Action<GameScreen> ScreenRemoved;

    // The list of screens in the scene.
    List<GameScreen> screens = new List<GameScreen>();

    /// <summary>
    /// Adds a new screen to the screen manager.
    /// </summary>
    public void AddScreen(GameScreen screen) {
      screens.Add(screen);

      if (ScreenAdded != null)
        ScreenAdded(screen);
    }

    /// <summary>
    /// Removes a screen from the screen manager. You should normally
    /// use GameScreen.ExitScreen instead of calling this directly, so
    /// the screen can gradually transition off rather than just being
    /// instantly removed.
    /// </summary>
    public void RemoveScreen(GameScreen screen) {
      screens.Remove(screen);

      if (ScreenRemoved != null)
        ScreenRemoved(screen);
    }

    // for for-in support
    public IEnumerator<GameScreen> GetEnumerator() {
      return screens.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}