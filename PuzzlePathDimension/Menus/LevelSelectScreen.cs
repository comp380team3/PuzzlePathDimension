using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class LevelSelectScreen : GameScreen {

    /// <summary>
    /// Structure that contains information for a level.
    /// </summary>
    public struct LevelInfo {

      /// <summary>
      /// The name for the current level.
      /// </summary>
      public string LevelName { get; set; }

      /// <summary>
      /// The file name for the current level.
      /// </summary>
      public string FileName { get; set; }

      /// <summary>
      /// The score for the current level.
      /// </summary>
      public int LevelScore { get; set; }

      /// <summary>
      /// The level is complete if true, otherwise the level is incomplete.
      /// </summary>
      public bool Completed { get; set; }

      /// <summary>
      /// The time that the user spent to complete the current level.
      /// </summary>
      public int CompletionTime { get; set; }
    }

    /// <summary>
    /// Menu template
    /// </summary>
    MenuTemplate menuTemplate = new MenuTemplate();

    /// <summary>
    /// List of level menu entries and menu buttons.
    /// </summary>
    IList<MenuButton> items;

    /// <summary>
    /// Menu Button for the Level menu entry.
    /// </summary>
    MenuButton aLevelMenuEntry;

    /// <summary>
    /// Menu Button for the Exit menu entry.
    /// </summary>
    MenuButton exitMenuEntry;

    /// <summary>
    /// Menu Button for the Next menu entry.
    /// </summary>
    MenuButton nextMenuEntry;

    /// <summary>
    /// Menu Button for the Back menu entry.
    /// </summary>
    MenuButton backMenuEntry;

    /// <summary>
    /// List of LevelInfo structures.
    /// </summary>
    List<LevelInfo> levelSet;

    /// <summary>
    /// Contains the information of the current level.
    /// </summary>
    LevelInfo levelInfo;

    /// <summary>
    /// Number that has the location of the current level in a list of levels.
    /// </summary>
    public int CurrentLevel { get; set; }

    /// <summary>
    /// List of the levels
    /// </summary>
    public string[] Levels { get; private set; }

    /// <summary>
    /// Maximum number of levels that are displayed to the screen. 
    /// </summary>
    int numberOfLevelsPerPage = 7;

    /// <summary>
    /// The set of levels that have been shown on the screen.
    /// </summary>
    int setOfLevels = 0;

    /// <summary>
    /// Keeps track of the number of times the user selects the next menu entry.
    /// </summary>
    int numberOfTimesNextSelected = 0;

    ContentManager content;

    SpriteFont font;

    #region Initialization

    /// <summary>
    /// Contructor
    /// Read an xml file and obtain information for each level in the xml file.
    /// </summary>
    public LevelSelectScreen(TopLevelModel topLevel, ContentManager Content)
      : base(topLevel) {

      // Create a list of all the levels and obtain the user progress for all existing levels.
      levelSet = new List<LevelInfo>();

      string levelListFile = Configuration.UserDataPath + Path.DirectorySeparatorChar + "levellist.xml";

      LevelGroup levels = LevelGroup.Load(levelListFile);
      SerializableDictionary<string, LevelStatus> progressInfo = base.Profile.Progress;

      // Go through the user progress data and display the correct information for each level.
      foreach (LevelEntry entry in levels.Entries) {
        LevelInfo info = new LevelInfo();

        info.FileName = entry.FullPath;
        info.LevelName = entry.Id;

        if (progressInfo.ContainsKey(info.LevelName)) {
          info.CompletionTime = progressInfo[info.LevelName].FastestTimeInSeconds;
          info.Completed = progressInfo[info.LevelName].Completed;
          info.LevelScore = progressInfo[info.LevelName].Score;
        } else {
          info.CompletionTime = 3600;
          info.Completed = false;
          info.LevelScore = 0;
        }
        levelSet.Add(info);
      }

      // Add the custom level's entry at the end of the list.
      AddCustomLevel();

      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
      
      items = menuTemplate.Items;
    }

    /// <summary>
    /// Adds the hard-coded entry for the custom level.
    /// </summary>
    private void AddCustomLevel() {
      LevelInfo customInfo = new LevelInfo();
      SerializableDictionary<string, LevelStatus> progressInfo = base.Profile.Progress;

      customInfo.FileName = Configuration.UserDataPath + Path.DirectorySeparatorChar
        + "Level" + Path.DirectorySeparatorChar + "Custom.xml";
      customInfo.LevelName = "Custom Level";

      if (progressInfo.ContainsKey(customInfo.LevelName)) {
        customInfo.CompletionTime = progressInfo[customInfo.LevelName].FastestTimeInSeconds;
        customInfo.Completed = progressInfo[customInfo.LevelName].Completed;
        customInfo.LevelScore = progressInfo[customInfo.LevelName].Score;
      } else {
        customInfo.CompletionTime = 3600;
        customInfo.Completed = false;
        customInfo.LevelScore = 0;
      }
      levelSet.Add(customInfo);
    }

    /// <summary>
    /// Load content that will be used to create the level select screen.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      font = shared.Load<SpriteFont>("Font/menufont");
      content = shared;

      menuTemplate.Title = new TextLine("Select A Level", font, new Color(192, 192, 192));

      exitMenuEntry = new MenuButton("Exit", font);
      exitMenuEntry.Selected += OnCancel;
      
      nextMenuEntry = new MenuButton("Next", font);
      nextMenuEntry.Selected += NextMenuEntrySelected;

      backMenuEntry = new MenuButton("Back", font);
      backMenuEntry.Selected += BackMenuEntrySelected;

      UpdateCurrentPage();
    }

    private void UpdateCurrentPage() {
      items.Clear();
      int previousLevel = 0;

      for (int count = 0; CurrentLevel < levelSet.Count && count < numberOfLevelsPerPage; count++) {
        levelInfo = levelSet.ElementAt<LevelInfo>(CurrentLevel);
        aLevelMenuEntry = new MenuButton(levelInfo.LevelName, font);
        aLevelMenuEntry.OriginalColor = Color.Gray;
        previousLevel = CurrentLevel - 1;

        if (previousLevel >= 0) {
          if (levelSet[previousLevel].Completed || levelInfo.LevelName == "Custom Level") { // Meh. -Jorenz
            aLevelMenuEntry.Selected += () => ALevelMenuEntrySelected(menuTemplate.SelectedItem);
            aLevelMenuEntry.OriginalColor = Color.White;
          }
        } else {
          previousLevel = 0;
          aLevelMenuEntry.Selected += () => ALevelMenuEntrySelected(menuTemplate.SelectedItem);
          aLevelMenuEntry.OriginalColor = Color.White;
        }

        items.Add(aLevelMenuEntry);
        CurrentLevel = CurrentLevel + 1;
        setOfLevels = count + 1;
      }

      if ((levelSet.Count - CurrentLevel) > 0) {
        items.Add(nextMenuEntry);
      }

      if ((CurrentLevel - numberOfLevelsPerPage) > 0) {
        items.Add(backMenuEntry);
      }

      items.Add(exitMenuEntry);
    }

    /// <summary>
    /// Handle User Input
    /// </summary>
    /// <param name="button"></param>
    protected override void OnButtonReleased(VirtualButtons button) {
      switch (button) {
      case VirtualButtons.Up:
        menuTemplate.SelectPrev();
        break;
      case VirtualButtons.Down:
        menuTemplate.SelectNext();
        break;
      case VirtualButtons.Select:
      case VirtualButtons.Context:
        menuTemplate.Confirm();
        break;
      case VirtualButtons.Delete:
        OnCancel();
        break;
      }
    }

    /// <summary>
    /// Handle User input from the mouse.
    /// </summary>
    /// <param name="point"></param>
    protected override void OnPointChanged(Point point) {
      menuTemplate.SelectAtPoint(point);
    }

    /// <summary>
    /// Update the Screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="otherScreenHasFocus"></param>
    /// <param name="coveredByOtherScreen"></param>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      menuTemplate.TransitionPosition = TransitionPosition;

      menuTemplate.Update(gameTime);
    }

    /// <summary>
    /// Draw all the Level menu entries to the screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);
      menuTemplate.Draw(spriteBatch, gameTime);
    }

    #endregion

    #region Handle Input

    /// <summary>
    /// Event handler for when the Exit menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnCancel() {
      ExitScreen();
    }

    /// <summary>
    /// Event handler for when the Level menu entry is selected.
    /// </summary>
    void ALevelMenuEntrySelected(int selected) {
      if (CurrentLevel > numberOfLevelsPerPage * (numberOfTimesNextSelected)) {
        selected = selected + (numberOfLevelsPerPage * (numberOfTimesNextSelected));
      } else {
        selected = selected + CurrentLevel - setOfLevels;
      }
      LevelInfo level = levelSet.ElementAt<LevelInfo>(selected);
      ScreenList.AddScreen(new LevelStatusScreen(TopLevel, level));
    }

    void NextMenuEntrySelected() {
      turnPage(false);
      numberOfTimesNextSelected++;
    }

    void BackMenuEntrySelected() {
      turnPage(true);
    }

    #endregion

    /// <summary>
    /// Display the next set of levels if the Next menu entry is selected. Display
    /// the previous levels if the Back menu entry is selected.
    /// </summary>
    /// <param name="back"></param>
    private void turnPage(bool back) {
      if (back) {
        CurrentLevel = CurrentLevel - numberOfLevelsPerPage - setOfLevels;
      }

      UpdateCurrentPage();
      menuTemplate.SelectedItem = 0;
    }
  }
}
