using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;


namespace PuzzlePathDimension {
  /// <summary>
  /// This is a game component that implements IUpdateable.
  /// </summary>
  public class GameEditorScreen : GameScreen {
    ContentManager content;
    Simulation simulation;
    MouseState previousMouseState;
    MouseState currentMouseState;


    //The level object that is selected
    ILevelObject target;
    SpriteFont font;
    Boolean foundCollision, launchToolbox, toolboxLaunched;
    Platform addedPlatform;
    ToolboxScreen confirmExitMessageBox;

    float pauseAlpha;

    public GameEditorScreen() {
      base.TransitionOnTime = TimeSpan.FromSeconds(1.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    /// <summary>
    /// Load graphics content for the game.
    /// </summary>
    public override void LoadContent(ContentManager shared) {
      // Create a new ContentManager so that all level data is flushed
      // from the cache after the level ends.
      if (content == null)
        content = new ContentManager(shared.ServiceProvider, "Content");

      font = shared.Load<SpriteFont>("Font/textfont");
      launchToolbox = toolboxLaunched =  false;
      // Create the hard-coded level.
      simulation = CreateTestLevel();

      foundCollision = false;
    }

    /// <summary>
    /// Unload graphics content used by the game.
    /// </summary>
    public override void UnloadContent() {
      content.Unload();
    }


    /// <summary>
    /// Updates the state of the game. This method checks the GameScreen.IsActive
    /// property, so the game will stop updating when the pause menu is active,
    /// or if you tab away to a different application.
    /// </summary>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                   bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, false);

      // Gradually fade in or out depending on whether we are covered by the pause screen.
      if (coveredByOtherScreen)
        pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
      else
        pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

      // Bail early if this isn't the active screen.
      if (!IsActive)
        return;
      if (launchToolbox && !toolboxLaunched) {
        String message = "Select a platform to add to the level";
        confirmExitMessageBox = new ToolboxScreen(message);
        ScreenList.AddScreen(confirmExitMessageBox, PlayerIndex.One);
        launchToolbox = false;
        toolboxLaunched = true;
        //Console.WriteLine(addedPlatform.Origin)
      }
      if (toolboxLaunched) {
        addedPlatform = confirmExitMessageBox.Selected;
      }
      if (addedPlatform != null) {
        Console.WriteLine(addedPlatform.Origin);
        simulation.Platforms.Add(addedPlatform);
        ScreenList.RemoveScreen(confirmExitMessageBox);
        addedPlatform = null;
        toolboxLaunched = false;

      }
      
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {

      //Calls the UpdateMovement method to move object on the scree
      UpdateMovement();

      //Check if there is collisions in the simulation. 
      if (simulation.FindCollision()) {
        foundCollision = true;
      } else {
        foundCollision = false;
      }

      //I was going to handle launching the gameplayscreen here but im not sure how to. -Brian
      if (previousMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton ==  ButtonState.Pressed) {
        launchToolbox = true;
      }

      if(!foundCollision && vtroller.CheckForRecentRelease(VirtualButtons.Confirm)){
        ScreenList.AddScreen(new GameplayScreen(simulation), ControllingPlayer);

      }


      //Pause Screen
      if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        ScreenList.AddScreen(new PauseMenuScreen(), ControllingPlayer);
      }

      // TODO: Replace this restart mechanism
      if (Keyboard.GetState().IsKeyDown(Keys.R) ||
        GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X)) {
        Console.WriteLine("Completely restarted.");
        simulation.Restart();
      }
    }

    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      spriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0, 0);

      spriteBatch.Begin();

      // Draw the background.
      spriteBatch.Draw(simulation.Background, Vector2.Zero, Color.White);
      // Draw the walls.
      DrawWalls(spriteBatch);
      // Draw all the level objects.
      DrawLevelObjects(spriteBatch);
      // Draw any informational text.
      DrawText(spriteBatch);

      spriteBatch.End();

      // If the game is transitioning on or off, fade it out to black.
      if (TransitionPosition > 0 || pauseAlpha > 0) {
        float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

        spriteBatch.FadeBackBufferToBlack(alpha);
      }
    }


    /// <summary>
    /// Draw the hard-coded walls.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the walls.</param>
    private void DrawWalls(SpriteBatch spriteBatch) {
      Texture2D wallTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
      wallTexture.SetData<Color>(new Color[] { Color.White });

      spriteBatch.Draw(wallTexture, new Rectangle(0, 0, 800, 5), null, Color.Black);
      spriteBatch.Draw(wallTexture, new Rectangle(0, 595, 800, 5), null, Color.Black);
      spriteBatch.Draw(wallTexture, new Rectangle(0, 0, 5, 600), null, Color.Black);
      spriteBatch.Draw(wallTexture, new Rectangle(795, 0, 5, 600), null, Color.Black);
    }

    /// <summary>
    /// Draws the level objects.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the level objects.</param>
    private void DrawLevelObjects(SpriteBatch spriteBatch) {
      // Draw the goal onto the canvas.
      simulation.Goal.Draw(spriteBatch);

      // Draw the platforms onto the canvas.
      foreach (Platform platform in simulation.Platforms) {
        platform.Draw(spriteBatch);
      }
      // Draw the treasures onto the canvas.
      foreach (Treasure treasure in simulation.Treasures) {
        treasure.Draw(spriteBatch);
      }
      // Draw the death traps onto the canvas
      foreach (DeathTrap deathTrap in simulation.DeathTraps) {
        deathTrap.Draw(spriteBatch);
      }

      // Since we are not playing do not draw the ball. 
      //simulation.Ball.Draw(spriteBatch);

      // Draw the launcher onto the canvas.
      simulation.Launcher.Draw(spriteBatch);
    }

    /// <summary>
    /// Draws any relevant text on the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the text.</param>
    private void DrawText(SpriteBatch spriteBatch) {
      // Draw the number of balls left.
      string attemptsText = "Balls left: " + simulation.AttemptsLeft;
      spriteBatch.DrawString(font, attemptsText, new Vector2(10f, 570f), Color.Black);


      // If there is a collision this is where the message will be displayed
      if (foundCollision) {
        spriteBatch.DrawString(font, "Collision!", new Vector2(400f, 300f), Color.Black);
      }
    }


    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    internal Simulation CreateTestLevel() {
      Simulation simulation = new Simulation(LevelLoader.Load("Content/Level/TestLevel.xml", content), content);
      simulation.Background = content.Load<Texture2D>("Texture/GameScreen");

      return simulation;
    }



    /// <summary>
    /// Handles the movement of level objects
    /// </summary>
    public void UpdateMovement() {
      previousMouseState = currentMouseState;
      currentMouseState = Mouse.GetState();

      if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed) {
        target = FindTarget(currentMouseState);
      }
      if (target != null) {
        if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed) {
          float changeInX = currentMouseState.X - previousMouseState.X;
          float changeInY = currentMouseState.Y - previousMouseState.Y;
          //if(currentMouseState.X < 5||  > Simulation.FieldWidth- 5){
          //    changeInX = 0;
          //}


          //if (currentMouseState.Y < 5 || currentMouseState.Y > Simulation.FieldHeight - 5) {
          //    changeInY = 0;
          //}
          target.Move(new Vector2(changeInX, changeInY));
        }
        if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released) {
          target = null;
        }
      }
    }

    /// <summary>
    /// After clicking it will select the Level Object the user wants to move.
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    public ILevelObject FindTarget(MouseState mousePosition) {
      if (simulation.Goal.IsSelected(mousePosition)) {
        return simulation.Goal;
      }
      foreach (Platform platform in simulation.Platforms) {
        if (platform.IsSelected(mousePosition))
          return platform;
      }
      foreach (DeathTrap deathtrap in simulation.DeathTraps) {
        if (deathtrap.IsSelected(mousePosition))
          return deathtrap;
      }
      foreach (Treasure treasure in simulation.Treasures) {
        if (treasure.IsSelected(mousePosition))
          return treasure;
      }
      return null;
    }

  }
}