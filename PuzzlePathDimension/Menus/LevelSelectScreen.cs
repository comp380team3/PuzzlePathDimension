using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class LevelSelectScreen : GameScreen {
    MenuTemplate menuTemplate = new MenuTemplate();

    /// <summary>
    /// Menu entries for the Level Select Screen.
    /// </summary>
    MenuButton aLevelMenuEntry;
    MenuButton exitMenuEntry;

    /// <summary>
    /// The level which the user has selected.
    /// </summary>
    int levelNumber;

    /// <summary>
    /// The score for the current level.
    /// </summary>
    int levelScore;

    /// <summary>
    /// The level is complete if true, otherwise the level is incomplete.
    /// </summary>
    bool completed;

    /// <summary>
    /// The time that the user spent to complete the current level.
    /// </summary>
    string completionTime;

    #region Initialization

    /// <summary>
    /// Contructor
    /// Read an xml file and obtain information for each level in the xml file.
    /// </summary>
    public LevelSelectScreen() {
      // Add the levels to the screen
      // Note: need xml file format to be completed to add level information
      levelNumber = 1;
      levelScore = 0;
      completed = false;
      completionTime = "0:00";

      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("Font/menufont");

      menuTemplate.Title = new TextLine("Select A Level", font, new Color(192, 192, 192));


      IList<MenuButton> items = menuTemplate.Items;

      aLevelMenuEntry = new MenuButton(string.Empty, font);
      aLevelMenuEntry.Selected += ALevelMenuEntrySelected;
      items.Add(aLevelMenuEntry);

      exitMenuEntry = new MenuButton(string.Empty, font);
      exitMenuEntry.Selected += OnCancel;
      items.Add(exitMenuEntry);


      SetMenuEntryText();
    }

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
    /// Set the text that will be displayed for the menu entries
    /// </summary>
    void SetMenuEntryText() {
      // Set the text of each level
      aLevelMenuEntry.Text = "Level " + levelNumber;
      exitMenuEntry.Text = "Back";
    }

    #endregion

    #region Handle Input

    void OnCancel(object sender, PlayerIndexEventArgs e) {
      ExitScreen();
    }

    /// <summary>
    /// Event handler for when the Level menu entry is selected.
    /// </summary>
    void ALevelMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      ScreenList.AddScreen(new LevelStatusScreen(completed, levelScore, levelNumber, completionTime), e.PlayerIndex);
    }

    #endregion
  }
}
