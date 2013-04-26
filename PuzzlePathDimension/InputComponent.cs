using System;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  public interface IVirtualAdapter {
    void Update(WritableVirtualController controller, GameTime gameTime);
  }

  /// <summary>
  /// The type of input device that input is being received from.
  /// </summary>
  public enum InputType {
    /// <summary>
    /// Corresponds to the keyboard and mouse combination.
    /// </summary>
    KeyboardMouse = 0,
    /// <summary>
    /// Corresponds to the Xbox 360 controller combination.
    /// </summary>
    Xbox360Controller
  }

  public class InputComponent : GameComponent {
    private WritableVirtualController Controller { get; set; }

    private IVirtualAdapter Adapter { get; set; }

    public InputComponent(PuzzlePathGame game, WritableVirtualController controller)
      : base(game) {
      Controller = controller;
    }

    public override void Initialize() {
      base.Initialize();

      SetAdapter(Controller.InputType);
      Controller.InputTypeChanged += SetAdapter;
    }

    public override void Update(GameTime gameTime) {
      base.Update(gameTime);

      if (Adapter == null)
        return;

      Adapter.Update(Controller, gameTime);
    }


    private void SetAdapter(InputType inputType) {
      switch (inputType) {
      case InputType.KeyboardMouse:
        Adapter = new KeyboardMouseAdapter();
        break;
      case InputType.Xbox360Controller:
        Adapter = new Xbox360ControllerAdapter();
        break;
      default:
        throw new ArgumentException("Unexpected InputType value: " + inputType.ToString());
      }
    }
  }
}