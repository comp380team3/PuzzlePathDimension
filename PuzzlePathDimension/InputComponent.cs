using System;
using Microsoft.Xna.Framework;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace PuzzlePathDimension {
  public interface IVirtualAdapter {
    void Update(IObserver<VirtualControllerState> observer, GameTime gameTime);
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
    private Subject<VirtualControllerState> Source { get; set; }
    private VirtualControllerState PreviousState { get; set; }
    private IVirtualAdapter Adapter { get; set; }

    public InputComponent(PuzzlePathGame game, IObserver<VirtualControllerState> observer, InputType type)
      : base(game) {
      Source = new Subject<VirtualControllerState>();
      PreviousState = new VirtualControllerState();

      Source.Where((state) => {
        if (state.Equals(PreviousState))
          return false;

        PreviousState = state;
        return true;
      }).Subscribe(observer);

      SetAdapter(type);
    }

    public override void Initialize() {
      base.Initialize();
    }

    public override void Update(GameTime gameTime) {
      base.Update(gameTime);

      if (Adapter == null)
        return;

      Adapter.Update(Source, gameTime);
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