using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace PuzzlePathDimension {
  class CompletionScreen : GameScreen{

    /// <summary>
    /// Contains information on displaying the Completion Screen.
    /// </summary>
    DetailsTemplate detailsTemplate;

    /// <summary>
    /// Level Data that will be used to determine the players score.
    /// </summary>
    LevelScoreData levelData;

    /// <summary>
    /// Set of Levels in the game.
    /// </summary>
    LevelGroup levelSet;

    /// <summary>
    /// Menu Button for the next button menu entry.
    /// </summary>
    MenuButton nextLevelMenuEntry;

    /// <summary>
    /// Menu Button for the level select menu entry.
    /// </summary>
    MenuButton levelSelectMenuEntry;

    /// <summary>
    /// Menu Button for the retry menu entry.
    /// </summary>
    MenuButton retryMenuEntry;

    /// <summary>
    /// Allows content to be loaded.
    /// </summary>
    ContentManager content;

    /// <summary>
    /// Simulates the game.
    /// </summary>
    Simulation simulation;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="topLevel"></param>
    /// <param name="completedLevel"></param>
    /// <param name="simulation"></param>
    public CompletionScreen(TopLevelModel topLevel, LevelScoreData completedLevel, Simulation simulation)
    : base(topLevel) {

      levelData = completedLevel;
      this.simulation = simulation;

      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    /// <summary>
    /// Load the content that will be used to display the screen. Calculate
    /// the players score and display all the data on the screen.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);

      content = shared;
      SpriteFont font = shared.Load<SpriteFont>("Font/menufont");

      // Calculate the players score based on the data of the 
      // players actions in the game.
      int treasureScore = 500 * levelData.TreasuresCollected;
      int ballsLeftScore = 150 * levelData.BallsLeft;
      string fullPath = Configuration.UserDataPath + Path.DirectorySeparatorChar + "levellist.xml";
      int timeSpentInSeconds = levelData.TimeSpent % 60;
      int timeSpentInMinutes = levelData.TimeSpent / 60;
      int parTimeInSeconds = levelData.ParTime % 60;
      int parTimeInMinutes = levelData.ParTime / 60;
      string timeSpentSeconds = Convert.ToString(timeSpentInSeconds);
      string parTimeSeconds = Convert.ToString(parTimeInSeconds);

      // Add a zero in front of the number of seconds if the
      // seconds is less than 10. This is for the formatting of the time.
      if (timeSpentInSeconds < 10) {
        timeSpentSeconds = "0" + timeSpentInSeconds;
      }

      if (parTimeInSeconds < 10) {
        parTimeSeconds = "0" + parTimeInSeconds;
      }

      detailsTemplate = new DetailsTemplate();

      // Load the list of levels
      levelSet = LevelGroup.Load(fullPath);


      detailsTemplate.Title = new TextLine("Congratulations, Level Complete.", font, Color.White);

      IList<IMenuLine> description = detailsTemplate.Lines;

      // Add the description of how the user's score was generated.
      description.Add(new TextLine("Treasures obtained: " + levelData.TreasuresCollected + "/" + levelData.TreasuresInLevel +
      " (+" + treasureScore + ")", font, Color.White));
      description.Add(new TextLine("Balls remaining: " + levelData.BallsLeft + " (+" + ballsLeftScore + ")", font, Color.White));
      description.Add(new TextLine("Time spent: " + timeSpentInMinutes + ":" + timeSpentSeconds, font, Color.White));
      description.Add(new TextLine("Par time: " + parTimeInMinutes + ":" + parTimeSeconds, font, Color.White));
      if (levelData.TimeSpent <= levelData.ParTime) {
        description.Add(new TextLine("Par time met! (+100)", font, Color.White));
      }

      description.Add(new TextLine("Your score is: " + levelData.Score, font, Color.White));

      // Create the Buttons and attach the events.
      retryMenuEntry = new MenuButton("Retry Level", font);
      retryMenuEntry.Selected += RetryMenuEntrySelected;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Left] = retryMenuEntry;

      levelSelectMenuEntry = new MenuButton("Level Select", font);
      levelSelectMenuEntry.Selected += LevelSelectMenuEntrySelected;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Middle] = levelSelectMenuEntry;
      detailsTemplate.SelectedItem = DetailsTemplate.Selection.Middle;

      // Check if there is a next level in the list. If there exist one
      // display the next Level Button, otherwise don't.
      if (levelSet.FindNextLevel(levelData.LevelName) != null) {
        nextLevelMenuEntry = new MenuButton("Next Level", font);
        nextLevelMenuEntry.Selected += NextLevelMenuEntrySelected;
        detailsTemplate.Buttons[DetailsTemplate.Selection.Right] = nextLevelMenuEntry;
        detailsTemplate.SelectedItem = DetailsTemplate.Selection.Right;
      }

      SaveUserProgress();
    }

    /// <summary>
    /// Handle user input from the keyboard or xbox controller.
    /// </summary>
    /// <param name="button"></param>
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
      }
    }

    /// <summary>
    /// Handle mouse pointer.
    /// </summary>
    /// <param name="point"></param>
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
    /// Event handler for when the user selects the Next level menu entry.
    /// </summary>
    void NextLevelMenuEntrySelected() {
      LevelEntry nextLevel = levelSet.FindNextLevel(levelData.LevelName);
      Console.WriteLine(nextLevel.FullPath);

      LoadingScreen.Load(TopLevel, true, new GameEditorScreen(TopLevel, nextLevel.FullPath));
    }

    /// <summary>
    /// Event handler for when the user selects ok on the level select
    /// button on the message box. This uses the loading screen to
    /// transition from the game back to the level select screen.
    /// </summary>
    void LevelSelectMenuEntrySelected() {
      LoadingScreen.Load(TopLevel, false, null, new BackgroundScreen(TopLevel), new MainMenuScreen(TopLevel), new LevelSelectScreen(TopLevel, content));
    }

    /// <summary>
    /// Event handler for when the user selects the Retry menu entry.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void RetryMenuEntrySelected() {
      simulation.Restart();
      ExitScreen();
    }

    /// <summary>
    /// Saves the user progress.
    /// </summary>
    private void SaveUserProgress() {
      string levelName = levelData.LevelName;
      UserProfile profile = base.Profile;
      SerializableDictionary<string, LevelStatus> progressInfo = base.Profile.Progress;

      LevelStatus status;

      // If the user has played the level before, get the info from the dictionary,
      // and replace the score if it's better.
      if (progressInfo.ContainsKey(levelName)) {
        status = progressInfo[levelName];

        status.Completed = true;

        if (status.Score < levelData.Score) {
          status.Score = levelData.Score;
          status.FastestTimeInSeconds = levelData.TimeSpent;
        } else if (status.Score == levelData.Score) {
          if (status.FastestTimeInSeconds < status.FastestTimeInSeconds) {
            status.FastestTimeInSeconds = levelData.TimeSpent;
          }
        }

      } else {
        // Otherwise, create a new LevelStatus object and insert it into
        // the dictionary.
        status = new LevelStatus();
        status.Completed = true;
        status.FastestTimeInSeconds = levelData.TimeSpent;
        status.Score = levelData.Score;

        progressInfo[levelName] = status;
      }

      // In either case, save the user's profile.
      profile.Save(Configuration.UserDataPath + Path.DirectorySeparatorChar + "profile.xml");
    }
  }
}
