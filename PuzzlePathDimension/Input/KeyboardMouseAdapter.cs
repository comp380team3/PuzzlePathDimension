using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  public class KeyboardMouseAdapter : VirtualAdapter {
    public KeyboardMouseAdapter() { }

    public override DigitalButtonState Confirm() {
      return Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Space) ?
        DigitalButtonState.Pressed : DigitalButtonState.Released;
    }

    public override DigitalButtonState Back() {
      return Keyboard.GetState().IsKeyDown(Keys.Escape) ?
        DigitalButtonState.Pressed : DigitalButtonState.Released;
    }

    public override DigitalButtonState Up() {
      return Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W) ?
        DigitalButtonState.Pressed : DigitalButtonState.Released;
    }

    public override DigitalButtonState Down() {
      return Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S) ?
        DigitalButtonState.Pressed : DigitalButtonState.Released;
    }

    public override DigitalButtonState Left() {
      return Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A) ?
        DigitalButtonState.Pressed : DigitalButtonState.Released;
    }

    public override DigitalButtonState Right() {
      return Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D) ?
        DigitalButtonState.Pressed : DigitalButtonState.Released;
    }

    public override Point Point() {
      return new Point(Mouse.GetState().X, Mouse.GetState().Y);
    }
  }
}
