using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
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

    LinesTemplate creditsEntry = new LinesTemplate();

    ///<summary>
    ///Constructor
    ///<summary>
    public CreditsMenuScreen() : base("") {
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont titleFont = shared.Load<SpriteFont>("menufont");
      SpriteFont textFont = shared.Load<SpriteFont>("textfont");

      creditsEntry.Title = new TextLine("Credits", titleFont, new Color(192, 192, 192));
      creditsEntry.Selected += OnCancel;
      MenuEntries.Add(creditsEntry);

      MenuButton exitMenuEntry = new MenuButton("Back", titleFont);
      exitMenuEntry.Selected += OnCancel;
      MenuEntries.Add(exitMenuEntry);


      IList<IMenuLine> credits = creditsEntry.Lines;
      credits.Clear();

      credits.Add(new TextLine("Team Members", textFont, Color.White, 1.25f));
      credits.Add(new Spacer(textFont.LineSpacing));
      foreach (string name in TeamMembers)
        credits.Add(new TextLine(name, textFont, Color.Black, 1.25f));
      credits.Add(new Spacer(textFont.LineSpacing));

      credits.Add(new Spacer(textFont.LineSpacing));

      credits.Add(new TextLine("Organizations", textFont, Color.White, 1.25f));
      credits.Add(new Spacer(textFont.LineSpacing));
      foreach (string name in Organizations)
        credits.Add(new TextLine(name, textFont, Color.Black, 1.25f));
      foreach (string name in Individuals)
        credits.Add(new TextLine(name, textFont, Color.Black, 1.25f));
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      creditsEntry.TransitionPosition = TransitionPosition;
      base.Draw(gameTime, spriteBatch);
    }
  }
}
