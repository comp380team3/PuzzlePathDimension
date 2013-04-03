using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  class HowToPlayScreen2 : MenuScreen {
    MenuEntry nextMenuEntry;
    MenuEntry exitMenuEntry;

    #region Initialize

    public HowToPlayScreen2()
      : base("How To Play") {
      // Create a menu entry to transition to the next screen
      nextMenuEntry = new MenuEntry("Next");
      exitMenuEntry = new MenuEntry("Back");

      // Hook up menu event handlers.
      nextMenuEntry.Selected += NextMenuEntrySelected;
      exitMenuEntry.Selected += OnCancel;

      // Add the menu entry to the menu
      MenuEntries.Add(nextMenuEntry);
      MenuEntries.Add(exitMenuEntry);
    }

    #endregion

    #region Handle Input

    /// <summary>
    /// Event handler for when the Next menu entry is selected
    /// </summary>
    void NextMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ScreenManager.AddScreen(new HowToPlayScreen3(), e.PlayerIndex);
    }

    #endregion
  }
}
