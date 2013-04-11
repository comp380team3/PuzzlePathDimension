using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  interface ICredit {
    int Draw(SpriteBatch spriteBatch, Vector2 pos, GameTime gameTime);
  }

  class Spacer : ICredit {
    private int Height { get; set; }

    public Spacer(int height) {
      Height = height;
    }

    public int Draw(SpriteBatch spriteBatch, Vector2 pos, GameTime gameTime) {
      return Height;
    }
  }

  class Line : ICredit {
    public Color TextColor { get; set; }
    private SpriteFont Font { get; set; }
    private string Text { get; set; }

    public Line(String text, SpriteFont font, Color textColor) {
      Text = text;
      TextColor = textColor;
      Font = font;
    }

    public int Draw(SpriteBatch spriteBatch, Vector2 pos, GameTime gameTime) {
      Vector2 origin = new Vector2(0, Font.MeasureString(Text).Y / 2);

      spriteBatch.DrawString(Font, Text, pos, TextColor, 0,
                                origin, 1.25f, SpriteEffects.None, 0);
      return Font.LineSpacing;
    }
  }

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
      SpriteFont font = screen.TextFont;
      float width = 0.0f;

      width = Math.Max(width, font.MeasureString("Team Members").X);
      foreach (string name in TeamMembers)
        width = Math.Max(width, font.MeasureString(name).X);

      width = Math.Max(width, font.MeasureString("Organizations").X);
      foreach (string name in Organizations)
        width = Math.Max(width, font.MeasureString(name).X);

      foreach (string name in Individuals)
        width = Math.Max(width, font.MeasureString(name).X);

      return (int)Math.Round(width);
    }

    public int GetHeight(MenuScreen screen) {
      int numberOfNames = TeamMembers.Length
                        + Organizations.Length
                        + Individuals.Length;

      numberOfNames += 8; // for the spacing

      return numberOfNames * screen.TextFont.LineSpacing;
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
      SpriteFont textFont = screen.TextFont;

      List<ICredit> items = new List<ICredit>();

      items.Add(new Line("Team Members", textFont, Color.White));
      items.Add(new Spacer(textFont.LineSpacing));
      foreach (string name in TeamMembers)
        items.Add(new Line(name, textFont, Color.Black));
      items.Add(new Spacer(textFont.LineSpacing));

      items.Add(new Spacer(textFont.LineSpacing));

      items.Add(new Line("Organizations", textFont, Color.White));
      items.Add(new Spacer(textFont.LineSpacing));
      foreach (string name in Organizations)
        items.Add(new Line(name, textFont, Color.Black));
      foreach (string name in Individuals)
        items.Add(new Line(name, textFont, Color.Black));

      Vector2 position = Position;
      foreach (ICredit credit in items)
        position.Y += credit.Draw(spriteBatch, position, gameTime);
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
