using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The first set of three How To Play Screens. Describes
  /// the basic concepts of the Puzzle Path game and gives a
  /// brief description of the basic actions available in the game.
  /// </summary>
  class HowToPlayScreen1 : MenuScreen {
    MenuButton nextMenuEntry = new MenuButton("Next");
    MenuButton exitMenuEntry = new MenuButton("Exit");

    public HowToPlayScreen1()
        : base("How To Play") {
      nextMenuEntry.Selected += NextMenuEntrySelected;
      MenuEntries.Add(nextMenuEntry);

      exitMenuEntry.Selected += OnCancel;
      MenuEntries.Add(exitMenuEntry);
    }


    /// <summary>
    /// Update the MenuEntry's location.
    /// </summary>
    protected override void UpdateMenuEntryLocations() {
      base.UpdateMenuEntryLocations();

      // TODO: Use virtual coordinate system instead of physical screen viewport.
      Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

      // start at Y = 550; start at the lower end of the screen
      Vector2 position = new Vector2(viewport.Width / 6, 550);
      exitMenuEntry.Position = position;

      position.X += 450;
      nextMenuEntry.Position = position;
    }


    /// <summary>
    /// Event handler for when the Next menu entry is selected
    /// </summary>
    void NextMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ExitScreen();
      ScreenList.AddScreen(new HowToPlayScreen2(), e.PlayerIndex);
    }

    /// <summary>
    /// Handle the input of the user. If the user wants to move
    /// to a diffenrent menu entry, they can press left or right.
    /// </summary>
    /// <param name="vtroller"></param>
    public override void HandleInput(VirtualController vtroller) {
      if (vtroller.CheckForRecentRelease(VirtualButtons.Left)) {
        SelectedEntry -= 1;

        if (SelectedEntry < 0)
          SelectedEntry = MenuEntries.Count - 1;
      }

      // Move to the next menu entry?
      if (vtroller.CheckForRecentRelease(VirtualButtons.Right)) {
        SelectedEntry += 1;

        if (SelectedEntry >= MenuEntries.Count)
          SelectedEntry = 0;
      }

      // Accept or cancel the menu? We pass in our ControllingPlayer, which may
      // either be null (to accept input from any player) or a specific index.
      // If we pass a null controlling player, the InputState helper returns to
      // us which player actually provided the input. We pass that through to
      // OnSelectEntry and OnCancel, so they can tell which player triggered them.

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        OnSelectEntry(SelectedEntry, PlayerIndex.One);
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        OnCancel(PlayerIndex.One);
      }
    }
  }
}
