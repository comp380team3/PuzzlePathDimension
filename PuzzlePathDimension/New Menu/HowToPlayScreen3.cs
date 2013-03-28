using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzlePathDimension
{
    class HowToPlayScreen3 : MenuScreen
    {
        MenuEntry exitMenuEntry;

        #region Initialize

        public HowToPlayScreen3()
            : base("How To Play")
        {
            // Create a menu entry to transition to the next screen
            exitMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            exitMenuEntry.Selected += OnCancel;

            // Add the menu entry to the menu
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion
    }
}
