using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Reactive;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace PuzzlePathDimension {
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class PuzzlePathGame : Microsoft.Xna.Framework.Game {
    private TopLevelModel TopLevel = new TopLevelModel();

    /// <summary>
    /// Creates a PuzzlePathGame object.
    /// </summary>
    public PuzzlePathGame() {
      // Set the resolution to 800x600
      GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = 800;
      graphics.PreferredBackBufferHeight = 600;
      graphics.ApplyChanges();

      // Tells the game where the content directory is
      Content.RootDirectory = "Content";

      // Make the mouse visible
      IsMouseVisible = true;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize() {
      // Bootstrap the top-level context model
      TopLevel.Game = this;
      TopLevel.Prefs = new UserPrefs();

      Scene scene = new Scene();
      TopLevel.Scene = scene;

      Subject<VirtualControllerState> inputSubject = new Subject<VirtualControllerState>();
      TopLevel.Input = inputSubject;

      WritableVirtualController controller = new WritableVirtualController();
      controller.InputType = InputType.KeyboardMouse;
      inputSubject.Subscribe((state) => {
        Console.WriteLine("State change.");

        controller.IsConnected = state.IsConnected;
        controller.Point = state.Point;

        controller.SetButtonState(VirtualButtons.Up, state.Up);
        controller.SetButtonState(VirtualButtons.Down, state.Down);
        controller.SetButtonState(VirtualButtons.Left, state.Left);
        controller.SetButtonState(VirtualButtons.Right, state.Right);
        controller.SetButtonState(VirtualButtons.Select, state.Select);
        controller.SetButtonState(VirtualButtons.Delete, state.Delete);
        controller.SetButtonState(VirtualButtons.Context, state.Context);
        controller.SetButtonState(VirtualButtons.Mode, state.Mode);
        controller.SetButtonState(VirtualButtons.Pause, state.Pause);
        controller.SetButtonState(VirtualButtons.Debug, state.Debug);
        controller.SetButtonState(VirtualButtons.Easter, state.Easter);
      });
      TopLevel.Controller = controller;


      // Create the input component.
      InputComponent input = new InputComponent(this, inputSubject, controller.InputType);
      controller.InputTypeChanged += input.SetAdapter;
      input.UpdateOrder = 0;
      Components.Add(input);

      // Create the graphical component.
      RenderComponent menus = new RenderComponent(this, scene);
      TopLevel.Scene.AddScreen(new BackgroundScreen(TopLevel));
      TopLevel.Scene.AddScreen(new MainMenuScreen(TopLevel));
      menus.UpdateOrder = 1;
      Components.Add(menus);

      // Initialize all sub-components.
      base.Initialize();
    }
  }
}
