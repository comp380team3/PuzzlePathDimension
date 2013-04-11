//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  /// <summary>
  /// The main menu screen is the first thing displayed when the game starts up.
  /// </summary>
  class MainMenuScreen : MenuScreen {
    /// <summary>
    /// Constructor fills in the menu contents.
    /// </summary>
    public MainMenuScreen()
        : base("Puzzle Path") {
      MenuButton playGameMenuEntry = new MenuButton("Play Game");
      playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
      MenuEntries.Add(playGameMenuEntry);

      MenuButton optionsMenuEntry = new MenuButton("Options");
      optionsMenuEntry.Selected += OptionsMenuEntrySelected;
      MenuEntries.Add(optionsMenuEntry);

      MenuButton howToPlayMenuEntry = new MenuButton("How To Play");
      howToPlayMenuEntry.Selected += howToPlayMenuEntrySelected;
      MenuEntries.Add(howToPlayMenuEntry);

      MenuButton creditsMenuEntry = new MenuButton("Credits");
      creditsMenuEntry.Selected += CreditsMenuEntrySelected;
      MenuEntries.Add(creditsMenuEntry);

      MenuButton exitMenuEntry = new MenuButton("Exit");
      exitMenuEntry.Selected += OnCancel;
      MenuEntries.Add(exitMenuEntry);
    }


    /// <summary>
    /// Event handler for when the Play Game menu entry is selected.
    /// </summary>
    void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ScreenList.AddScreen(new LevelSelectScreen(), e.PlayerIndex);
    }

    /// <summary>
    /// Event handler for when the Options menu entry is selected.
    /// </summary>
    void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ScreenList.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
    }

    /// <summary>
    /// Event handler for when the How To Play menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void howToPlayMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ScreenList.AddScreen(new HowToPlayScreen1(), e.PlayerIndex);
    }

    /// <summary>
    /// Event handler for when the Credits meny entry is selected.
    /// </summary>
    void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ScreenList.AddScreen(new CreditsMenuScreen(), e.PlayerIndex);
    }

    /// <summary>
    /// When the user cancels the main menu, ask if they want to exit the sample.
    /// </summary>
    protected override void OnCancel(PlayerIndex playerIndex) {
      const string message = "Are you sure you want to exit this sample?";

      MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
      confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
      ScreenList.AddScreen(confirmExitMessageBox, playerIndex);
    }


    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to exit" message box.
    /// </summary>
    void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e) {
      ScreenManager.Game.Exit();
    }
  }
}
