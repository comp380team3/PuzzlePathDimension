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
    string[] teamMembersMenuEntry = new string[] {
      "Chris Babayas",
      "Jonathan Castello",
      "Brian Marroquin",
      "Jorenz Paragas",
      "Michael Sandoval",
    };

    ///<summary>
    ///The organizations that contributed to Puzzle Path
    ///</summary>
    string[] organizationsMenuEntry = new string[] {
      "Microsoft XNA Community Game Platform",
    };

    ///<summary>
    ///Individuals that have contributed to Puzzle Path
    ///</summary>
    string[] individualContributorMenuEntry = new string[] { "" };

    ///<summary>
    ///An exit menu entry to return to the previous screen
    ///</summary>
    MenuEntry exitMenuEntry = new MenuEntry("Back");

    ///<summary>
    ///Constructor
    ///<summary>
    public CreditsMenuScreen()
        : base("Credits") {
      // Add entry to the menu
      MenuEntries.Add(exitMenuEntry);

      exitMenuEntry.Selected += OnCancel;
    }


    /// <summary>
    /// Update the MenuEntry's location. The location will be after the
    /// list of names that are given credit for the work applied to the game.
    /// </summary>
    protected override void UpdateMenuEntryLocations() {
      base.UpdateMenuEntryLocations();

      // TODO: Replace physical screen viewport with virtual coordinate system.
      Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

      // start at Y = 80; start at the center of the screen
      int numberOfNames = teamMembersMenuEntry.Length
                        + organizationsMenuEntry.Length
                        + individualContributorMenuEntry.Length;

      exitMenuEntry.Position = new Vector2(
        viewport.Width / 2,
        80 + (numberOfNames + 3) * TitleFont.LineSpacing
      );
    }

    /// <summary>
    /// Draw onto the screen the names in the members list,
    /// organizations list, and the individual contributions list.
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      Viewport viewport = spriteBatch.GraphicsDevice.Viewport;

      spriteBatch.Begin();

      // Make the menu slide into place during transitions, using a
      // power curve to make things look more interesting (this makes
      // the movement slow down as it nears the end).
      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

      // Draw the menu title centered on the screen
      Vector2 titlePosition = new Vector2(viewport.Width / 2, 80);
      Vector2 titleOrigin = TextFont.MeasureString(organizationsMenuEntry[0]) / 2;
      Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
      float titleScale = 1.25f;
      titlePosition.Y -= transitionOffset * 100;

      // Make space between the menu title and the Team member title
      titlePosition.Y += TextFont.LineSpacing * 2;

      // Draw the list of team members to the screen
      spriteBatch.DrawString(TextFont, "Team Members", titlePosition, Color.White, 0,
                                titleOrigin, titleScale, SpriteEffects.None, 0);

      titlePosition.Y += 2 * TextFont.LineSpacing;

      for (int i = 0; i < teamMembersMenuEntry.Length; i++) {
        spriteBatch.DrawString(TextFont, teamMembersMenuEntry[i], titlePosition, Color.Black, 0,
                                  titleOrigin, titleScale, SpriteEffects.None, 0);
        titlePosition.Y += TextFont.LineSpacing;
      }

      // Make space in between the team member title and the organization title
      titlePosition.Y += 2 * TextFont.LineSpacing;

      // Draw the list of Organizations to the screen
      spriteBatch.DrawString(TextFont, "Organizations", titlePosition, Color.White, 0,
                                titleOrigin, titleScale, SpriteEffects.None, 0);

      titlePosition.Y += 2 * TextFont.LineSpacing;

      for (int i = 0; i < organizationsMenuEntry.Length; i++) {
        spriteBatch.DrawString(TextFont, organizationsMenuEntry[i], titlePosition, Color.Black, 0,
                               titleOrigin, titleScale, SpriteEffects.None, 0);
        titlePosition.Y += TextFont.LineSpacing;
      }

      // Draw the list of individuals who contributed to the Puzzle Path game
      for (int i = 0; i < individualContributorMenuEntry.Length; i++) {
        spriteBatch.DrawString(TextFont, individualContributorMenuEntry[i], titlePosition, Color.Black, 0,
                                titleOrigin, titleScale, SpriteEffects.None, 0);
        titlePosition.Y += TextFont.LineSpacing;
      }

      spriteBatch.End();
    }
  }
}
