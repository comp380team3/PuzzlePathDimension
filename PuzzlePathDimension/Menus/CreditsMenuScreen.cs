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

    /// <summary>
    /// List of Team members for Puzzle Path.
    /// </summary>
    string[] TeamMembers = new string[] {
      "Chris Babayas",
      "Jonathan Castello",
      "Brian Marroquin",
      "Jorenz Paragas",
      "Michael Sandoval",
    };

    /// <summary>
    /// List of organizations that contributed to Puzzle Path.
    /// </summary>
    string[] Organizations = new string[] {
      "Microsoft XNA Community Game Platform",
      "Farseer Physics Engine",
    };

    /// <summary>
    /// List of individuals that contributed to Puzzle Path.
    /// </summary>
    string[] Individuals = new string[] { };

    DetailsTemplate detailsTemplate = new DetailsTemplate();

    ///<summary>
    ///Constructor
    ///<summary>
    public CreditsMenuScreen(TopLevelModel topLevel)
      : base(topLevel) {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    /// <summary>
    /// Load the buttons and the Content that will be used to display the Credits.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont titleFont = shared.Load<SpriteFont>("Font/menufont");
      SpriteFont textFont = shared.Load<SpriteFont>("Font/textfont");

      detailsTemplate.Title = new TextLine("Credits", titleFont, new Color(192, 192, 192));

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

    protected override void OnButtonReleased(VirtualButtons button) {
      switch (button) {
      case VirtualButtons.Left:
        detailsTemplate.SelectPrev();
        break;
      case VirtualButtons.Right:
        detailsTemplate.SelectNext();
        break;
      case VirtualButtons.Select:
        detailsTemplate.Confirm();
        break;
      case VirtualButtons.Delete:
        OnCancel();
        break;
      }
    }

    protected override void OnPointChanged(Point point) {
      detailsTemplate.SelectAtPoint(point);
    }

    /// <summary>
    /// Update the Screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="otherScreenHasFocus"></param>
    /// <param name="coveredByOtherScreen"></param>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      detailsTemplate.TransitionPosition = TransitionPosition;
      detailsTemplate.Update(gameTime);
    }

    /// <summary>
    /// Draw the Credits title, members, organizations, and contributers to the screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      detailsTemplate.Draw(spriteBatch, gameTime);
    }

    /// <summary>
    /// Event handler for when the Exit Menu Entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnCancel() {
      ExitScreen();
    }
  }
}
