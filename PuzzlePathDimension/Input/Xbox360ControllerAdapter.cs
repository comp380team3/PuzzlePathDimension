using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace PuzzlePathDimension {
  public class Xbox360ControllerAdapter : IVirtualAdapter {
    [DllImport("User32.Dll")]
    static extern long SetCursorPos(int x, int y);

    [DllImport("User32.dll")]
    static extern bool GetCursorPos(out POINT pointer);

    [StructLayout(LayoutKind.Sequential)]
    struct POINT {
      public int x;
      public int y;
    }

    public VirtualControllerState GetState(GameTime gameTime) {
      VirtualControllerState state = new VirtualControllerState();

      GamePadState pad = GamePad.GetState(PlayerIndex.One);

      state.IsConnected = pad.IsConnected;

      state.Up = pad.IsButtonDown(Buttons.DPadUp);
      state.Down = pad.IsButtonDown(Buttons.DPadDown);
      state.Left = pad.IsButtonDown(Buttons.DPadLeft);
      state.Right = pad.IsButtonDown(Buttons.DPadRight);

      state.Select = pad.IsButtonDown(Buttons.A);
      state.Delete = pad.IsButtonDown(Buttons.B);
      state.Context = pad.IsButtonDown(Buttons.Y);
      state.Mode = pad.IsButtonDown(Buttons.X);
      state.Pause = pad.IsButtonDown(Buttons.Start);
      state.Debug = pad.IsButtonDown(Buttons.Back);

      state.Point = MoveMousePointer(gameTime, pad);

      state.Easter =
        pad.IsButtonDown(Buttons.LeftShoulder) && pad.IsButtonDown(Buttons.LeftShoulder) &&
        pad.IsButtonDown(Buttons.LeftTrigger) && pad.IsButtonDown(Buttons.RightTrigger);

      return state;
    }

    private Point MoveMousePointer(GameTime gameTime, GamePadState pad) {
      Vector2 change = new Vector2(0, 0);
      if (pad.IsButtonDown(Buttons.LeftThumbstickUp))
        change.Y -= 1.0f;
      if (pad.IsButtonDown(Buttons.LeftThumbstickDown))
        change.Y += 1.0f;
      if (pad.IsButtonDown(Buttons.LeftThumbstickLeft))
        change.X -= 1.0f;
      if (pad.IsButtonDown(Buttons.LeftThumbstickRight))
        change.X += 1.0f;
      change *= (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 3);

      // Update the OS pointer
      POINT nativePointer;
      GetCursorPos(out nativePointer);
      nativePointer.x += (int)change.X;
      nativePointer.y += (int)change.Y;
      SetCursorPos(nativePointer.x, nativePointer.y);

      MouseState mouse = Mouse.GetState();
      return new Point(mouse.X, mouse.Y);
    }
  }
}
