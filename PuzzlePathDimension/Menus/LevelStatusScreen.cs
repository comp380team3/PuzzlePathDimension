using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {

  class LevelStatusScreen : MenuScreen {

    #region Fields
    
    /// <summary>
    /// If the level is complete, true, otherwise false.
    /// </summary>
    bool completed;

    /// <summary>
    /// The highest score for the current level.
    /// </summary>
    int levelScore;

    /// <summary>
    /// The number that identifies the current level.
    /// </summary>
    int levelNumber;

    /// <summary>
    /// The time spent on the completion of the current level.
    /// </summary>
    string completionTime;

    MenuEntry startMenuEntry;

    MenuEntry exitMenuEntry;

    #endregion

    #region Properties

    /// <summary>
    /// Return true if level is completed, otherwise false.
    /// </summary>
    public bool Completed {
      get { return completed; }
    }

    /// <summary>
    /// Return the user's score for the current level.
    /// </summary>
    public int LevelScore {
      get { return levelScore; }
    }

    /// <summary>
    /// Return the numnber identifier of the current level.
    /// </summary>
    public int LevelNumber {
      get { return LevelNumber; }
    }

    /// <summary>
    /// Return the time spent on completing a level with the highest score.
    /// </summary>
    public string CompletionTime {
      get { return completionTime; }
    }

    #endregion
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="completed"></param>
    /// <param name="levelScore"></param>
    /// <param name="levelNumber"></param>
    /// <param name="completionTime"></param>
    public LevelStatusScreen(bool completed, int levelScore, int levelNumber, string completionTime)
    : base("Level " + levelNumber)
    {

      this.completed = completed;
      this.levelScore = levelScore;
      this.levelNumber = levelNumber;
      this.completionTime = completionTime;

      //Create our menu entries
      startMenuEntry = new MenuEntry("Start");
      exitMenuEntry = new MenuEntry("Back");

      //Hook up menu event handlers
      startMenuEntry.Selected += StartMenuEntrySelected;
      exitMenuEntry.Selected += OnCancel;

      //Add entries to the menu
      MenuEntries.Add(startMenuEntry);
      MenuEntries.Add(exitMenuEntry);
    }

    #region Update and Draw

    /// <summary>
    /// Update the MenuEntry's location.
    /// </summary>
    protected override void UpdateMenuEntryLocations() {
      base.UpdateMenuEntryLocations();
      GraphicsDevice graphics = ScreenManager.GraphicsDevice;
      // start at Y = 80; start at the center of the screen
      Vector2 position = new Vector2(graphics.Viewport.Width / 4, 400);

      exitMenuEntry.Position = position;

      position.X = position.X + 400;

      startMenuEntry.Position = position;
    }

    /// <summary>
    /// Draw onto the screen the level's completion status,
    /// the time it took to complete, and the score obtained
    /// by the user.
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Draw(GameTime gameTime) {
      base.Draw(gameTime);
      GraphicsDevice graphics = ScreenManager.GraphicsDevice;
      SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
      SpriteFont font = ScreenManager.Font;

      spriteBatch.Begin();

      // Make the menu slide into place during transitions, using a
      // power curve to make things look more interesting (this makes
      // the movement slow down as it nears the end).
      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

      // Draw the menu title centered on the screen
      Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
      Vector2 titleOrigin = font.MeasureString("Competion Time: 0:00") / 2;
      Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
      float titleScale = 1.25f;
      titlePosition.Y -= transitionOffset * 100;

      // Make space between the menu title and the level information
      titlePosition.Y = titlePosition.Y + font.LineSpacing * 2;

      // Draw the level information to the screen
      spriteBatch.DrawString(font, "Status: " + (completed ? "Completed" : "Incomplete"), titlePosition,
                             Color.White, 0, titleOrigin, titleScale, SpriteEffects.None, 0);
      
      titlePosition.Y = titlePosition.Y + font.LineSpacing * 2;

      spriteBatch.DrawString(font, "Completion Time: " + completionTime, titlePosition, Color.White, 
                             0, titleOrigin, titleScale, SpriteEffects.None, 0);

      titlePosition.Y = titlePosition.Y + font.LineSpacing * 2;

      spriteBatch.DrawString(font, "Score: " + levelScore, titlePosition, Color.White,
                             0, titleOrigin, titleScale, SpriteEffects.None, 0);
      spriteBatch.End();
    }

    #endregion

#region Handle Input

    /// <summary>
    /// Event handler for when the Start menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void StartMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                         new GameplayScreen());
    }

#endregion
  }
}
