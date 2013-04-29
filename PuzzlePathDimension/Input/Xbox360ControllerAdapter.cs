using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  public class Xbox360ControllerAdapter : IVirtualAdapter {
    public void Update(IObserver<VirtualControllerState> observer, GameTime gameTime) {
      VirtualControllerState state = new VirtualControllerState();

      GamePadState pad = GamePad.GetState(PlayerIndex.One);

      state.IsConnected = pad.IsConnected;

      Point point = new Point(0, 0);
      // TODO: Implement a virtualized pointer.
      state.Point = point;

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

      state.Easter =
        pad.IsButtonDown(Buttons.LeftShoulder) && pad.IsButtonDown(Buttons.LeftShoulder) &&
        pad.IsButtonDown(Buttons.LeftTrigger) && pad.IsButtonDown(Buttons.RightTrigger);

      observer.OnNext(state);
    }
  }
}
