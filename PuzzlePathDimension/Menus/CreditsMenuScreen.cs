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
  class CreditsMenuScreen : GameScreen {
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

    DetailsTemplate detailsTemplate = new DetailsTemplate();

    ///<summary>
    ///Constructor
    ///<summary>
    public CreditsMenuScreen() {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont titleFont = shared.Load<SpriteFont>("menufont");
      SpriteFont textFont = shared.Load<SpriteFont>("textfont");

      detailsTemplate.Title = new TextLine("Credits", titleFont, new Color(192, 192, 192));
      detailsTemplate.Cancelled += OnCancel;

      MenuButton exitMenuEntry = new MenuButton("Back", titleFont);
      exitMenuEntry.Selected += OnCancel;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Middle] = exitMenuEntry;
      detailsTemplate.SelectedItem = DetailsTemplate.Selection.Middle;


      IList<IMenuLine> credits = detailsTemplate.Lines;
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

    public override void HandleInput(VirtualController vtroller) {
      base.HandleInput(vtroller);

      if (vtroller.CheckForRecentRelease(VirtualButtons.Left)) {
        detailsTemplate.SelectPrev();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Right)) {
        detailsTemplate.SelectNext();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        detailsTemplate.Confirm();
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        detailsTemplate.Cancel();
      }
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      detailsTemplate.TransitionPosition = TransitionPosition;
      detailsTemplate.Update(true, gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      detailsTemplate.Draw(spriteBatch, true, gameTime);
    }

    protected void OnCancel(object sender, PlayerIndexEventArgs e) {
      ExitScreen();
    }
  }
}
