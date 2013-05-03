using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  class LevelStatusScreen : GameScreen {
    DetailsTemplate detailsTemplate = new DetailsTemplate();

    /// <summary>
    /// Menu Button for the Start menu entry.
    /// </summary>
    MenuButton startMenuEntry;

    /// <summary>
    /// Menu Button for the Exit menu entry.
    /// </summary>
    MenuButton exitMenuEntry;

    /// <summary>
    /// Return true if level is completed, otherwise false.
    /// </summary>
    public bool Completed { get; private set; }

    /// <summary>
    /// Return the user's score for the current level.
    /// </summary>
    public int LevelScore { get; private set; }

    /// <summary>
    /// Return the numnber identifier of the current level.
    /// </summary>
    public string LevelName { get; private set; }

    /// <summary>
    /// Return the time spent on completing a level with the highest score.
    /// </summary>
    public int CompletionTime { get; private set; }

    public string LevelFileName { get; private set; }

    private SpriteFont Font { get; set; }


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="completed"></param>
    /// <param name="levelScore"></param>
    /// <param name="levelNumber"></param>
    /// <param name="completionTime"></param>
    public LevelStatusScreen(TopLevelModel topLevel, PuzzlePathDimension.LevelSelectScreen.LevelInfo levelInfo)
      : base(topLevel) {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);

      Completed = levelInfo.Completed;
      LevelScore = levelInfo.LevelScore;
      LevelName = levelInfo.LevelName;
      CompletionTime = levelInfo.CompletionTime;
      LevelFileName = levelInfo.FileName;
    }

    /// <summary>
    /// Load the content that will be used to create the screen.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      Font = shared.Load<SpriteFont>("Font/menufont");
      int numberOfMinutes = CompletionTime / 60;
      int numberOfSeconds = CompletionTime % 60;

      string minutes;
      string seconds;

      if (numberOfMinutes < 10) {
        minutes = "0" + numberOfMinutes;
      } else {
        minutes = Convert.ToString(numberOfMinutes);
      }

      if (numberOfSeconds < 10) {
        seconds = "0" + numberOfSeconds;
      } else {
        seconds = Convert.ToString(numberOfSeconds);
      }

      detailsTemplate.Title = new TextLine(LevelName, Font, new Color(192, 192, 192));

      startMenuEntry = new MenuButton("Start", Font);
      startMenuEntry.Selected += StartMenuEntrySelected;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Right] = startMenuEntry;
      detailsTemplate.SelectedItem = DetailsTemplate.Selection.Right;

      exitMenuEntry = new MenuButton("Back", Font);
      exitMenuEntry.Selected += OnCancel;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Left] = exitMenuEntry;


      IList<IMenuLine> stats = detailsTemplate.Lines;
      stats.Clear();

      stats.Add(new TextLine("Status: " + (Completed ? "Completed" : "Incomplete"), Font, Color.White));
      stats.Add(new Spacer(Font.LineSpacing));
      stats.Add(new TextLine("Completion Time: " + minutes + ":" + seconds, Font, Color.White));
      stats.Add(new Spacer(Font.LineSpacing));
      stats.Add(new TextLine("Score: " + LevelScore, Font, Color.White));
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
    /// Draw the Stats of the current selected level to the Screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      detailsTemplate.Draw(spriteBatch, gameTime);
    }


    /// <summary>
    /// Event handler for when the Start menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void StartMenuEntrySelected() {
      LoadingScreen.Load(TopLevel, true, new GameEditorScreen(TopLevel, LevelFileName));
    }

    protected void OnCancel() {
      ExitScreen();
    }
  }
}
