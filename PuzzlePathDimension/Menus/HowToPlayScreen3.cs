using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  class HowToPlayScreen3 : GameScreen {
    DetailsTemplate detailsTemplate = new DetailsTemplate();

    /// <summary>
    /// Exit menu entry on the screen.
    /// </summary>
    MenuButton exitMenuEntry;

    /// <summary>
    /// Description for the Controls of the game.
    /// </summary>
    string[] ControlScheme = new string[] {
      " Simulation ",
      "Rotate Launcher Left:        Left Arrow Key/ Left on DPad",
      "Rotate Launcher Right:       Right Arrow Key/ Right on DPad",
      "Increase Magnitude:          Up Arrow Key/ Up on DPad",
      "Decrease Magnitude:          Down Arrow Key/ Down on DPad",
      "Launch Ball:                 Space Bar/ A Button ",
      "Pause Game:                  Esc/ Start Button",
      " Editor ",
      "Open Toolbox :                    T Key/ X Button        ",
      "Select Platform:             Left Click on Mouse/ A Button ",
      "Enter Simulation Mode:       Enter/Start                     ",
    };


    /// <summary>
    /// Contructor
    /// </summary>
    public HowToPlayScreen3(TopLevelModel topLevel)
      : base(topLevel) {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    /// <summary>
    /// Load content that will be used to create the help screen.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont titleFont = shared.Load<SpriteFont>("Font/menufont");

      detailsTemplate.Title = new TextLine("Controls", titleFont, new Color(192, 192, 192));

      IList<IMenuLine> controlScheme = detailsTemplate.Lines;
      controlScheme.Clear();

      foreach (string name in ControlScheme)
        controlScheme.Add(new TextLine(name, titleFont, Color.Black, .75f));

      exitMenuEntry = new MenuButton("Back", titleFont);
      exitMenuEntry.Selected += OnCancel;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Middle] = exitMenuEntry;
      detailsTemplate.SelectedItem = DetailsTemplate.Selection.Middle;
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
      case VirtualButtons.Context:
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
    /// Draw the control scheme to the Screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);
      detailsTemplate.Draw(spriteBatch, gameTime);
    }

    /// <summary>
    /// Event handler for when the Exit menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnCancel() {
      ExitScreen();
    }
  }
}
