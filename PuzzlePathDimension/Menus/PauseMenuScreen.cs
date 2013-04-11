//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  /// <summary>
  /// The pause menu comes up over the top of the game,
  /// giving the player options to resume or quit.
  /// </summary>
  class PauseMenuScreen : MenuScreen {
    /// <summary>
    /// Constructor.
    /// </summary>
    public PauseMenuScreen()
        : base("Paused") {
      MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
      resumeGameMenuEntry.Selected += OnCancel;
      MenuEntries.Add(resumeGameMenuEntry);

      MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");
      quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
      MenuEntries.Add(quitGameMenuEntry);
    }


    /// <summary>
    /// Event handler for when the Quit Game menu entry is selected.
    /// </summary>
    void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      const string message = "Are you sure you want to quit this game?";

      MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
      confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
      ScreenList.AddScreen(confirmQuitMessageBox, ControllingPlayer);
    }

    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to quit" message box. This uses the loading screen to
    /// transition from the game back to the main menu screen.
    /// </summary>
    void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e) {
      LoadingScreen.Load(ScreenList, false, null, new BackgroundScreen(),
                                                     new MainMenuScreen());
    }
  }
}
