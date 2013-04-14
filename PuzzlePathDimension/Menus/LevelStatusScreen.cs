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

    MenuButton startMenuEntry;
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
    public int LevelNumber { get; private set; }

    /// <summary>
    /// Return the time spent on completing a level with the highest score.
    /// </summary>
    public string CompletionTime { get; private set; }

    private SpriteFont Font { get; set; }


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="completed"></param>
    /// <param name="levelScore"></param>
    /// <param name="levelNumber"></param>
    /// <param name="completionTime"></param>
    public LevelStatusScreen(bool completed, int levelScore, int levelNumber, string completionTime) {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);

      Completed = completed;
      LevelScore = levelScore;
      LevelNumber = levelNumber;
      CompletionTime = completionTime;
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      Font = shared.Load<SpriteFont>("menufont");

      detailsTemplate.Title = new TextLine("Level " + LevelNumber, Font, new Color(192, 192, 192));
      detailsTemplate.Cancelled += OnCancel;

      startMenuEntry = new MenuButton("Start", Font);
      startMenuEntry.Selected += StartMenuEntrySelected;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Right] = startMenuEntry;

      exitMenuEntry = new MenuButton("Back", Font);
      exitMenuEntry.Selected += OnCancel;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Left] = exitMenuEntry;


      IList<IMenuLine> stats = detailsTemplate.Lines;
      stats.Clear();

      stats.Add(new TextLine("Status: " + (Completed ? "Completed" : "Incomplete"), Font, Color.White, 1.25f));
      stats.Add(new Spacer(Font.LineSpacing));
      stats.Add(new TextLine("Completion Time: " + CompletionTime, Font, Color.White, 1.25f));
      stats.Add(new Spacer(Font.LineSpacing));
      stats.Add(new TextLine("Score: " + LevelScore, Font, Color.White, 1.25f));
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
      detailsTemplate.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      detailsTemplate.Draw(spriteBatch, gameTime);
    }


    /// <summary>
    /// Event handler for when the Start menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void StartMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      LoadingScreen.Load(ScreenList, true, e.PlayerIndex,
                         new GameplayScreen());
    }

    protected void OnCancel(object sender, PlayerIndexEventArgs e) {
      ExitScreen();
    }
  }
}
