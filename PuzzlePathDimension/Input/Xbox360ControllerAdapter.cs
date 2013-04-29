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

    public void Update(WritableVirtualController controller, GameTime gameTime) {
      GamePadState pad = GamePad.GetState(PlayerIndex.One);

      controller.IsConnected = pad.IsConnected;

      Point point = new Point(0, 0);
      // TODO: Implement a virtualized pointer.
      controller.Point = point;

      controller.SetButtonState(VirtualButtons.Up, pad.IsButtonDown(Buttons.DPadUp));
      controller.SetButtonState(VirtualButtons.Down, pad.IsButtonDown(Buttons.DPadDown));
      controller.SetButtonState(VirtualButtons.Left, pad.IsButtonDown(Buttons.DPadLeft));
      controller.SetButtonState(VirtualButtons.Right, pad.IsButtonDown(Buttons.DPadRight));

      controller.SetButtonState(VirtualButtons.Select, pad.IsButtonDown(Buttons.A));
      controller.SetButtonState(VirtualButtons.Delete, pad.IsButtonDown(Buttons.B));
      controller.SetButtonState(VirtualButtons.Context, pad.IsButtonDown(Buttons.Y));
      controller.SetButtonState(VirtualButtons.Mode, pad.IsButtonDown(Buttons.X));
      controller.SetButtonState(VirtualButtons.Pause, pad.IsButtonDown(Buttons.Start));
      controller.SetButtonState(VirtualButtons.Debug, pad.IsButtonDown(Buttons.Back));

      controller.SetButtonState(VirtualButtons.Easter,
        pad.IsButtonDown(Buttons.LeftShoulder) && pad.IsButtonDown(Buttons.LeftShoulder) &&
        pad.IsButtonDown(Buttons.LeftTrigger) && pad.IsButtonDown(Buttons.RightTrigger));
    }
  }
}
