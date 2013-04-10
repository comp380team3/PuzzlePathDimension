using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  class LevelStatusScreen : MenuScreen {
    MenuEntry startMenuEntry = new MenuEntry("Start");
    MenuEntry exitMenuEntry = new MenuEntry("Back");

    /// <summary>
    /// Return true if level is completed, otherwise false.
    /// </summary>
    public bool Completed { get; private set; }

    /// <summary>
    /// Return the user's score for the current level.
    /// </summary>
    public int LevelScore { get; private set; }

    /// <summary>
    /// Return the numnber identifier of the current level.
    /// </summary>
    public int LevelNumber { get; private set; }

    /// <summary>
    /// Return the time spent on completing a level with the highest score.
    /// </summary>
    public string CompletionTime { get; private set; }


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="completed"></param>
    /// <param name="levelScore"></param>
    /// <param name="levelNumber"></param>
    /// <param name="completionTime"></param>
    public LevelStatusScreen(bool completed, int levelScore, int levelNumber, string completionTime)
        : base("Level " + levelNumber) {
      Completed = completed;
      LevelScore = levelScore;
      LevelNumber = levelNumber;
      CompletionTime = completionTime;

      startMenuEntry.Selected += StartMenuEntrySelected;
      MenuEntries.Add(startMenuEntry);

      exitMenuEntry.Selected += OnCancel;
      MenuEntries.Add(exitMenuEntry);
    }


    /// <summary>
    /// Update the MenuEntry's location.
    /// </summary>
    protected override void UpdateMenuEntryLocations() {
      base.UpdateMenuEntryLocations();

      // TODO: Use virtual coordinate system instead of physical screen viewport.
      Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

      // start at Y = 80; start at the center of the screen
      Vector2 position = new Vector2(viewport.Width / 4, 400);
      exitMenuEntry.Position = position;

      position.X += 400;
      startMenuEntry.Position = position;
    }

    /// <summary>
    /// Draw onto the screen the level's completion status,
    /// the time it took to complete, and the score obtained
    /// by the user.
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      Viewport viewport = spriteBatch.GraphicsDevice.Viewport;
      SpriteFont font = base.TitleFont;

      spriteBatch.Begin();

      // Make the menu slide into place during transitions, using a
      // power curve to make things look more interesting (this makes
      // the movement slow down as it nears the end).
      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

      // Draw the menu title centered on the screen
      Vector2 titlePosition = new Vector2(viewport.Width / 2, 80);
      Vector2 titleOrigin = font.MeasureString("Competion Time: 0:00") / 2;
      Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
      float titleScale = 1.25f;
      titlePosition.Y -= transitionOffset * 100;

      // Make space between the menu title and the level information
      titlePosition.Y = titlePosition.Y + font.LineSpacing * 2;

      // Draw the level information to the screen
      spriteBatch.DrawString(font, "Status: " + (Completed ? "Completed" : "Incomplete"), titlePosition,
                             Color.White, 0, titleOrigin, titleScale, SpriteEffects.None, 0);
      
      titlePosition.Y += font.LineSpacing * 2;

      spriteBatch.DrawString(font, "Completion Time: " + CompletionTime, titlePosition, Color.White, 
                             0, titleOrigin, titleScale, SpriteEffects.None, 0);

      titlePosition.Y += font.LineSpacing * 2;

      spriteBatch.DrawString(font, "Score: " + LevelScore, titlePosition, Color.White,
                             0, titleOrigin, titleScale, SpriteEffects.None, 0);
      spriteBatch.End();
    }

    public override void HandleInput(VirtualController vtroller) {
      if (vtroller.CheckForRecentRelease(VirtualButtons.Left)) {
        SelectedEntry -= 1;

        if (SelectedEntry < 0)
          SelectedEntry = MenuEntries.Count - 1;
      }

      // Move to the next menu entry?
      if (vtroller.CheckForRecentRelease(VirtualButtons.Right)) {
        SelectedEntry += 1;

        if (SelectedEntry >= MenuEntries.Count)
          SelectedEntry = 0;
      }

      // Accept or cancel the menu? We pass in our ControllingPlayer, which may
      // either be null (to accept input from any player) or a specific index.
      // If we pass a null controlling player, the InputState helper returns to
      // us which player actually provided the input. We pass that through to
      // OnSelectEntry and OnCancel, so they can tell which player triggered them.

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        OnSelectEntry(SelectedEntry, PlayerIndex.One);
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        OnCancel(PlayerIndex.One);
      }
    }

    /// <summary>
    /// Event handler for when the Start menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void StartMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      LoadingScreen.Load(ScreenList, true, e.PlayerIndex,
                         new GameplayScreen());
    }
  }
}
