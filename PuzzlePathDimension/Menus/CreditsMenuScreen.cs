using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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
    MenuButton exitMenuEntry = new MenuButton("Back");

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

      IList<IMenuLine> credits = creditsEntry.Lines;
      credits.Clear();

      credits.Add(new TextLine("Team Members", TextFont, Color.White, 1.25f));
      credits.Add(new Spacer(TextFont.LineSpacing));
      foreach (string name in TeamMembers)
        credits.Add(new TextLine(name, TextFont, Color.Black, 1.25f));
      credits.Add(new Spacer(TextFont.LineSpacing));

      credits.Add(new Spacer(TextFont.LineSpacing));

      credits.Add(new TextLine("Organizations", TextFont, Color.White, 1.25f));
      credits.Add(new Spacer(TextFont.LineSpacing));
      foreach (string name in Organizations)
        credits.Add(new TextLine(name, TextFont, Color.Black, 1.25f));
      foreach (string name in Individuals)
        credits.Add(new TextLine(name, TextFont, Color.Black, 1.25f));
    }
  }
}
