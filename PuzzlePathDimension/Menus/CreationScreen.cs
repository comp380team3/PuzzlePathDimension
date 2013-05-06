using System;
using System.IO;
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
    Point prevPosition;


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
      String name = Configuration.UserDataPath + Path.DirectorySeparatorChar + "Level" + Path.DirectorySeparatorChar + "Custom.xml";

      // Create the hard-coded level.
      editableLevel = new EditableLevel(LevelLoader.Load(name, shared), shared);
      editableLevel.AdditionsLeft = 30;
      editableLevel.ParTime = 60;
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
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    internal EditableLevel LoadLevel(string level) {
      EditableLevel simulation = new EditableLevel(LevelLoader.Load(LevelName.Replace(" ", ""), content), content);
      simulation.Background = content.Load<Texture2D>("Texture/GameScreen");

      return simulation;
    }

    /// <summary>
    /// Updates the state of the game. This method checks the GameScreen.IsActive
    /// property, so the game will stop updating when the pause menu is active,
    /// or if you tab away to a different application.
    /// </summary>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                   bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, false);
      if (coveredByOtherScreen)
        pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
      else
        pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {


      //I was going to handle launching the gameplayscreen here but im not sure how to. -Brian
      if (Controller.IsButtonPressed(VirtualButtons.Mode)) {
        launchToolbox = true;
      }

      if (launchToolbox && !toolboxLaunched) {
        String message = "Select an object to add to the level";
        if (editableLevel.AdditionsLeft > 0) {
          toolbox = new ToolboxScreen(TopLevel, editableLevel, message, false);
        } else {
          message = "Object addition limit reached";
          toolbox = new ToolboxScreen(TopLevel, editableLevel, message, true);
        }
        ScreenList.AddScreen(toolbox);
        launchToolbox = false;
        if(!editableLevel.FindCollision())
           LevelSaver.SaveLevel(editableLevel);
      }

      //Calls the UpdateMovement method to move object on the scree
      UpdateMovement();

      //Check if there is collisions in the simulation. 
      if (editableLevel.FindCollision()) {
        foundCollision = true;
      } else {
        foundCollision = false;
      }
    }

    /// <summary>
    /// Handles the movement of level objects
    /// </summary>
    public void UpdateMovement() {
      if (target != null) {
        Point currPosition = Controller.Point;
        float changeInX = currPosition.X - prevPosition.X;
        float changeInY = currPosition.Y - prevPosition.Y;
        prevPosition = currPosition;

        target.Move(new Vector2(changeInX, changeInY));
      }
    }

    /// <summary>
    /// Looks for a ILevelObject that intersects with a mouse click.
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    private ILevelObject FindTarget(Point mousePosition) {
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

    /// <summary>
    /// Deletes the top most object where the mouse click ovvured
    /// </summary>
    /// <param name="mousePosition"></param>
    private void delete(Point mousePosition) {
      //loops start at end since the last added to draw are the first ones to be deleted
      for (int i = editableLevel.MoveablePlatforms.Count - 1; i >= 0; i--) {
        if (editableLevel.MoveablePlatforms[i].IsSelected(Controller.Point)) {
          editableLevel.MoveablePlatforms.Remove(editableLevel.MoveablePlatforms[i]);
          return;
        }
      }
      foreach (Platform platform in editableLevel.Platforms) {
        if (platform.IsSelected(Controller.Point)) {
          editableLevel.Platforms.Remove(platform);
          return;
        }
      }
      for (int i = editableLevel.DeathTraps.Count - 1; i >= 0; i--) {
        if (editableLevel.DeathTraps[i].IsSelected(Controller.Point)) {
          editableLevel.DeathTraps.Remove(editableLevel.DeathTraps[i]);
          return;
        }
      }

      for (int i = editableLevel.Treasures.Count - 1; i >= 0; i--) {
        if (editableLevel.Treasures[i].IsSelected(Controller.Point)) {
          editableLevel.Treasures.Remove(editableLevel.Treasures[i]);
          return;
        }
      }
    }


    protected override void OnButtonPressed(VirtualButtons button) {
      switch (button) {
      case VirtualButtons.Select:
        prevPosition = Controller.Point;
        target = FindTarget(Controller.Point);
        break;
      case VirtualButtons.Delete:
        delete(Controller.Point);
        break;
      }
    }

    /// <summary>
    /// Handle user input.
    /// </summary>
    /// <param name="button"></param>
    protected override void OnButtonReleased(VirtualButtons button) {
      switch (button) {
      case VirtualButtons.Context:
        if (!editableLevel.FindCollision())
          LevelSaver.SaveLevel(editableLevel);
        break;
      case VirtualButtons.Pause:
        ScreenList.AddScreen(new PauseMenuScreen(TopLevel, editableLevel, LevelName));
        break;
      case VirtualButtons.Select:
        target = null;
        break;
      }
    }

    /// <summary>
    /// Draws the screen.
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
      string objectsLeftText;

      // Figure out what text to display, and calculate how long it is.
      if (!foundCollision) {
        objectsLeftText = "Number of objects available: " + editableLevel.AdditionsLeft;
      } else {
        objectsLeftText = "Number of objects available: " + editableLevel.AdditionsLeft + 
          " | Invalid object placement detected.";
      }
      Vector2 textLength = font.MeasureString(objectsLeftText);

      // Draw a transparent white box underneath the text that is as long as the string (and a bit more).
      Texture2D textTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
      textTexture.SetData<Color>(new Color[] { Color.FromNonPremultiplied(255, 255, 255, 192) });
      spriteBatch.Draw(textTexture, new Rectangle(5, 570, (int)textLength.X + 15, 25), 
        null, Color.White);

      // Draw the number of additions left.
      spriteBatch.DrawString(font, objectsLeftText, new Vector2(10f, 570f), Color.Black);
    }
  }
}
