using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PuzzlePathDimension {
  /// <summary>
  /// The VirtualButtons enum contains all the different types of virtual buttons
  /// that the game needs.
  /// </summary>
  public enum VirtualButtons {
    /// <summary>
    /// The button that confirms something.
    /// </summary>
    Select = 0,
    /// <summary>
    /// The button that usually cancels something.
    /// </summary>
    Delete,
    /// <summary>
    /// The button that has different functionality depending on the current
    /// state of the game.
    /// </summary>
    Context,
    /// <summary>
    /// The button that pauses the game.
    /// </summary>
    Pause,
    /// <summary>
    /// The button that switches between modes.
    /// </summary>
    Mode,
    /// <summary>
    /// The button that exposes debugging functionality.
    /// </summary>
    Debug,
    /// <summary>
    /// The button that does something silly.
    /// </summary>
    Easter,
    /// <summary>
    /// The button that represents the up direction.
    /// </summary>
    Up,
    /// <summary>
    /// The button that represents the down direction.
    /// </summary>
    Down,
    /// <summary>
    /// The button that represents the left direction.
    /// </summary>
    Left,
    /// <summary>
    /// The button that represents the right direction.
    /// </summary>
    Right,
  }

  public interface VirtualController {
    event Action Connected;
    event Action Disconnected;
    event Action<VirtualButtons> ButtonPressed;
    event Action<VirtualButtons> ButtonReleased;
    event Action<Point> PointChanged;

    InputType InputType { get; set; }
    bool IsConnected { get; }
    Point Point { get; }

    bool IsButtonPressed(VirtualButtons type);
  }

  /// <summary>
  /// The VirtualController class provides a device-agonstic way of reading input.
  /// </summary>
  public class WritableVirtualController : VirtualController {
    private static readonly int ButtonCount = Enum.GetNames(typeof(VirtualButtons)).Length;

    public event Action Connected;
    public event Action Disconnected;
    public event Action<InputType> InputTypeChanged;

    public event Action<VirtualButtons> ButtonPressed;
    public event Action<VirtualButtons> ButtonReleased;
    public event Action<Point> PointChanged;

    private bool connected = false;
    private InputType inputType = InputType.KeyboardMouse;
    private Point point = new Point(0, 0);
    private bool[] buttons = new bool[ButtonCount];

    public InputType InputType {
      get { return inputType; }
      set {
        if (inputType == value)
          return;
        inputType = value;

        if (InputTypeChanged != null)
          InputTypeChanged(value);
      }
    }

    /// <summary>
    /// Gets the current position of the Point input.
    /// </summary>
    public Point Point {
      get { return point; }
      set {
        if (point == value)
          return;
        point = value;

        if (PointChanged != null)
          PointChanged(value);
      }
    }

    public bool IsButtonPressed(VirtualButtons button) {
      return buttons[(int)button];
    }
    public void SetButtonState(VirtualButtons button, bool state) {
      int type = (int)button;

      if (buttons[type] == state)
        return;
      buttons[type] = state;

      if (state) {
        Console.WriteLine("Something was pressed.");
        if (ButtonPressed != null)
          ButtonPressed(button);
      } else {
        Console.WriteLine("Something was released.");
        if (ButtonReleased != null)
          ButtonReleased(button);
      }
    }

    /// <summary>
    /// Gets whether the VirtualController can read input from
    /// the current adapter.
    /// </summary>
    public bool IsConnected {
      get { return connected; }
      set {
        if (connected == value)
          return;
        connected = value;

        if (connected) {
          if (Connected != null)
            Connected();
        } else {
          if (Disconnected != null)
            Disconnected();
        }
      }
    }
  }
}
