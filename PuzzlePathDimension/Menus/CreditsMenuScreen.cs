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

    int Height { get; }
    int Width { get; }
  }

  class Spacer : ICredit {
    public int Height { get; private set; }

    public int Width {
      get {
        return 0;
      }
    }

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

    public int Width {
      get {
        return (int)Font.MeasureString(Text).X;
      }
    }

    public int Height {
      get {
        return (int)Font.MeasureString(Text).Y;
      }
    }

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
    public IList<ICredit> Credits { get; private set; }

    public int Height {
      get {
        return Credits.Aggregate(0, (acc, credit) => acc + credit.Height);
      }
    }

    public int Width {
      get {
        return Credits.Aggregate(0, (acc, credit) => Math.Max(acc, credit.Width));
      }
    }

    public CreditsEntry() {
      Credits = new List<ICredit>();
    }

    /// <summary>
    /// Method for raising the Selected event.
    /// </summary>
    public void OnSelectEntry(PlayerIndex playerIndex) {
      if (Selected != null)
        Selected(this, new PlayerIndexEventArgs(playerIndex));
    }

    public int GetWidth(MenuScreen screen) {
      return Width;
    }

    public int GetHeight(MenuScreen screen) {
      return Height;
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
      Vector2 position = Position;
      foreach (ICredit credit in Credits)
        position.Y += credit.Draw(spriteBatch, position, gameTime);
    }
  }

  /// <summary>
  /// The credits screen displays the individuals and organizations
  /// that contributed toward the games completion.
  /// </summary>
  class CreditsMenuScreen : MenuScreen {
    string[] TeamMembers = new string[] {
      "Chris Babayas",
      "Jonathan Castello",
      "Brian Marroquin",
      "Jorenz Paragas",
      "Michael Sandoval",
    };

    string[] Organizations = new string[] {
      "Microsoft XNA Community Game Platform",
    };

    string[] Individuals = new string[] { };


    CreditsEntry creditsEntry = new CreditsEntry();
    MenuEntry exitMenuEntry = new MenuEntry("Back");

    ///<summary>
    ///Constructor
    ///<summary>
    public CreditsMenuScreen()
        : base("Credits") {
      creditsEntry.Selected += OnCancel;
      exitMenuEntry.Selected += OnCancel;

      MenuEntries.Add(creditsEntry);
      MenuEntries.Add(exitMenuEntry);
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);

      IList<ICredit> credits = creditsEntry.Credits;
      credits.Clear();

      credits.Add(new Line("Team Members", TextFont, Color.White));
      credits.Add(new Spacer(TextFont.LineSpacing));
      foreach (string name in TeamMembers)
        credits.Add(new Line(name, TextFont, Color.Black));
      credits.Add(new Spacer(TextFont.LineSpacing));

      credits.Add(new Spacer(TextFont.LineSpacing));

      credits.Add(new Line("Organizations", TextFont, Color.White));
      credits.Add(new Spacer(TextFont.LineSpacing));
      foreach (string name in Organizations)
        credits.Add(new Line(name, TextFont, Color.Black));
      foreach (string name in Individuals)
        credits.Add(new Line(name, TextFont, Color.Black));
    }
  }
}
