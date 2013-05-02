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
    public MainMenuScreen(TopLevelModel topLevel)
      : base(topLevel) {
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

    protected override void OnButtonReleased(VirtualButtons button) {
      switch (button) {
      case VirtualButtons.Up:
        menuTemplate.SelectPrev();
        break;
      case VirtualButtons.Down:
        menuTemplate.SelectNext();
        break;
      case VirtualButtons.Select:
        menuTemplate.Confirm();
        break;
      case VirtualButtons.Delete:
        OnCancel();
        break;
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
    void PlayGameMenuEntrySelected() {
      ScreenList.AddScreen(new LevelSelectScreen(TopLevel, content));
    }

    /// <summary>
    /// Event handler for when the How To Play menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void howToPlayMenuEntrySelected() {
      ScreenList.AddScreen(new HowToPlayScreenContentList(TopLevel));
    }

    /// <summary>
    /// Event handler for when the Options menu entry is selected.
    /// </summary>
    void OptionsMenuEntrySelected() {
      ScreenList.AddScreen(new OptionsMenuScreen(TopLevel));
    }

    /// <summary>
    /// Event handler for when the Credits meny entry is selected.
    /// </summary>
    void CreditsMenuEntrySelected() {
      ScreenList.AddScreen(new CreditsMenuScreen(TopLevel));
    }

    /// <summary>
    /// When the user cancels the main menu, ask if they want to exit the sample.
    /// </summary>
    void OnCancel() {
      const string message = "Are you sure you want to exit the game?";

      MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(TopLevel, message);
      confirmExitMessageBox.RightButton += ConfirmExitMessageBoxAccepted;
      ScreenList.AddScreen(confirmExitMessageBox);
    }

    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to exit" message box.
    /// </summary>
    void ConfirmExitMessageBoxAccepted() {
      Game.Exit();
    }
  }
}
