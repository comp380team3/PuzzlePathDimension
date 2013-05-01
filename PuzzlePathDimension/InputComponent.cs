using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  public interface IVirtualAdapter {
    VirtualControllerState GetState(GameTime gameTime);
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
    private ISubject<VirtualControllerState> Source { get; set; }
    private IVirtualAdapter Adapter { get; set; }

    public IObservable<VirtualControllerState> InputStates { get; private set; }

    public InputComponent(PuzzlePathGame game)
      : base(game) {
      Source = new Subject<VirtualControllerState>();

      // Skip duplicate state-steps.
      InputStates = Source.DistinctUntilChanged();
    }

    public override void Initialize() {
      base.Initialize();
    }

    public override void Update(GameTime gameTime) {
      base.Update(gameTime);
      Source.OnNext(Adapter.GetState(gameTime));
    }

    public void SetAdapter(InputType inputType) {
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