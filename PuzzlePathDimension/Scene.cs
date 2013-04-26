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

    public Scene(WritableVirtualController controller) {
      // TODO: factor this out of Scene
      controller.ButtonPressed += OnButtonPressed;
      controller.ButtonReleased += OnButtonReleased;
      controller.Connected += OnConnected;
      controller.Disconnected += OnDisconnected;
      controller.PointChanged += OnPointChanged;
    }

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

    /* Input-event proxying to screen scoped controllers */

    private void OnButtonPressed(VirtualButtons button) {
      WithActiveScreen((screen) => {
        screen.Controller.SetButtonState(button, true);
      });
    }

    private void OnButtonReleased(VirtualButtons button) {
      WithActiveScreen((screen) => {
        screen.Controller.SetButtonState(button, false);
      });
    }

    private void OnConnected() {
      WithActiveScreen((screen) => {
        screen.Controller.IsConnected = true;
      });
    }

    private void OnDisconnected() {
      WithActiveScreen((screen) => {
        screen.Controller.IsConnected = false;
      });
    }

    private void OnPointChanged(Point point) {
      WithActiveScreen((screen) => {
        screen.Controller.Point = point;
      });
    }

    // Helper method used in the event callbacks
    private void WithActiveScreen(Action<GameScreen> action) {
      GameScreen activeScreen = screens.Last((screen) =>
        screen.ScreenState == ScreenState.TransitionOn ||
        screen.ScreenState == ScreenState.Active);

      action(activeScreen);
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