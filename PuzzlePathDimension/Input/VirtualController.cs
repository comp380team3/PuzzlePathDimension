using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  public enum DigitalButtonState {
    Pressed,
    Released
  }

  public enum DigitalInputs {
    Confirm = 0,
    Back,
    Up,
    Down,
    Left,
    Right
  }

  public enum DigitalInputChange {
    /// <summary>
    /// There was no change in the input state of that button.
    /// </summary>
    NoChange,
    /// <summary>
    /// On the previous frame, the input was released, but on this frame, it was pressed.
    /// </summary>
    JustPressed,
    /// <summary>
    /// On the previous frame, the input was pressed, but on this frame, it was released.
    /// </summary>
    JustReleased
  }

  public class VirtualController {
    private static readonly int DigitalInputsCount = Enum.GetNames(typeof(DigitalInputs)).Length;

    private DigitalButtonState[] _currentState;
    private DigitalButtonState[] _oldState;
    private DigitalInputChange[] _inputChanges;

    private Point _currentPointState;
    private Point _oldPointState;

    private VirtualAdapter _activeAdapter;

    public DigitalButtonState Confirm {
      get { return _currentState[(int)DigitalInputs.Confirm]; }
    }

    public DigitalButtonState Back {
      get { return _currentState[(int)DigitalInputs.Back]; }
    }

    public DigitalButtonState Up {
      get { return _currentState[(int)DigitalInputs.Up]; }
    }

    public DigitalButtonState Down {
      get { return _currentState[(int)DigitalInputs.Down]; }
    }

    public DigitalButtonState Left {
      get { return _currentState[(int)DigitalInputs.Left]; }
    }

    public DigitalButtonState Right {
      get { return _currentState[(int)DigitalInputs.Right]; }
    }

    public Point Point {
      get { return _currentPointState; }
    }

    public bool PointMoved {
      get { return _currentPointState.Equals(_oldPointState); }
    }

    public VirtualAdapter Adapter {
      get { return _activeAdapter; }
      set { _activeAdapter = value; }
    }

    public VirtualController(VirtualAdapter adapter) {
      _currentState = new DigitalButtonState[DigitalInputsCount];
      _oldState = new DigitalButtonState[DigitalInputsCount];
      _inputChanges = new DigitalInputChange[DigitalInputsCount];

      for (int i = 0; i < DigitalInputsCount; i++) {
        _currentState[i] = DigitalButtonState.Released;
        _oldState[i] = DigitalButtonState.Released;
        _inputChanges[i] = DigitalInputChange.NoChange;
      }

      _currentPointState = new Point(0, 0);
      _oldPointState = new Point(0, 0);

      _activeAdapter = adapter;
    }

    public void Update() {
      // Update every possible digital input type.
      UpdateDigital(DigitalInputs.Confirm, _activeAdapter.Confirm());
      UpdateDigital(DigitalInputs.Back, _activeAdapter.Back());
      UpdateDigital(DigitalInputs.Up, _activeAdapter.Up());
      UpdateDigital(DigitalInputs.Down, _activeAdapter.Down());
      UpdateDigital(DigitalInputs.Left, _activeAdapter.Left());
      UpdateDigital(DigitalInputs.Right, _activeAdapter.Right());

      // Update the Point input separately.
      _oldPointState = _currentPointState;
      _currentPointState = _activeAdapter.Point();
    }

    private void UpdateDigital(DigitalInputs type, DigitalButtonState newState) {
      int typeIndex = (int)type;

      _oldState[typeIndex] = _currentState[typeIndex];
      _currentState[typeIndex] = newState;

      if (_oldState[typeIndex] == DigitalButtonState.Pressed 
        && _currentState[typeIndex] == DigitalButtonState.Released) {
        _inputChanges[typeIndex] = DigitalInputChange.JustReleased;
        Console.WriteLine("Something was released.");

      } else if (_oldState[typeIndex] == DigitalButtonState.Released
        && _currentState[typeIndex] == DigitalButtonState.Pressed) {
        _inputChanges[typeIndex] = DigitalInputChange.JustPressed;
        Console.WriteLine("Something was pressed.");

      } else {
        _inputChanges[typeIndex] = DigitalInputChange.NoChange;
      }
    }

    public bool CheckForRecentRelease(DigitalInputs type) {
      return _inputChanges[(int)type] == DigitalInputChange.JustReleased;
    }

    public bool CheckForRecentPress(DigitalInputs type) {
      return _inputChanges[(int)type] == DigitalInputChange.JustPressed;
    }
  }
}
