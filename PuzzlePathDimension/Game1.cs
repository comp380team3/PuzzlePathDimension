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
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game {
    /// <summary>
    /// The size of one square on the level grid.
    /// </summary>
    public static readonly float GridSize = 20f;

    // The drawing API
    SpriteBatch spriteBatch;

    //The screens and the current screen
    Stack<GameState> stateStack;

    /// <summary>
    /// Creates a Game1 object.
    /// </summary>
    public Game1() {
      stateStack = new Stack<GameState>();

      // Set the resolution to 800x600
      GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = 800;
      graphics.PreferredBackBufferHeight = 600;
      graphics.ApplyChanges();

      // Tells the game where the content directory is
      Content.RootDirectory = "Content";

      // Make the mouse visible
      this.IsMouseVisible = true;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize() {
      // Initialize the state stack.
      this.PushState(new ExitState(this));

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      // Obtain a reference to the graphics API.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      this.PushState(new ControllerDetectState(this, spriteBatch));
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent() {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
          Keyboard.GetState().IsKeyDown(Keys.Escape)) {
        this.Exit();
      }

      // TODO: Add your update logic here
      //By taking advantage of Polymorphism, we can call update on the current screen class,
      //but the Update in the subclass is the one that will be executed.
      stateStack.Peek().Update(gameTime);

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      spriteBatch.Begin();

      GraphicsDevice.Clear(Color.White);

      //Again, using Polymorphism, we can call draw on the current screen class
      //and the Draw in the subclass is the one that will be executed.
      stateStack.Peek().Draw(spriteBatch);

      spriteBatch.End();

      base.Draw(gameTime);
    }

    // These may be better suited to a separate StateStack class.
    public void PushState(GameState state) {
      stateStack.Push(state);
    }

    public void PopState() {
      if (stateStack.Count == 1) {
        throw new InvalidOperationException("There are no states that can be popped.");
      }

      stateStack.Pop();
    }

    public void ReplaceState(GameState state) {
      this.PopState();
      this.PushState(state);
    }
  }
}
