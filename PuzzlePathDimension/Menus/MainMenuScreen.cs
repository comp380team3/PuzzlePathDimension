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
  class MainMenuScreen : GameScreen {
    MenuTemplate menuTemplate = new MenuTemplate();

    ContentManager content;

    /// <summary>
    /// Constructor
    /// </summary>
    public MainMenuScreen() {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }
    
    /// <summary>
    /// Load the assets required for the Main Menu.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("Font/menufont");

      content = shared;

      menuTemplate.Title = new TextLine("Puzzle Path", font, new Color(192, 192, 192));

      // List that contains the menu buttons
      IList<MenuButton> items = menuTemplate.Items;

      // Create the Menu Buttons, attach what happens for each entry,
      // and add the menu buttons to the list of buttons.
      MenuButton playGameMenuEntry = new MenuButton("Play Game", font);
      playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
      items.Add(playGameMenuEntry);

      MenuButton levelEditorMenuEntry = new MenuButton("Level Editor", font);
      levelEditorMenuEntry.Selected += LevelEditorMenuEntrySelected;
      items.Add(levelEditorMenuEntry);

      MenuButton howToPlayMenuEntry = new MenuButton("How To Play", font);
      howToPlayMenuEntry.Selected += howToPlayMenuEntrySelected;
      items.Add(howToPlayMenuEntry);

      MenuButton optionsMenuEntry = new MenuButton("Options", font);
      optionsMenuEntry.Selected += OptionsMenuEntrySelected;
      items.Add(optionsMenuEntry);

      MenuButton creditsMenuEntry = new MenuButton("Credits", font);
      creditsMenuEntry.Selected += CreditsMenuEntrySelected;
      items.Add(creditsMenuEntry);

      MenuButton exitMenuEntry = new MenuButton("Exit", font);
      exitMenuEntry.Selected += OnCancel;
      items.Add(exitMenuEntry);
    }

    /// <summary>
    /// Handle the input from the user. Down moves the selected Menu Button
    /// to the next Menu Button in the list. Up moves the selected Menu Button
    /// to the previous Menu Button in the list.
    /// </summary>
    /// <param name="vtroller"></param>
    public override void HandleInput(VirtualController vtroller) {
      base.HandleInput(vtroller);

      if (vtroller.CheckForRecentRelease(VirtualButtons.Up)) {
        menuTemplate.SelectPrev();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Down)) {
        menuTemplate.SelectNext();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        menuTemplate.Confirm();
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        OnCancel(null, new PlayerIndexEventArgs(PlayerIndex.One));
      }
    }

    /// <summary>
    /// 
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
    /// Draw the list of Menu Buttons to the Screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      menuTemplate.Draw(spriteBatch, gameTime);
    }


    /// <summary>
    /// Event handler for when the Play Game menu entry is selected.
    /// </summary>
    void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ScreenList.AddScreen(new LevelSelectScreen(content), e.PlayerIndex);
    }

    /// <summary>
    /// Event handler for when the Level Editor menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void LevelEditorMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      // Add the Level Editor Screen Here
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
    /// Event handler for when the Options menu entry is selected.
    /// </summary>
    void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ScreenList.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
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
    void OnCancel(object sender, PlayerIndexEventArgs e) {
      const string message = "Are you sure you want to exit the game?";

      MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
      confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
      ScreenList.AddScreen(confirmExitMessageBox, e.PlayerIndex);
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
