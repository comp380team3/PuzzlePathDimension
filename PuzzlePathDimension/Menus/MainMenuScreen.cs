//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  /// <summary>
  /// The main menu screen is the first thing displayed when the game starts up.
  /// </summary>
  class MainMenuScreen : MenuScreen {
    MenuTemplate menuTemplate = new MenuTemplate();

    public MainMenuScreen() : base("") {
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("menufont");

      menuTemplate.Title = new TextLine("Puzzle Path", font, new Color(192, 192, 192));
      menuTemplate.Cancelled += OnCancel;


      IList<MenuButton> items = menuTemplate.Items;

      MenuButton playGameMenuEntry = new MenuButton("Play Game", font);
      playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
      items.Add(playGameMenuEntry);

      MenuButton optionsMenuEntry = new MenuButton("Options", font);
      optionsMenuEntry.Selected += OptionsMenuEntrySelected;
      items.Add(optionsMenuEntry);

      MenuButton howToPlayMenuEntry = new MenuButton("How To Play", font);
      howToPlayMenuEntry.Selected += howToPlayMenuEntrySelected;
      items.Add(howToPlayMenuEntry);

      MenuButton creditsMenuEntry = new MenuButton("Credits", font);
      creditsMenuEntry.Selected += CreditsMenuEntrySelected;
      items.Add(creditsMenuEntry);

      MenuButton exitMenuEntry = new MenuButton("Exit", font);
      exitMenuEntry.Selected += OnCancel;
      items.Add(exitMenuEntry);
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

      menuTemplate.Update(this, true, gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      menuTemplate.Draw(this, spriteBatch, true, gameTime);
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
