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
      KeyboardState kb = Keyboard.GetState();
      MouseState mouse = Mouse.GetState();

      controller.IsConnected = true;

      controller.Point = new Point(mouse.X, mouse.Y);

      controller.SetButtonState(VirtualButtons.Up, kb.IsKeyDown(Keys.Up));
      controller.SetButtonState(VirtualButtons.Down, kb.IsKeyDown(Keys.Down));
      controller.SetButtonState(VirtualButtons.Left, kb.IsKeyDown(Keys.Left));
      controller.SetButtonState(VirtualButtons.Right, kb.IsKeyDown(Keys.Right));

      controller.SetButtonState(VirtualButtons.Select, kb.IsKeyDown(Keys.Space) || mouse.LeftButton == ButtonState.Pressed);
      controller.SetButtonState(VirtualButtons.Delete, kb.IsKeyDown(Keys.Back));
      controller.SetButtonState(VirtualButtons.Context, kb.IsKeyDown(Keys.Enter));
      controller.SetButtonState(VirtualButtons.Mode, kb.IsKeyDown(Keys.T) || mouse.RightButton == ButtonState.Pressed);
      controller.SetButtonState(VirtualButtons.Pause, kb.IsKeyDown(Keys.Escape));
      controller.SetButtonState(VirtualButtons.Debug, kb.IsKeyDown(Keys.LeftControl) && kb.IsKeyDown(Keys.OemTilde));

      controller.SetButtonState(VirtualButtons.Easter,
        kb.IsKeyDown(Keys.LeftShift) && kb.IsKeyDown(Keys.RightShift) &&
        kb.IsKeyDown(Keys.LeftControl) && kb.IsKeyDown(Keys.RightControl) &&
        kb.IsKeyDown(Keys.LeftAlt) && kb.IsKeyDown(Keys.RightAlt));
    }
  }
}
