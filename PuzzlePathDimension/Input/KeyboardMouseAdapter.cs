using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  /// <summary>
  /// The KeyboardMouseAdapter maps keyboard and mouse input to the input types
  /// expected by the VirtualController.
  /// </summary>
  public class KeyboardMouseAdapter : IVirtualAdapter {
    /// <summary>
    /// Gets whether the keyboard and mouse are connected.
    /// </summary>
    public bool Connected {
      // Assume that they always are connected.
      get { return true; }
    }

    public void Update(WritableVirtualController controller, GameTime gameTime) {
      controller.SetButtonState(VirtualButtons.Delete, IsKeyDown(Keys.Escape));
      controller.SetButtonState(VirtualButtons.Select, IsKeyDown(Keys.Enter) || IsKeyDown(Keys.Space));
      controller.SetButtonState(VirtualButtons.Pause, IsKeyDown(Keys.Pause) || IsKeyDown(Keys.Escape));
      controller.SetButtonState(VirtualButtons.Context, Mouse.GetState().RightButton == ButtonState.Pressed);

      controller.SetButtonState(VirtualButtons.Up, IsKeyDown(Keys.Up));
      controller.SetButtonState(VirtualButtons.Down, IsKeyDown(Keys.Down));
      controller.SetButtonState(VirtualButtons.Left, IsKeyDown(Keys.Left));
      controller.SetButtonState(VirtualButtons.Right, IsKeyDown(Keys.Right));

      controller.Point = new Point(Mouse.GetState().X, Mouse.GetState().Y);
    }


    private bool IsKeyDown(Keys key) {
      return Keyboard.GetState().IsKeyDown(key);
    }
  }
}
