using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  public enum VirtualButtonState {
    Pressed,
    Released
  }

  public class VirtualController {
    private VirtualButtonState _confirm;
    private VirtualButtonState _back;
    private VirtualButtonState _up;
    private VirtualButtonState _down;
    private VirtualButtonState _left;
    private VirtualButtonState _right;

    private Point _currentPointState;
    private Point _oldPointState;

    private List<VirtualAdapter> _adapters;

    public VirtualButtonState Confirm {
      get { return _confirm; }
    }

    public VirtualButtonState Back {
      get { return _back; }
    }

    public VirtualButtonState Up {
      get { return _up; }
    }

    public VirtualButtonState Down {
      get { return _down; }
    }

    public VirtualButtonState Left {
      get { return _left; }
    }

    public VirtualButtonState Right {
      get { return _right; }
    }

    public Point Point {
      get { return _currentPointState; }
    }

    public bool PointMoved {
      get { return _currentPointState.Equals(_oldPointState); }
    }

    public VirtualController() {
      _confirm = VirtualButtonState.Released;
      _back = VirtualButtonState.Released;
      _up = VirtualButtonState.Released;
      _down = VirtualButtonState.Released;
      _left = VirtualButtonState.Released;
      _right = VirtualButtonState.Released;

      _currentPointState = new Point(0, 0);
      _oldPointState = new Point(0, 0);

      _adapters = new List<VirtualAdapter>();
    }

    public void Update() {
      foreach (VirtualAdapter adapter in _adapters) {
        // TODO: there's a subtle bug here; one adapter can undo the effects
        // of another adapter. I think it's best to favor pressed events since
        // buttons are unpressed 99% of the time.
        _confirm = adapter.Confirm();
        _back = adapter.Back();
        _up = adapter.Up();
        _down = adapter.Down();
        _left = adapter.Left();
        _right = adapter.Right();

        _oldPointState = _currentPointState;
        _currentPointState = adapter.Point();
      }
    }

    public void RegisterAdapter(VirtualAdapter adapter) {
      _adapters.Add(adapter);
    }

    public void UnregisterAdapter(VirtualAdapter adapter) {
      _adapters.Remove(adapter);
    }
  }
}
