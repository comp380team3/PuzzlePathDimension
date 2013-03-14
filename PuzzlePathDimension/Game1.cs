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
    ControllerDetectScreen mControllerScreen;
    TitleScreen mTitleScreen;
    GameScreen mGameScreen;

    Stack<Screen> stateStack;

    /// <summary>
    /// Creates a Game1 object.
    /// </summary>
    public Game1() {
      stateStack = new Stack<Screen>();

      // Obtain a reference to the graphics API.
      spriteBatch = new SpriteBatch(GraphicsDevice);

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
      // Initialize the various screens in the game
      mControllerScreen = new ControllerDetectScreen(this.Content, new EventHandler(ControllerDetectScreenEvent));
      mTitleScreen = new TitleScreen(this.Content, new EventHandler(TitleScreenEvent));
      mGameScreen = new GameScreen(this.Content, GraphicsDevice.Viewport, new EventHandler(GameScreenEvent));

      //Set the current screen
      stateStack.Push(mControllerScreen);

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
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
      GraphicsDevice.Clear(Color.White);

      // TODO: Add your drawing code here
      spriteBatch.Begin();

      //Again, using Polymorphism, we can call draw on the current screen class
      //and the Draw in the subclass is the one that will be executed.
      stateStack.Peek().Draw(spriteBatch);

      spriteBatch.End();

      base.Draw(gameTime);
    }

    //This event fires when the Controller detect screen is returing control back to the main game class
    public void ControllerDetectScreenEvent(Object obj, EventArgs e) {
      //Switch to the title screen, the Controller detect screen is finished being displayed
      stateStack.Pop();
      stateStack.Push(mTitleScreen);
    }

    //Thid event is fired when the Title screen is returning control back to the main game class
    public void TitleScreenEvent(Object obj, EventArgs e) {
      //Switch to the controller detect screen, the Title screen is finished being displayed
      stateStack.Push(mGameScreen);
    }

    public void GameScreenEvent(Object obj, EventArgs e) {
      //Switch to the title screen
      stateStack.Pop();
    }
  }
}
