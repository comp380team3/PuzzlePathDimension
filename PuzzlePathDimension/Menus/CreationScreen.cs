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
  class CreationScreen : GameScreen {
    ContentManager content;
    EditableLevel editableLevel;
    MouseState previousMouseState;
    MouseState currentMouseState;


    string LevelName { get; set; }

    //The level object that is selected
    ILevelObject target;
    SpriteFont font;
    Boolean foundCollision, launchToolbox, toolboxLaunched;
    ToolboxScreen toolbox;

    float pauseAlpha;

    public CreationScreen(TopLevelModel topLevel, string levelName)
      : base(topLevel) {
      base.TransitionOnTime = TimeSpan.FromSeconds(1.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);

      LevelName = levelName;
    }

    /// <summary>
    /// Load graphics content for the game.
    /// </summary>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);

      // Create a new ContentManager so that all level data is flushed
      // from the cache after the level ends.
      if (content == null)
        content = new ContentManager(shared.ServiceProvider, "Content");

      font = shared.Load<SpriteFont>("Font/textfont");
      launchToolbox = toolboxLaunched = false;
      // Create the hard-coded level.
      editableLevel = new EditableLevel(LevelLoader.Load("Content/Level/Custom.xml", shared), shared);
      editableLevel.AdditionsLeft = 30;
      editableLevel.Custom = true;
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


    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {

      // there was a bug where if you exit the toolbox without
      // selecting a platform the toolbox was unreachable.
      //toolboxLaunched = false;

      //I was going to handle launching the gameplayscreen here but im not sure how to. -Brian
      if (Controller.IsButtonPressed(VirtualButtons.Mode)) {
        launchToolbox = true;
      }

      if (launchToolbox && !toolboxLaunched) {
        String message = "Select a platform to add to the level";
        if (editableLevel.AdditionsLeft > 0) {
          toolbox = new ToolboxScreen(TopLevel, editableLevel, message, false);
        } else {
          message = "Platform addition limit reached";
          toolbox = new ToolboxScreen(TopLevel, editableLevel, message, true);
        }
        ScreenList.AddScreen(toolbox);
        launchToolbox = false;
        if(!editableLevel.FindCollision())
           LevelSaver.SaveLevel(editableLevel);
        //toolboxLaunched = true;
        //Console.WriteLine(addedPlatform.Origin)
      }

      //Calls the UpdateMovement method to move object on the scree
      UpdateMovement();

      //Check if there is collisions in the simulation. 
      if (editableLevel.FindCollision()) {
        foundCollision = true;
      } else {
        foundCollision = false;
      }

      if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && previousMouseState.LeftButton == ButtonState.Released &&
                      currentMouseState.LeftButton == ButtonState.Pressed) {
        delete(currentMouseState);
      }
    }

    public void delete(MouseState mousePosition) {
      for (int i = editableLevel.MoveablePlatforms.Count - 1; i >= 0; i++) {
        if (editableLevel.MoveablePlatforms[i].IsSelected(mousePosition)) {
          editableLevel.MoveablePlatforms.Remove(editableLevel.MoveablePlatforms[i]);
          return;
        }
      }
      foreach (Platform platform in editableLevel.Platforms) {
        if (platform.IsSelected(mousePosition)) {
          editableLevel.Platforms.Remove(platform);
          return;
        }
      }
      for (int i = editableLevel.DeathTraps.Count - 1; i >= 0; i++) {
        if (editableLevel.DeathTraps[i].IsSelected(mousePosition)) {
          editableLevel.DeathTraps.Remove(editableLevel.DeathTraps[i]);
          return;
        }
      }

      for (int i = editableLevel.Treasures.Count - 1; i >= 0; i++) {
        if (editableLevel.Treasures[i].IsSelected(mousePosition)) {
          editableLevel.Treasures.Remove(editableLevel.Treasures[i]);
          return;
        }
      }
    }


    protected override void OnButtonReleased(VirtualButtons button) {
      switch (button) {
      case VirtualButtons.Context:
        if (!editableLevel.FindCollision()) {
          LevelSaver.SaveLevel(editableLevel);
        }
        break;
      case VirtualButtons.Pause:
        ScreenList.AddScreen(new PauseMenuScreen(TopLevel, editableLevel, LevelName));
        break;
      }
    }

    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      spriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0, 0);

      spriteBatch.Begin();

      // Draw the background.
      spriteBatch.Draw(editableLevel.Background, Vector2.Zero, Color.White);
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


    private Level CreateLevel() {
      Level level = new Level();
      level.Goal = editableLevel.Goal;
      level.DeathTraps = editableLevel.DeathTraps;
      level.Treasures = editableLevel.Treasures;
      level.Launcher = editableLevel.Launcher;
      level.Attempts = editableLevel.Attempts;
      level.ParTime = editableLevel.ParTime;
      foreach (Platform platform in editableLevel.Platforms) {
        level.Platforms.Add(platform);
      }
      foreach (Platform platform in editableLevel.MoveablePlatforms) {
        level.Platforms.Add(platform);
      }
      return level;

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
      editableLevel.Goal.Draw(spriteBatch);




      // Draw the treasures onto the canvas.
      foreach (Treasure treasure in editableLevel.Treasures) {
        treasure.Draw(spriteBatch);
      }
      // Draw the death traps onto the canvas
      foreach (DeathTrap deathTrap in editableLevel.DeathTraps) {
        deathTrap.Draw(spriteBatch);
      }

      // Draw the platforms onto the canvas.
      foreach (Platform platform in editableLevel.Platforms) {
        platform.Draw(spriteBatch);
      }
      foreach (Platform platform in editableLevel.MoveablePlatforms) {
        platform.Draw(spriteBatch);
      }

      // Draw the launcher onto the canvas.
      editableLevel.Launcher.Draw(spriteBatch);
    }

    /// <summary>
    /// Draws any relevant text on the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the text.</param>
    private void DrawText(SpriteBatch spriteBatch) {
      // Draw the number of balls left.
      string attemptsText = "Number of platforms available: " + editableLevel.AdditionsLeft;
      spriteBatch.DrawString(font, attemptsText, new Vector2(10f, 570f), Color.Black);


      // If there is a collision this is where the message will be displayed
      if (foundCollision) {
        spriteBatch.DrawString(font, "Collision!", new Vector2(400f, 300f), Color.Black);
      }
    }


    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    internal EditableLevel LoadLevel(string level) {
      EditableLevel simulation = new EditableLevel(LevelLoader.Load(LevelName.Replace(" ", ""), content), content);
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
          target.Move(new Vector2(changeInX, changeInY));
        }
        if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released) {
          target = null;
          if(!editableLevel.FindCollision())
            LevelSaver.SaveLevel(editableLevel);
        }
      }
    }

    /// <summary>
    /// Looks for a ILevelObject that intersects with a mouse click.
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    public ILevelObject FindTarget(MouseState mousePosition) {
      foreach (Platform platform in editableLevel.MoveablePlatforms) {
        if (platform.IsSelected(mousePosition))
          return platform;
      }
      foreach (Platform platform in editableLevel.Platforms) {
        if (platform.IsSelected(mousePosition))
          return platform;
      }
      if (editableLevel.Goal.IsSelected(mousePosition)) {
        return editableLevel.Goal;
      }
      foreach (DeathTrap deathTrap in editableLevel.DeathTraps) {
        if (deathTrap.IsSelected(mousePosition))
          return deathTrap;
      }
      foreach (Treasure treasure in editableLevel.Treasures) {
        if (treasure.IsSelected(mousePosition))
          return treasure;
      }
      if (editableLevel.Launcher.IsSelected(mousePosition)) {
        return editableLevel.Launcher;
      }
      return null;
    }





  }
}
