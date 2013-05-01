//-----------------------------------------------------------------------------
// GameScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  public class ScreenInput {
    public IObservable<VirtualButtons> ButtonPresses { get; private set; }
    public IObservable<VirtualButtons> ButtonReleases { get; private set; }
    public IObservable<Point> PointChanges { get; private set; }

    public ScreenInput(IObservable<VirtualControllerState> Input) {
      var ButtonPresses = new Subject<VirtualButtons>();
      var ButtonReleases = new Subject<VirtualButtons>();
      var PointChanges = new Subject<Point>();

      Input = Input.StartWith(new VirtualControllerState());

      // Wire up the buttons
      Input
        .Zip(Input.Skip(1), Tuple.Create)
        .Subscribe((tuple) => {
          var old = tuple.Item1;
          var curr = tuple.Item2;

          foreach (VirtualButtons button in Enum.GetValues(typeof(VirtualButtons))) {
            if (old.IsButtonPressed(button) != curr.IsButtonPressed(button)) {
              ((curr.IsButtonPressed(button)) ? ButtonPresses : ButtonReleases).OnNext(button);
              Console.WriteLine(button.ToString() + " " + curr.IsButtonPressed(button));
            }
          }
        });

      // Wire up the point
      Input
        .Select((state) => state.Point)
        .DistinctUntilChanged()
        .Subscribe(PointChanges);

      this.ButtonPresses = ButtonPresses;
      this.ButtonReleases = ButtonReleases;
      this.PointChanges = PointChanges;
    }
  }

  /// <summary>
  /// Enum describes the screen transition state.
  /// </summary>
  public enum ScreenState {
    TransitionOn = 0,
    Active,
    TransitionOff,
    Hidden,
  }

  /// <summary>
  /// A screen is a single layer that has update and draw logic, and which
  /// can be combined with other layers to build up a complex menu system.
  /// For instance the main menu, the options menu, the "are you sure you
  /// want to quit" message box, and the main game itself are all implemented
  /// as screens.
  /// </summary>
  public abstract class GameScreen {
    bool otherScreenHasFocus;

    /// <summary>
    /// Normally when one screen is brought up over the top of another,
    /// the first screen will transition off to make room for the new
    /// one. This property indicates whether the screen is only a small
    /// popup, in which case screens underneath it do not need to bother
    /// transitioning off.
    /// </summary>
    public bool IsPopup { get; protected set; }

    /// <summary>
    /// Indicates how long the screen takes to
    /// transition on when it is activated.
    /// </summary>
    public TimeSpan TransitionOnTime { get; protected set; }

    /// <summary>
    /// Indicates how long the screen takes to
    /// transition off when it is deactivated.
    /// </summary>
    public TimeSpan TransitionOffTime { get; protected set; }

    /// <summary>
    /// Gets the current alpha of the screen transition, ranging
    /// from 1 (fully active, no transition) to 0 (transitioned
    /// fully off to nothing).
    /// </summary>
    public float TransitionAlpha { get; protected set; }

    /// <summary>
    /// Gets the current screen transition state.
    /// </summary>
    public ScreenState ScreenState { get; protected set; }

    /// <summary>
    /// There are two possible reasons why a screen might be transitioning
    /// off. It could be temporarily going away to make room for another
    /// screen that is on top of it, or it could be going away for good.
    /// This property indicates whether the screen is exiting for real:
    /// if set, the screen will automatically remove itself as soon as the
    /// transition finishes.
    /// </summary>
    public bool IsExiting { get; protected internal set; }

    protected TopLevelModel TopLevel { get; private set; }

    public Game Game {
      get { return TopLevel.Game; }
    }

    public IScreenList ScreenList {
      get { return TopLevel.Scene; }
    }

    /// <summary>
    /// Gets the user's set of preferences.
    /// </summary>
    public UserPrefs Prefs {
      get { return TopLevel.Prefs; }
    }

    /// <summary>
    /// Retrieves the game controller.
    /// </summary>
    public VirtualController Controller {
      get { return TopLevel.Controller; }
    }


    /// <summary>
    /// Checks whether this screen is active and can respond to user input.
    /// </summary>
    public bool IsActive {
      get {
        return !otherScreenHasFocus &&
               (ScreenState == ScreenState.TransitionOn ||
                ScreenState == ScreenState.Active);
      }
    }

    /// <summary>
    /// Gets the current position of the screen transition, ranging
    /// from zero (fully active, no transition) to one (transitioned
    /// fully off to nothing).
    /// </summary>
    public float TransitionPosition {
      get { return 1f - TransitionAlpha; }
      protected set { TransitionAlpha = 1f - value; }
    }

    protected ScreenInput Input { get; private set; }
    private IDisposable InputSubscription { get; set; }

    public GameScreen(TopLevelModel topLevel) {
      TopLevel = topLevel;
    }

    /// <summary>
    /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
    /// instantly kills the screen, this method respects the transition timings
    /// and will give the screen a chance to gradually transition off.
    /// </summary>
    public void ExitScreen() {
      if (TransitionOffTime == TimeSpan.Zero) {
        // If the screen has a zero transition time, remove it immediately.
        ScreenList.RemoveScreen(this);
      } else {
        // Otherwise flag that it should transition off and then exit.
        IsExiting = true;
      }
    }


    /// <summary>
    /// Load graphics content for the screen.
    /// </summary>
    public virtual void LoadContent(ContentManager shared) {
      Subject<VirtualControllerState> subject = new Subject<VirtualControllerState>();

      Input = new ScreenInput(subject.Where((state) => IsActive));
      Input.ButtonPresses.Subscribe(OnButtonPressed);
      Input.ButtonReleases.Subscribe(OnButtonReleased);
      Input.PointChanges.Subscribe(OnPointChanged);

      InputSubscription = TopLevel.Input.Subscribe(subject);
      // TODO: Add Connected/Disconnected events (maybe)
    }

    /// <summary>
    /// Unload content for the screen.
    /// </summary>
    public virtual void UnloadContent() {
      InputSubscription.Dispose();
    }


    /// <summary>
    /// Allows the screen to run logic, such as updating the transition position.
    /// Unlike HandleInput, this method is called regardless of whether the screen
    /// is active, hidden, or in the middle of a transition.
    /// </summary>
    public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                  bool coveredByOtherScreen) {
      this.otherScreenHasFocus = otherScreenHasFocus;

      if (IsExiting) {
        // If the screen is going away to die, it should transition off.
        ScreenState = ScreenState.TransitionOff;

        if (!UpdateTransition(gameTime, TransitionOffTime, 1)) {
          // When the transition finishes, remove the screen.
          ScreenList.RemoveScreen(this);
        }
      } else if (coveredByOtherScreen) {
        // If the screen is covered by another, it should transition off.
        if (UpdateTransition(gameTime, TransitionOffTime, 1)) {
          // Still busy transitioning.
          ScreenState = ScreenState.TransitionOff;
        } else {
          // Transition finished!
          ScreenState = ScreenState.Hidden;
        }
      } else {
        // Otherwise the screen should transition on and become active.
        if (UpdateTransition(gameTime, TransitionOnTime, -1)) {
          // Still busy transitioning.
          ScreenState = ScreenState.TransitionOn;
        } else {
          // Transition finished!
          ScreenState = ScreenState.Active;
        }
      }

      if (IsActive)
        HandleInput(Controller);
    }

    /// <summary>
    /// Helper for updating the screen transition position.
    /// </summary>
    bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction) {
      // How much should we move by?
      float transitionDelta;

      if (time == TimeSpan.Zero)
        transitionDelta = 1;
      else
        transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                  time.TotalMilliseconds);

      // Update the transition position.
      TransitionPosition += transitionDelta * direction;

      // Did we reach the end of the transition?
      if (((direction < 0) && (TransitionPosition <= 0)) ||
          ((direction > 0) && (TransitionPosition >= 1))) {
        TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
        return false;
      }

      // Otherwise we are still busy transitioning.
      return true;
    }

    /// <summary>
    /// Allows the screen to handle user input. Unlike Update, this method
    /// is only called when the screen is active, and not when some other
    /// screen has taken the focus.
    /// </summary>
    public virtual void HandleInput(VirtualController vtroller) { }

    /// <summary>
    /// This is called when the screen should draw itself.
    /// </summary>
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

    /* Input-handling hooks */
    protected virtual void OnControllerConnected() { }
    protected virtual void OnControllerDisconnected() { }
    protected virtual void OnButtonPressed(VirtualButtons button) { }
    protected virtual void OnButtonReleased(VirtualButtons button) { }
    protected virtual void OnPointChanged(Point point) { }
  }
}
