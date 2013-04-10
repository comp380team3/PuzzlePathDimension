using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  class CreditsEntry : IMenuEntry {
    public event EventHandler<PlayerIndexEventArgs> Selected;

    float selectionFade;

    public Vector2 Position { get; set; }

    public string[] TeamMembers { get; set; }
    public string[] Organizations { get; set; }
    public string[] Individuals { get; set; }

    /// <summary>
    /// Method for raising the Selected event.
    /// </summary>
    public void OnSelectEntry(PlayerIndex playerIndex) {
      if (Selected != null)
        Selected(this, new PlayerIndexEventArgs(playerIndex));
    }

    public int GetWidth(MenuScreen screen) {
      return 100;
    }

    public int GetHeight(MenuScreen screen) {
      int numberOfNames = TeamMembers.Length
                        + Organizations.Length
                        + Individuals.Length;

      return (numberOfNames + 8) * screen.TextFont.LineSpacing;
    }

    public void Update(MenuScreen screen, bool isSelected, GameTime gameTime) {
      // When the menu selection changes, entries gradually fade between
      // their selected and deselected appearance, rather than instantly
      // popping to the new state.
      float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

      if (isSelected)
        selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
      else
        selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
    }

    /// <summary>
    /// Draw onto the screen the names in the members list,
    /// organizations list, and the individual contributions list.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Draw(MenuScreen screen, SpriteBatch spriteBatch, bool isSelected, GameTime gameTime) {
      Viewport viewport = spriteBatch.GraphicsDevice.Viewport;

      SpriteFont textFont = screen.TextFont;
      SpriteFont titleFont = screen.TitleFont;

      Color titleColor = new Color(192, 192, 192) * screen.TransitionAlpha;

      // Make the menu slide into place during transitions, using a
      // power curve to make things look more interesting (this makes
      // the movement slow down as it nears the end).
      float transitionOffset = (float)Math.Pow(screen.TransitionPosition, 2);
      const float scale = 1.25f;

      Vector2 origin = textFont.MeasureString(Organizations[0]) / 2;
      Vector2 position = new Vector2(viewport.Width / 2, 80);
      position.Y -= transitionOffset * 100;

      // Make space between the menu title and the Team member title
      position.Y += 2 * textFont.LineSpacing;

      // Draw the list of team members to the screen
      spriteBatch.DrawString(textFont, "Team Members", position, Color.White, 0,
                                origin, scale, SpriteEffects.None, 0);

      position.Y += 2 * textFont.LineSpacing;

      for (int i = 0; i < TeamMembers.Length; i++) {
        spriteBatch.DrawString(textFont, TeamMembers[i], position, Color.Black, 0,
                                  origin, scale, SpriteEffects.None, 0);
        position.Y += textFont.LineSpacing;
      }

      // Make space in between the team member title and the organization title
      position.Y += 2 * textFont.LineSpacing;

      // Draw the list of Organizations to the screen
      spriteBatch.DrawString(textFont, "Organizations", position, Color.White, 0,
                                origin, scale, SpriteEffects.None, 0);

      position.Y += 2 * textFont.LineSpacing;

      for (int i = 0; i < Organizations.Length; i++) {
        spriteBatch.DrawString(textFont, Organizations[i], position, Color.Black, 0,
                               origin, scale, SpriteEffects.None, 0);
        position.Y += textFont.LineSpacing;
      }

      // Draw the list of individuals who contributed to the Puzzle Path game
      for (int i = 0; i < Individuals.Length; i++) {
        spriteBatch.DrawString(textFont, Individuals[i], position, Color.Black, 0,
                                origin, scale, SpriteEffects.None, 0);
        position.Y += textFont.LineSpacing;
      }
    }
  }

  /// <summary>
  /// The credits screen displays the individuals and organizations
  /// that contributed toward the games completion.
  /// </summary>
  class CreditsMenuScreen : MenuScreen {
    ///<summary>
    ///Constructor
    ///<summary>
    public CreditsMenuScreen()
        : base("Credits") {
      CreditsEntry creditsEntry = new CreditsEntry();
      creditsEntry.TeamMembers = new string[] {
        "Chris Babayas",
        "Jonathan Castello",
        "Brian Marroquin",
        "Jorenz Paragas",
        "Michael Sandoval",
      };
      creditsEntry.Organizations = new string[] {
        "Microsoft XNA Community Game Platform",
      };
      creditsEntry.Individuals = new string[] {};
      creditsEntry.Selected += OnCancel;
      MenuEntries.Add(creditsEntry);

      MenuEntry exitMenuEntry = new MenuEntry("Back");
      exitMenuEntry.Selected += OnCancel;
      MenuEntries.Add(exitMenuEntry);
    }
  }
}
