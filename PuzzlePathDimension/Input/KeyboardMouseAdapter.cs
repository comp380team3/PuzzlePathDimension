using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension.Input {
  public class KeyboardMouseAdapter : VirtualAdapter {
    public KeyboardMouseAdapter() { }

    public override VirtualButtonState Confirm() {
      return Keyboard.GetState().IsKeyDown(Keys.Enter) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    public override VirtualButtonState Back() {
      return Keyboard.GetState().IsKeyDown(Keys.Escape) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    public override VirtualButtonState Up() {
      return Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    public override VirtualButtonState Down() {
      return Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    public override VirtualButtonState Left() {
      return Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    public override VirtualButtonState Right() {
      return Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    public override Point Point() {
      return new Point(Mouse.GetState().X, Mouse.GetState().Y);
    }
  }
}
