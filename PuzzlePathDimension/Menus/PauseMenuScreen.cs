//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The pause menu comes up over the top of the game,
  /// giving the player options to resume or quit.
  /// </summary>
  class PauseMenuScreen : GameScreen {
    MenuTemplate menuTemplate = new MenuTemplate();

    IRestartable restartable;
    ContentManager content;
    bool EditorMode { get; set; }
    string LevelName { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PauseMenuScreen(TopLevelModel topLevel, Simulation simulation, string levelName)
      : base(topLevel) {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
      EditorMode = false;
      this.restartable = simulation;
      LevelName = levelName;
    }

    public PauseMenuScreen(TopLevelModel topLevel, EditableLevel editableLevel, string levelName)
      : base(topLevel) {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
      EditorMode = true;
      this.restartable = editableLevel;
      LevelName = levelName;
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("Font/menufont");
      content = shared;

      menuTemplate.Title = new TextLine("Paused", font, new Color(192, 192, 192));


      IList<MenuButton> items = menuTemplate.Items;

      MenuButton resumeGameMenuEntry = new MenuButton("Resume Game", font);
      resumeGameMenuEntry.Selected += OnCancel;
      items.Add(resumeGameMenuEntry);

      MenuButton restartLevelEditor = new MenuButton("Restart Level Editor", font);
      restartLevelEditor.Selected += RestartLevelEditorMenuEntrySelected;
      items.Add(restartLevelEditor);

      MenuButton restartLevelMenuEntry = new MenuButton("Restart Level", font);
      restartLevelMenuEntry.Selected += RestartLevelMenuEntrySelected;
      if (!EditorMode) {
        items.Add(restartLevelMenuEntry);
      }

      MenuButton levelSelectMenuEntry = new MenuButton("Level Select", font);
      levelSelectMenuEntry.Selected += LevelSelectMenuEntrySelected;
      items.Add(levelSelectMenuEntry);

      MenuButton controlsMenuEntry = new MenuButton("Controls", font);
      controlsMenuEntry.Selected += ControlsMenuEntrySelected;
      items.Add(controlsMenuEntry);

      MenuButton quitGameMenuEntry = new MenuButton("Back to Main Menu", font);
      quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
      items.Add(quitGameMenuEntry);
    }

    public override void UnloadContent() {
      base.UnloadContent();
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

    protected override void OnPointChanged(Point point) {
      menuTemplate.SelectAtPoint(point);
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      menuTemplate.TransitionPosition = TransitionPosition;
      menuTemplate.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      menuTemplate.Draw(spriteBatch, gameTime);
    }

    /// <summary>
    /// Event handler when the exit menu entry is selected.
    /// </summary>
    void OnCancel() {
      ExitScreen();
    }

    void RestartLevelEditorMenuEntrySelected() {
      const string message = "Are you sure you want to restart the editor?";

      MessageBoxScreen confirmRestartLevelEditorMessageBox = new MessageBoxScreen(TopLevel, message);
      confirmRestartLevelEditorMessageBox.RightButton += ConfirmRestartLevelEditorBoxAccepted;
      ScreenList.AddScreen(confirmRestartLevelEditorMessageBox);
    }

    /// <summary>
    /// Event handler for when the Retry Game menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void RestartLevelMenuEntrySelected() {
      // Put what happens when the person clicks retry
      const string message = "Are you sure you want to restart this level?";

      MessageBoxScreen confirmRestartLevelMessageBox = new MessageBoxScreen(TopLevel, message);
      confirmRestartLevelMessageBox.RightButton += ConfirmRestartLevelBoxAccepted;
      ScreenList.AddScreen(confirmRestartLevelMessageBox);
    }

    void ControlsMenuEntrySelected() {
      ScreenList.AddScreen(new HowToPlayScreen3(TopLevel));
    }

    /// <summary>
    /// Event handler for when the Quit Game menu entry is selected.
    /// </summary>
    void QuitGameMenuEntrySelected() {
      const string message = "Are you sure you want to quit this level?";

      MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(TopLevel, message);
      confirmQuitMessageBox.RightButton += ConfirmQuitMessageBoxAccepted;
      ScreenList.AddScreen(confirmQuitMessageBox);
    }

    /// <summary>
    /// Event handler for when the Level Select menu entry is selected.
    /// </summary>
    void LevelSelectMenuEntrySelected() {
      const string message = "Are you sure you want to quit this level?";
      MessageBoxScreen confirmLevelSelectMessageBox = new MessageBoxScreen(TopLevel, message);
      confirmLevelSelectMessageBox.RightButton += ConfirmLevelSelectMessageBoxAccepted;
      ScreenList.AddScreen(confirmLevelSelectMessageBox);
    }

    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to quit" message box. This uses the loading screen to
    /// transition from the game back to the main menu screen.
    /// </summary>
    void ConfirmQuitMessageBoxAccepted() {
      LoadingScreen.Load(TopLevel, false, null, new BackgroundScreen(TopLevel), new MainMenuScreen(TopLevel));
    }

    /// <summary>
    /// Event handler for when the user selects confirm when the Retry menu entry.
    /// </summary>
    void ConfirmRestartLevelBoxAccepted() {
      restartable.Restart();
      ExitScreen();
    }

    void ConfirmRestartLevelEditorBoxAccepted() {
      LoadingScreen.Load(TopLevel, true, new GameEditorScreen(TopLevel, LevelName));
    }

    /// <summary>
    /// Event handler for when the user selects confirm for the level select menu entry.
    /// </summary>
    void ConfirmLevelSelectMessageBoxAccepted() {
      LoadingScreen.Load(TopLevel, false, null, new BackgroundScreen(TopLevel), new MainMenuScreen(TopLevel),
                                                    new LevelSelectScreen(TopLevel, content));
    }
  }
}
