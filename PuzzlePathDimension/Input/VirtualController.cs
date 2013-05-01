using System;
using Microsoft.Xna.Framework;

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

  public struct VirtualControllerState {
    public bool IsConnected { get; set; }

    public Point Point { get; set; }
    public bool Up { get; set; }
    public bool Down { get; set; }
    public bool Left { get; set; }
    public bool Right { get; set; }

    public bool Select { get; set; }
    public bool Delete { get; set; }
    public bool Context { get; set; }
    public bool Mode { get; set; }
    public bool Pause { get; set; }
    public bool Debug { get; set; }

    public bool Easter { get; set; }

    public bool IsButtonPressed(VirtualButtons type) {
      switch (type) {
      case VirtualButtons.Up:
        return Up;
      case VirtualButtons.Down:
        return Down;
      case VirtualButtons.Left:
        return Left;
      case VirtualButtons.Right:
        return Right;
      case VirtualButtons.Select:
        return Select;
      case VirtualButtons.Delete:
        return Delete;
      case VirtualButtons.Context:
        return Context;
      case VirtualButtons.Mode:
        return Mode;
      case VirtualButtons.Pause:
        return Pause;
      case VirtualButtons.Debug:
        return Debug;
      case VirtualButtons.Easter:
        return Easter;
      default:
        throw new ArgumentException("Invalid valud for type: " + type.ToString());
      }
    }
  }

  public interface VirtualController {
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

    public event Action<InputType> InputTypeChanged;

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
      set { point = value; }
    }

    public bool IsButtonPressed(VirtualButtons button) {
      return buttons[(int)button];
    }
    public void SetButtonState(VirtualButtons button, bool state) {
      buttons[(int)button] = state;
    }

    /// <summary>
    /// Gets whether the VirtualController can read input from
    /// the current adapter.
    /// </summary>
    public bool IsConnected {
      get { return connected; }
      set { connected = value; }
    }
  }
}
