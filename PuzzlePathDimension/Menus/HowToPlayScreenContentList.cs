using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class HowToPlayScreenContentList : GameScreen {

    MenuTemplate menuTemplate = new MenuTemplate();

    ContentManager content;

    /// <summary>
    /// Constructor
    /// </summary>
    public HowToPlayScreenContentList() {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("Font/menufont");

      content = shared;

      menuTemplate.Title = new TextLine("How To Play", font, new Color(192, 192, 192));

      // List that contains the menu buttons
      IList<MenuButton> items = menuTemplate.Items;

      // Create the Menu Buttons, attach what happens for each entry,
      // and add the menu buttons to the list of buttons.
      MenuButton gameDescriptionMenuEntry = new MenuButton("The Game's Objective", font);
      gameDescriptionMenuEntry.Selected += GameObjectiveMenuEntrySelected;
      items.Add(gameDescriptionMenuEntry);

      MenuButton controlsDescriptionMenuEntry = new MenuButton("Controls", font);
      controlsDescriptionMenuEntry.Selected += ControlsDescriptionMenuEntrySelected;
      items.Add(controlsDescriptionMenuEntry);

      MenuButton gameObjectsDescriptionMenuEntry = new MenuButton("Game Objects", font);
      gameObjectsDescriptionMenuEntry.Selected += GameObjectsDescriptionMenuEntrySelected;
      items.Add(gameObjectsDescriptionMenuEntry);

      MenuButton gameScreenShotMenuEntry = new MenuButton("Example", font);
      gameScreenShotMenuEntry.Selected += GameScreenShotMenuEntrySelected;
      items.Add(gameScreenShotMenuEntry);

      MenuButton levelEditorDescriptionMenuEntry = new MenuButton("Editing Levels", font);
      levelEditorDescriptionMenuEntry.Selected += LevelEditorDescriptionMenuEntrySelected;
      items.Add(levelEditorDescriptionMenuEntry);

      MenuButton exitMenuEntry = new MenuButton("Back To Main Menu", font);
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
        OnCancel();
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
    /// Event handler for when the Game Objective menu entry is selected.
    /// </summary>
    void GameObjectiveMenuEntrySelected() {
      ScreenList.AddScreen(new HowToPlayScreen1());
    }

    /// <summary>
    /// Event handler for when the Game Objects description menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void GameObjectsDescriptionMenuEntrySelected() {
      ScreenList.AddScreen(new HowToPlayScreen2());
    }

    /// <summary>
    /// Event handler for when the Controls description menu entry is selected.
    /// </summary>
    void ControlsDescriptionMenuEntrySelected() {
      ScreenList.AddScreen(new HowToPlayScreen3());
    }

    /// <summary>
    /// Event handler for when the Game Screen Shot meny entry is selected.
    /// </summary>
    void GameScreenShotMenuEntrySelected() {
      ScreenList.AddScreen(new HowToPlayScreen4());
    }

    /// <summary>
    /// Event handler for when the Level Editor description meny entry is selected.
    /// </summary>
    void LevelEditorDescriptionMenuEntrySelected() {
      ScreenList.AddScreen(new HowToPlayScreen5());
    }

    /// <summary>
    /// When the user cancels the How to play screen content list.
    /// </summary>
    protected void OnCancel() {
      ExitScreen();
    }
  }
}
