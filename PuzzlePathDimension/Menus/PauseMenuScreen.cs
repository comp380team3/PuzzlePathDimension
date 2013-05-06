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

    /// <summary>
    /// Determines if level is restarble.
    /// </summary>
    IRestartable restartable;

    /// <summary>
    /// Content manager for the screen.
    /// </summary>
    ContentManager content;
    
    /// <summary>
    /// Contains information on whether the user is in editor mode or simulation mode. 
    /// </summary>
    bool EditorMode { get; set; }
    bool custom;
    /// <summary>
    /// The name of the current level.
    /// </summary>
    string LevelName { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="topLevel"></param>
    /// <param name="simulation"></param>
    /// <param name="levelName"></param>
    public PauseMenuScreen(TopLevelModel topLevel, Simulation simulation, string levelName)
      : base(topLevel) {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
      EditorMode = false;
      this.restartable = simulation;
      LevelName = levelName;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="topLevel"></param>
    /// <param name="editableLevel"></param>
    /// <param name="levelName"></param>
    public PauseMenuScreen(TopLevelModel topLevel, EditableLevel editableLevel, string levelName)
      : base(topLevel) {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
      EditorMode = true;
      custom = editableLevel.Custom;
      this.restartable = editableLevel;
      LevelName = levelName;
    }

    /// <summary>
    /// Load the content that will be displayed on the screen.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("Font/menufont");
      content = shared;

      menuTemplate.Title = new TextLine("Paused", font, new Color(192, 192, 192));


      IList<MenuButton> items = menuTemplate.Items;

      // Create new menu buttons and attach to them events.
      // Add menu buttons the list that will be dislayed on the screen.
      MenuButton resumeGameMenuEntry = new MenuButton("Resume Game", font);
      resumeGameMenuEntry.Selected += OnCancel;
      items.Add(resumeGameMenuEntry);

      // Only allow the player to restart the level in simulation mode.
      MenuButton restartLevelMenuEntry = new MenuButton("Restart Level", font);
      restartLevelMenuEntry.Selected += RestartLevelMenuEntrySelected;
      if (!EditorMode) {
        items.Add(restartLevelMenuEntry);
      }

      MenuButton restartLevelEditor;
      if (EditorMode) {
        if (custom)
          restartLevelEditor = new MenuButton("Empty Level", font);
        else
          restartLevelEditor = new MenuButton("Restart Level Editor", font);
      } else {
        restartLevelEditor = new MenuButton("Back to Level Editor", font);
      }
      restartLevelEditor.Selected += RestartLevelEditorMenuEntrySelected;
      items.Add(restartLevelEditor);

      MenuButton howToPlayMenuEntry = new MenuButton("How to Play", font);
      howToPlayMenuEntry.Selected += HowToPlayMenuEntrySelected;
      items.Add(howToPlayMenuEntry);

      MenuButton levelSelectMenuEntry = new MenuButton("Back to Level Select", font);
      levelSelectMenuEntry.Selected += LevelSelectMenuEntrySelected;
      items.Add(levelSelectMenuEntry);

      MenuButton quitGameMenuEntry = new MenuButton("Back to Main Menu", font);
      quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
      items.Add(quitGameMenuEntry);
    }

    /// <summary>
    /// Remove the content from the screen.
    /// </summary>
    public override void UnloadContent() {
      base.UnloadContent();
    }

    /// <summary>
    /// Handle user input.
    /// </summary>
    /// <param name="button"></param>
    protected override void OnButtonReleased(VirtualButtons button) {
      switch (button) {
      case VirtualButtons.Up:
        menuTemplate.SelectPrev();
        break;
      case VirtualButtons.Down:
        menuTemplate.SelectNext();
        break;
      case VirtualButtons.Select:
      case VirtualButtons.Context:
        menuTemplate.Confirm();
        break;
      case VirtualButtons.Delete:
        OnCancel();
        break;
      }
    }

    /// <summary>
    /// Handle mouse input.
    /// </summary>
    /// <param name="point"></param>
    protected override void OnPointChanged(Point point) {
      menuTemplate.SelectAtPoint(point);
    }

    /// <summary>
    /// Update the current state of the game.
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
    /// Draw the content to the screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      menuTemplate.Draw(spriteBatch, gameTime);
    }

    /// <summary>
    /// Event handler for when the Exit menu entry is selected.
    /// </summary>
    void OnCancel() {
      ExitScreen();
    }

    /// <summary>
    /// Event handler for when the Restart level editor menu entry is selected.
    /// </summary>
    void RestartLevelEditorMenuEntrySelected() {
      string message = "Are you sure you want to restart the editor?";
      if (!EditorMode) {
        message = "Are you sure you want to go to the editor?";
      }

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

    /// <summary>
    /// Event handler for when the Controls menu entry is selected.
    /// </summary>
    void HowToPlayMenuEntrySelected() {
      ScreenList.AddScreen(new HowToPlayScreenContentList(TopLevel));
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
    /// Event handler for when the user selects confirm on the Restart level message box.
    /// </summary>
    void ConfirmRestartLevelBoxAccepted() {
      restartable.Restart();
      ExitScreen();
    }

    /// <summary>
    /// Event handler for when the user selects confirm on the Restart editor Message Box.
    /// </summary>
    void ConfirmRestartLevelEditorBoxAccepted() {
      EditableLevel level = (EditableLevel)restartable;
      if (level.Custom) {
        level.Restart();
        ExitScreen();
      } else
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
