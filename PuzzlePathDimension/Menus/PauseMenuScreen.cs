//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The pause menu comes up over the top of the game,
  /// giving the player options to resume or quit.
  /// </summary>
  class PauseMenuScreen : MenuScreen {
    MenuTemplate menuTemplate = new MenuTemplate();

    /// <summary>
    /// Constructor.
    /// </summary>
    public PauseMenuScreen() : base("") {
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("menufont");

      menuTemplate.Title = new TextLine("Paused", font, new Color(192, 192, 192));
      menuTemplate.Cancelled += OnCancel;


      IList<MenuButton> items = menuTemplate.Items;

      MenuButton resumeGameMenuEntry = new MenuButton("Resume Game", font);
      resumeGameMenuEntry.Selected += OnCancel;
      items.Add(resumeGameMenuEntry);

      MenuButton quitGameMenuEntry = new MenuButton("Quit Game", font);
      quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
      items.Add(quitGameMenuEntry);
    }


    public override void HandleInput(VirtualController vtroller) {
      if (vtroller.CheckForRecentRelease(VirtualButtons.Up)) {
        menuTemplate.SelectPrev();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Down)) {
        menuTemplate.SelectNext();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        menuTemplate.Confirm();
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        menuTemplate.Cancel();
      }
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      menuTemplate.TransitionPosition = TransitionPosition;
      menuTemplate.Update(true, gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      menuTemplate.Draw(this, spriteBatch, true, gameTime);
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
