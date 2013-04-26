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

namespace PuzzlePathDimension {
  // Please, please, please ensure that this does not become a god object.
  public class TopLevelModel {
    public WritableVirtualController Controller { get; set; }
    public UserPrefs Prefs { get; set; }
    public Game Game { get; set; }
    public Scene Scene { get; set; }
  }

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

      WritableVirtualController controller = new WritableVirtualController();
      controller.InputType = InputType.KeyboardMouse;
      TopLevel.Controller = controller;

      Scene scene = new Scene();
      TopLevel.Scene = scene;

      // Create the input component.
      InputComponent input = new InputComponent(this, controller);
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
