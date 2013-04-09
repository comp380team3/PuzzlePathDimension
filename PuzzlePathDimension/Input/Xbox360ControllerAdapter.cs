using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  public class Xbox360ControllerAdapter : IVirtualAdapter {
    /// <summary>
    /// Gets whether the Xbox 360 controller is connected.
    /// </summary>
    public bool Connected {
      get { return GamePad.GetState(PlayerIndex.One).IsConnected; }
    }

    /// <summary>
    /// Constructs an Xbox360ControllerAdapter object.
    /// </summary>
    public Xbox360ControllerAdapter() { }

    /// <summary>
    /// Defines the controller input that corresponds to the Confirm input.
    /// </summary>
    /// <returns>The state of the controller input that is mapped to the Confirm input.</returns>
    public VirtualButtonState Confirm() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) ? 
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the controller input that corresponds to the Back input.
    /// </summary>
    /// <returns>The state of the controller input that is mapped to the Back input.</returns>
    public VirtualButtonState Back() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the controller input that corresponds to the Context input.
    /// </summary>
    /// <returns>The state of the controller input that is mapped to the Context input.</returns>
    public VirtualButtonState Context() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the controller input that corresponds to the Pause input.
    /// </summary>
    /// <returns>The state of the controller input that is mapped to the Pause input.</returns>
    public VirtualButtonState Pause() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the controller input that corresponds to the Up input.
    /// </summary>
    /// <returns>The state of the controller input that is mapped to the Up input.</returns>
    public VirtualButtonState Up() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || 
        GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the controller input that corresponds to the Down input.
    /// </summary>
    /// <returns>The state of the controller input that is mapped to the Down input.</returns>
    public VirtualButtonState Down() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || 
        GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the controller input that corresponds to the Left input.
    /// </summary>
    /// <returns>The state of the controller input that is mapped to the Left input.</returns>
    public VirtualButtonState Left() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadLeft) || 
        GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickLeft) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the controller input that corresponds to the Right input.
    /// </summary>
    /// <returns>The state of the controller input that is mapped to the Right input.</returns>
    public VirtualButtonState Right() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadRight) || 
        GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickRight) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the controller input that corresponds to the Point input.
    /// </summary>
    /// <returns>The current position of the Point input.</returns>
    public Point Point() {
      // TODO: implement the Point input
      return new Point(0, 0);
    }
  }
}
