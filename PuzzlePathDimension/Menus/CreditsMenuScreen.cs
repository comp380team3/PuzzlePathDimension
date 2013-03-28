using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  /// <summary>
  /// The credits screen displays the individuals and organizations
  /// that contributed toward the games completion.
  /// </summary>
  class CreditsMenuScreen : MenuScreen {
    /// <summary>
    /// The team members of Puzzle Path
    /// </summary>
    private string[] teamMembersMenuEntry;

    ///<summary>
    ///The organizations that contributed to Puzzle Path
    ///</summary>
    private string[] organizationsMenuEntry;

    ///<summary>
    ///Individuals that have contributed to Puzzle Path
    ///</summary>
    private string[] individualContributorMenuEntry;

    ///<summary>
    ///An exit menu entry to return to the previous screen
    ///</summary>
    MenuEntry exitMenuEntry;

    #region Initialize
    ///<summary>
    ///Constructor
    ///<summary>
    public CreditsMenuScreen()
      : base("Credits") {

      teamMembersMenuEntry = new string[] { "Chris Babayas", "Jonathan Catello", "Brian Marroquin",
                                                  "Jorenz Paragas", "Michael Sandoval"};
      organizationsMenuEntry = new string[] { "Microsoft XNA Community Game Platform" };

      individualContributorMenuEntry = new string[] { "" };

      // Create a exit menu entry
      exitMenuEntry = new MenuEntry("Back");
      // Add entry to the menu
      MenuEntries.Add(exitMenuEntry);

      exitMenuEntry.Selected += OnCancel;

    }

    #endregion

    #region Update and Draw

    /// <summary>
    /// Update the MenuEntry's location. The location will be after the
    /// list of names that are given credit for the work applied to the game.
    /// </summary>
    protected override void UpdateMenuEntryLocations() {
      base.UpdateMenuEntryLocations();
      GraphicsDevice graphics = ScreenManager.GraphicsDevice;
      // start at Y = 80; start at the center of the screen
      Vector2 position = new Vector2(graphics.Viewport.Width / 2, 80);
      int numberOfNames = teamMembersMenuEntry.Length + organizationsMenuEntry.Length + individualContributorMenuEntry.Length;

      for (int i = 0; i < numberOfNames + 2; i++) {
        position.Y = position.Y + ScreenManager.Font.LineSpacing;
      }

      exitMenuEntry.Position = position;
    }

    /// <summary>
    /// Draw onto the screen the names in the members list,
    /// organizations list, and the individual contributions list.
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Draw(GameTime gameTime) {
      base.Draw(gameTime);
      GraphicsDevice graphics = ScreenManager.GraphicsDevice;
      SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
      SpriteFont font = ScreenManager.TextFont;

      spriteBatch.Begin();

      // Make the menu slide into place during transitions, using a
      // power curve to make things look more interesting (this makes
      // the movement slow down as it nears the end).
      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

      // Draw the menu title centered on the screen
      Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
      Vector2 titleOrigin = font.MeasureString(organizationsMenuEntry[0]) / 2;
      Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
      float titleScale = 1.25f;
      titlePosition.Y -= transitionOffset * 100;

      // Make space between the menu title and the Team member title
      titlePosition.Y = titlePosition.Y + font.LineSpacing * 2;

      // Draw the list of team members to the screen
      spriteBatch.DrawString(font, "Team Members", titlePosition, Color.White, 0,
                                titleOrigin, titleScale, SpriteEffects.None, 0);

      titlePosition.Y = titlePosition.Y + 2 * font.LineSpacing;

      for (int i = 0; i < teamMembersMenuEntry.Length; i++) {
        spriteBatch.DrawString(font, teamMembersMenuEntry[i], titlePosition, Color.Black, 0,
                                  titleOrigin, titleScale, SpriteEffects.None, 0);
        titlePosition.Y = titlePosition.Y + font.LineSpacing;
      }

      // Make space in between the team member title and the organization title
      titlePosition.Y = titlePosition.Y + 2 * font.LineSpacing;

      // Draw the list of Organizations to the screen
      spriteBatch.DrawString(font, "Organizations", titlePosition, Color.White, 0,
                                titleOrigin, titleScale, SpriteEffects.None, 0);

      titlePosition.Y = titlePosition.Y + 2 * font.LineSpacing;

      for (int i = 0; i < organizationsMenuEntry.Length; i++) {
        spriteBatch.DrawString(font, organizationsMenuEntry[i], titlePosition, Color.Black, 0,
                               titleOrigin, titleScale, SpriteEffects.None, 0);
        titlePosition.Y = titlePosition.Y + font.LineSpacing;
      }

      // Draw the list of individuals who contributed to the Puzzle Path game
      for (int i = 0; i < individualContributorMenuEntry.Length; i++) {
        spriteBatch.DrawString(font, individualContributorMenuEntry[i], titlePosition, Color.Black, 0,
                                titleOrigin, titleScale, SpriteEffects.None, 0);
        titlePosition.Y = titlePosition.Y + font.LineSpacing;
      }

      spriteBatch.End();
    }

    #endregion

  }
}
