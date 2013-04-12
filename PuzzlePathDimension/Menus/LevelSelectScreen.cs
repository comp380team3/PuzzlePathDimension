using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {

  class LevelSelectScreen : MenuScreen {

    /// <summary>
    /// Menu entries for the Level Select Screen.
    /// </summary>
    MenuButton aLevelMenuEntry;
    MenuButton exitMenuEntry;

    /// <summary>
    /// The level which the user has selected.
    /// </summary>
    int levelNumber;

    /// <summary>
    /// The score for the current level.
    /// </summary>
    int levelScore;

    /// <summary>
    /// The level is complete if true, otherwise the level is incomplete.
    /// </summary>
    bool completed;

    /// <summary>
    /// The time that the user spent to complete the current level.
    /// </summary>
    string completionTime;

    #region Initialization

    /// <summary>
    /// Contructor
    /// Read an xml file and obtain information for each level in the xml file.
    /// </summary>
    public LevelSelectScreen()
      : base("Select A Level") {
      // Add the levels to the screen
      //Note: need xml file format to be completed to add level information
        levelNumber = 1;
        levelScore = 0;
        completed = false;
        completionTime = "0:00";
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);

      aLevelMenuEntry = new MenuButton(string.Empty);
      aLevelMenuEntry.Selected += ALevelMenuEntrySelected;
      MenuEntries.Add(aLevelMenuEntry);

      exitMenuEntry = new MenuButton(string.Empty);
      exitMenuEntry.Selected += OnCancel;
      MenuEntries.Add(exitMenuEntry);

      SetMenuEntryText();
    }

    /// <summary>
    /// Set the text that will be displayed for the menu entries
    /// </summary>
    void SetMenuEntryText() {
      // Set the text of each level
      aLevelMenuEntry.Text = "Level " + levelNumber;
      exitMenuEntry.Text = "Back";
    }

    #endregion

    #region Handle Input

    /// <summary>
    /// Event handler for when the Level menu entry is selected.
    /// </summary>
    void ALevelMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ScreenList.AddScreen(new LevelStatusScreen(completed, levelScore, levelNumber, completionTime), e.PlayerIndex);
    }

    #endregion
  }
}
