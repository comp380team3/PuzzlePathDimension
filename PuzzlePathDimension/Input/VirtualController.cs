using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PuzzlePathDimension {
  public enum AdapterType {
    KeyboardMouse = 0,
    Xbox360Controller
  }

  /// <summary>
  /// The VirtualButtonState enum represents the two states of a digital button.
  /// </summary>
  public enum VirtualButtonState {
    /// <summary>
    /// The button is being held down.
    /// </summary>
    Pressed,
    /// <summary>
    /// The button is not being held down.
    /// </summary>
    Released
  }

  /// <summary>
  /// The VirtualButtons enum contains all the different types of virtual buttons
  /// that the game needs.
  /// </summary>
  public enum VirtualButtons {
    /// <summary>
    /// The button that confirms something.
    /// </summary>
    Confirm = 0,
    /// <summary>
    /// The button that usually cancels something.
    /// </summary>
    Back,
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
    Right
  }

  /// <summary>
  /// The VirtualInputChange enum contains the state transitions of a digital button.
  /// </summary>
  public enum VirtualButtonChange {
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

  /// <summary>
  /// The VirtualController class provides a device-agonstic way of reading input.
  /// </summary>
  public class VirtualController {
    /// <summary>
    /// The total number of digital inputs that the virtual controller supports.
    /// </summary>
    private static readonly int DigitalInputsCount = Enum.GetNames(typeof(VirtualButtons)).Length;
    /// <summary>
    /// The adapters that are supported by the virtual controller.
    /// </summary>
    private static readonly IVirtualAdapter[] _availableAdapters = 
      { new KeyboardMouseAdapter(), new Xbox360ControllerAdapter() };

    /// <summary>
    /// The array that contains the current state of all the buttons.
    /// </summary>
    private VirtualButtonState[] _currentState;
    /// <summary>
    /// The array that contains the previous state of all the buttons.
    /// </summary>
    private VirtualButtonState[] _oldState;
    /// <summary>
    /// The array that contains any input state transitions between the current frame
    /// and the previous frame.
    /// </summary>
    private VirtualButtonChange[] _inputChanges;

    /// <summary>
    /// The current position returned by the Point input.
    /// </summary>
    private Point _currentPointState;
    /// <summary>
    /// The former position returned by the Point input.
    /// </summary>
    private Point _oldPointState;

    /// <summary>
    /// The adapter that is currently being used to send input to the VirtualController object.
    /// </summary>
    private IVirtualAdapter _activeAdapter;

    /// <summary>
    /// Gets the state of the Confirm button.
    /// </summary>
    public VirtualButtonState Confirm {
      get { return _currentState[(int)VirtualButtons.Confirm]; }
    }

    /// <summary>
    /// Gets the state of the Back button.
    /// </summary>
    public VirtualButtonState Back {
      get { return _currentState[(int)VirtualButtons.Back]; }
    }

    /// <summary>
    /// Gets the state of the Context button.
    /// </summary>
    public VirtualButtonState Context {
      get { return _currentState[(int)VirtualButtons.Context]; }
    }

    /// <summary>
    /// Gets the state of the Pause button.
    /// </summary>
    public VirtualButtonState Pause {
      get { return _currentState[(int)VirtualButtons.Pause]; }
    }

    /// <summary>
    /// Gets the state of the Up button.
    /// </summary>
    public VirtualButtonState Up {
      get { return _currentState[(int)VirtualButtons.Up]; }
    }

    /// <summary>
    /// Gets the state of the Down button.
    /// </summary>
    public VirtualButtonState Down {
      get { return _currentState[(int)VirtualButtons.Down]; }
    }

    /// <summary>
    /// Gets the state of the Left button.
    /// </summary>
    public VirtualButtonState Left {
      get { return _currentState[(int)VirtualButtons.Left]; }
    }

    /// <summary>
    /// Gets the state of the Right button.
    /// </summary>
    public VirtualButtonState Right {
      get { return _currentState[(int)VirtualButtons.Right]; }
    }

    /// <summary>
    /// Gets the current position of the Point input.
    /// </summary>
    public Point Point {
      get { return _currentPointState; }
    }

    /// <summary>
    /// Gets whether the pointing device has moved between the last frame
    /// and the current frame.
    /// </summary>
    public bool PointMoved {
      get { return !_currentPointState.Equals(_oldPointState); }
    }

    /// <summary>
    /// Gets whether the VirtualController can read input from
    /// the current adapter.
    /// </summary>
    public bool Connected {
      get { return _activeAdapter.Connected; }
    }

    /// <summary>
    /// Gets or sets the current adapter.
    /// </summary>
    public IVirtualAdapter Adapter {
      get { return _activeAdapter; }
      set { _activeAdapter = value; }
    }

    /// <summary>
    /// Gets the list of available adapters.
    /// </summary>
    /// <remarks>This is needed because a 'readonly' array only makes the
    /// reference to the array readonly, not the contents of the array.</remarks>
    public static IList<IVirtualAdapter> AvailableAdapters {
      get { return Array.AsReadOnly<IVirtualAdapter>(_availableAdapters); }
    }

    /// <summary>
    /// Constructs a VirtualController object.
    /// </summary>
    /// <param name="adapter">The VirtualAdapter object that will send input to the 
    /// VirtualController.</param>
    public VirtualController(IVirtualAdapter adapter) {
      // Initialize the three arrays with as many spots as there are digital buttons.
      _currentState = new VirtualButtonState[DigitalInputsCount];
      _oldState = new VirtualButtonState[DigitalInputsCount];
      _inputChanges = new VirtualButtonChange[DigitalInputsCount];

      // Set the input states to default values.
      for (int i = 0; i < DigitalInputsCount; i++) {
        _currentState[i] = VirtualButtonState.Released;
        _oldState[i] = VirtualButtonState.Released;
        _inputChanges[i] = VirtualButtonChange.NoChange;
      }

      // Initialize the Point input.
      _currentPointState = new Point(0, 0);
      _oldPointState = new Point(0, 0);

      // Set the current adapter.
      _activeAdapter = adapter;
    }

    /// <summary>
    /// Changes the active adapter by providing the type of adapter.
    /// </summary>
    /// <param name="type">The type of adapter to change the virtual controller to.</param>
    public void ChangeAdapter(AdapterType type) {
      _activeAdapter = _availableAdapters[(int)type];
    }

    /// <summary>
    /// Updates the state of the VirtualController object.
    /// </summary>
    public void Update() {
      // Update every possible digital input's state.
      UpdateDigital(VirtualButtons.Confirm, _activeAdapter.Confirm());
      UpdateDigital(VirtualButtons.Back, _activeAdapter.Back());
      UpdateDigital(VirtualButtons.Context, _activeAdapter.Context());
      UpdateDigital(VirtualButtons.Pause, _activeAdapter.Pause());
      UpdateDigital(VirtualButtons.Up, _activeAdapter.Up());
      UpdateDigital(VirtualButtons.Down, _activeAdapter.Down());
      UpdateDigital(VirtualButtons.Left, _activeAdapter.Left());
      UpdateDigital(VirtualButtons.Right, _activeAdapter.Right());

      // Update the Point input separately.
      _oldPointState = _currentPointState;
      _currentPointState = _activeAdapter.Point();
    }

    /// <summary>
    /// Updates a particular virtual button's state, and checks to see if there is
    /// an input state transition.
    /// </summary>
    /// <param name="type">The type of the button to update.</param>
    /// <param name="newState">The new state of the button.</param>
    private void UpdateDigital(VirtualButtons type, VirtualButtonState newState) {
      int typeIndex = (int)type; // Syntactic sugar

      // Keep track of two consecutive frames of input.
      _oldState[typeIndex] = _currentState[typeIndex];
      _currentState[typeIndex] = newState;

      // If the button was being pressed during the last frame but was
      // released this frame, change the corresponding entry in the
      // array that keeps track of input transitions.
      if (_oldState[typeIndex] == VirtualButtonState.Pressed 
        && _currentState[typeIndex] == VirtualButtonState.Released) {
        _inputChanges[typeIndex] = VirtualButtonChange.JustReleased;
        Console.WriteLine("Something was released.");
      // Same if the button was not being pressed during the last frame
      // but was released during this frame.
      } else if (_oldState[typeIndex] == VirtualButtonState.Released
        && _currentState[typeIndex] == VirtualButtonState.Pressed) {
        _inputChanges[typeIndex] = VirtualButtonChange.JustPressed;
        Console.WriteLine("Something was pressed.");
      // Or the button could have still been pressed or released.
      } else {
        _inputChanges[typeIndex] = VirtualButtonChange.NoChange;
      }
    }

    /// <summary>
    /// Checks if a button is being held down.
    /// </summary>
    /// <param name="type">The type of button to check.</param>
    /// <returns>Whether the button is being held down.</returns>
    public bool IsButtonDown(VirtualButtons type) {
      return _currentState[(int)type] == VirtualButtonState.Pressed;
    }

    /// <summary>
    /// Checks if a button is not being held down.
    /// </summary>
    /// <param name="type">The type of button to check.</param>
    /// <returns>Whether the button is not being held down.</returns>
    public bool IsButtonUp(VirtualButtons type) {
      return _currentState[(int)type] == VirtualButtonState.Released;
    }

    /// <summary>
    /// Checks if a virtual button was just released. This is similar to checking
    /// if a keyboard button was released during the current frame but pressed
    /// during the last frame.
    /// </summary>
    /// <param name="type">The type of button to check.</param>
    /// <returns>Whether the button was just released.</returns>
    public bool CheckForRecentRelease(VirtualButtons type) {
      return _inputChanges[(int)type] == VirtualButtonChange.JustReleased;
    }

    /// <summary>
    /// Checks if a virtual button was just pressed. This is similar to checking
    /// if a keyboard button was pressed during the current frame but released
    /// during the last frame.
    /// </summary>
    /// <param name="type">The type of button to check.</param>
    /// <returns>Whether the button was just pressed.</returns>
    public bool CheckForRecentPress(VirtualButtons type) {
      return _inputChanges[(int)type] == VirtualButtonChange.JustPressed;
    }
  }
}
