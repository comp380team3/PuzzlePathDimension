using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  /// <summary>
  /// The KeyboardMouseAdapter maps keyboard and mouse input to the input types
  /// expected by the VirtualController.
  /// </summary>
  public class KeyboardMouseAdapter : IVirtualAdapter {
    public VirtualControllerState GetState(GameTime gameTime) {
      VirtualControllerState state = new VirtualControllerState();

      KeyboardState kb = Keyboard.GetState();
      MouseState mouse = Mouse.GetState();

      state.IsConnected = true;

      state.Point = new Point(mouse.X, mouse.Y);

      state.Up = kb.IsKeyDown(Keys.Up);
      state.Down = kb.IsKeyDown(Keys.Down);
      state.Left = kb.IsKeyDown(Keys.Left);
      state.Right = kb.IsKeyDown(Keys.Right);

      state.Select = kb.IsKeyDown(Keys.Space) || mouse.LeftButton == ButtonState.Pressed;
      state.Delete = kb.IsKeyDown(Keys.Back);
      state.Context = kb.IsKeyDown(Keys.Enter);
      state.Mode = kb.IsKeyDown(Keys.T) || mouse.RightButton == ButtonState.Pressed;
      state.Pause = kb.IsKeyDown(Keys.Escape);
      state.Debug = kb.IsKeyDown(Keys.LeftControl) && kb.IsKeyDown(Keys.OemTilde);

      state.Easter =
        kb.IsKeyDown(Keys.LeftShift) && kb.IsKeyDown(Keys.RightShift) &&
        kb.IsKeyDown(Keys.LeftControl) && kb.IsKeyDown(Keys.RightControl) &&
        kb.IsKeyDown(Keys.LeftAlt) && kb.IsKeyDown(Keys.RightAlt);

      return state;
    }
  }
}
