using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  /// <summary>
  /// The KeyboardMouseAdapter maps keyboard and mouse input to the input types
  /// expected by the VirtualController.
  /// </summary>
  public class KeyboardMouseAdapter : VirtualAdapter {
    public KeyboardMouseAdapter() { }

    /// <summary>
    /// Defines the keyboard input that corresponds to the Confirm input.
    /// </summary>
    /// <returns>The state of the keyboard input that is mapped to the Confirm input.</returns>
    public override VirtualButtonState Confirm() {
      return Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Space) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the keyboard input that corresponds to the Back input.
    /// </summary>
    /// <returns>The state of the keyboard input that is mapped to the Back input.</returns>
    public override VirtualButtonState Back() {
      return Keyboard.GetState().IsKeyDown(Keys.Escape) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the keyboard input that corresponds to the Up input.
    /// </summary>
    /// <returns>The state of the keyboard input that is mapped to the Up input.</returns>
    public override VirtualButtonState Up() {
      return Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the keyboard input that corresponds to the Down input.
    /// </summary>
    /// <returns>The state of the keyboard input that is mapped to the Down input.</returns>
    public override VirtualButtonState Down() {
      return Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the keyboard input that corresponds to the Left input.
    /// </summary>
    /// <returns>The state of the keyboard input that is mapped to the Left input.</returns>
    public override VirtualButtonState Left() {
      return Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the keyboard input that corresponds to the Right input.
    /// </summary>
    /// <returns>The state of the keyboard input that is mapped to the Right input.</returns>
    public override VirtualButtonState Right() {
      return Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D) ?
        VirtualButtonState.Pressed : VirtualButtonState.Released;
    }

    /// <summary>
    /// Defines the mouse input that corresponds to the Point input.
    /// </summary>
    /// <returns>The coordinates of the mouse pointer.</returns>
    public override Point Point() {
      return new Point(Mouse.GetState().X, Mouse.GetState().Y);
    }
  }
}
