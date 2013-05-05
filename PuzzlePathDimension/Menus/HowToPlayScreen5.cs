using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  /// <summary>
  /// The first set of three How To Play Screens. Describes
  /// the basic concepts of the Puzzle Path game and gives a
  /// brief description of the basic actions available in the game.
  /// </summary>
  class HowToPlayScreen5 : GameScreen {
    DetailsTemplate detailsTemplate = new DetailsTemplate();

    /// <summary>
    /// Menu Button for the Exit menu entry.
    /// </summary>
    MenuButton exitMenuEntry;

    /// <summary>
    /// Description of the concepts and objectives that surround Puzzle Path.
    /// </summary>
    string[] gameDescription = new string[] { 
      "The level editor allows the player to modify existing levels ",
      "by adding breakable and non-breakable platforms. The reason for this ",
      "is to allow the player to create new paths in which the ball could ",
      "travel. The player is given a toolbox in which they could select from ",
      "a variety of platforms that would be added to the level. The player ",
      "will be able to move the platforms around and place them in a ",
      "desired position within the boundaries of the level. This game ",
      "contains one custom level. The level is initialy blank and the player ",
      "can create and store their own custom level. The player will be given ",
      "a toolbox with more game elements such as treasures and deathtraps. "
    };

    /// <summary>
    /// Contructor
    /// </summary>
    public HowToPlayScreen5(TopLevelModel topLevel)
      : base(topLevel){
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    /// <summary>
    /// Load content the will be used to create the help menu screen.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("Font/menufont");

      detailsTemplate.Title = new TextLine("Level Editor", font, new Color(192, 192, 192));

      IList<IMenuLine> description = detailsTemplate.Lines;
      description.Clear();

      foreach (string name in gameDescription)
        description.Add(new TextLine(name, font, Color.Black, .75f));

      exitMenuEntry = new MenuButton("Exit", font);
      exitMenuEntry.Selected += OnCancel;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Middle] = exitMenuEntry;
      detailsTemplate.SelectedItem = DetailsTemplate.Selection.Middle;
    }

    /// <summary>
    /// Handle the input of the user. If the user wants to move
    /// to a diffenrent menu entry, they can press left or right.
    /// </summary>
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
    /// Draw the game's description and the Buttons to the Screen.
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
