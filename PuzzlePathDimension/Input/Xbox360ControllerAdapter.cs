using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  public class Xbox360ControllerAdapter : IVirtualAdapter {
    public Xbox360ControllerAdapter() { }

    public VirtualButtonState Confirm() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) ? 
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }
    public VirtualButtonState Back() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }
    public VirtualButtonState Up() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }
    public VirtualButtonState Down() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }
    public VirtualButtonState Left() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadLeft) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickLeft) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }
    public VirtualButtonState Right() {
      return GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadRight) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickRight) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    public Point Point() {
      return new Point(0, 0);
    }
  }
}
