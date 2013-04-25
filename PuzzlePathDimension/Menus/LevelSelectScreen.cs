using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class LevelSelectScreen : GameScreen {

    /// <summary>
    /// Structure that contains information for a level.
    /// </summary>
    public struct LevelInfo {

      public string LevelName { get; set; }

      public string FileName { get; set; }

      /// <summary>
      /// The score for the current level.
      /// </summary>
      public int LevelScore { get; set; }

      /// <summary>
      /// The level is complete if true, otherwise the level is incomplete.
      /// </summary>
      public bool Completed { get; set; }

      /// <summary>
      /// The time that the user spent to complete the current level.
      /// </summary>
      public string CompletionTime { get; set; }
    }

    MenuTemplate menuTemplate = new MenuTemplate();

    IList<MenuButton> items;

    /// <summary>
    /// Menu Button for the Level menu entry.
    /// </summary>
    MenuButton aLevelMenuEntry;

    /// <summary>
    /// Menu Button for the Exit menu entry.
    /// </summary>
    MenuButton exitMenuEntry;

    /// <summary>
    /// Menu Button for the Next menu entry.
    /// </summary>
    MenuButton nextMenuEntry;

    /// <summary>
    /// Menu Button for the Back menu entry.
    /// </summary>
    MenuButton backMenuEntry;

    /// <summary>
    /// List of LevelInfo structures.
    /// </summary>
    List<LevelInfo> levelSet;

    /// <summary>
    /// Contains the information of the current level.
    /// </summary>
    LevelInfo levelInfo;

    int currentLevel;

    int numberOfLevelsPerPage = 7;

    int setOfLevels = 0;

    int numberOfTimesNextSelected = 0;

    ContentManager content;

    SpriteFont font;

    #region Initialization

    /// <summary>
    /// Contructor
    /// Read an xml file and obtain information for each level in the xml file.
    /// </summary>
    public LevelSelectScreen(ContentManager Content) {

      XmlDocument doc;
      XmlElement node;
      string[] levels = Directory.GetFiles(Content.RootDirectory + "\\Level");
      levelSet = new List<LevelInfo>();
      levelInfo = new LevelInfo();
      currentLevel = 0;

      // go through all the levels in the file
      foreach (string name in levels) {
        doc = new XmlDocument();
        doc.Load(name);

        levelInfo.FileName = name;

        node = (XmlElement)doc.GetElementsByTagName("level")[0];
        levelInfo.LevelName = Convert.ToString(node.Attributes["name"].Value);

        node = (XmlElement)doc.GetElementsByTagName("score")[0];
        levelInfo.LevelScore = Convert.ToInt16(node.Attributes["value"].Value);

        node = (XmlElement)doc.GetElementsByTagName("completed")[0];
        levelInfo.Completed = Convert.ToBoolean(node.Attributes["value"].Value);

        node = (XmlElement)doc.GetElementsByTagName("completionTime")[0];
        levelInfo.CompletionTime = Convert.ToString(node.Attributes["time"].Value);

        levelSet.Add(levelInfo);
      }

      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
      
      items = menuTemplate.Items;
    }

    /// <summary>
    /// Load content that will be used to create the level select screen.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      font = shared.Load<SpriteFont>("Font/menufont");
      content = shared;

      menuTemplate.Title = new TextLine("Select A Level", font, new Color(192, 192, 192));

      for (int count = 0; currentLevel < levelSet.Count && count < numberOfLevelsPerPage; count++) {
        levelInfo = levelSet.ElementAt<LevelInfo>(currentLevel);
        aLevelMenuEntry = new MenuButton(levelInfo.LevelName, font);
        items.Add(aLevelMenuEntry);
        aLevelMenuEntry.Selected += () => ALevelMenuEntrySelected(menuTemplate.SelectedItem);
        currentLevel = currentLevel + 1;
        setOfLevels = count + 1;
      }

      exitMenuEntry = new MenuButton("Exit", font);
      exitMenuEntry.Selected += OnCancel;
      

      nextMenuEntry = new MenuButton("Next", font);

      backMenuEntry = new MenuButton("Back", font);

      
      nextMenuEntry.Selected += NextMenuEntrySelected;

      
      backMenuEntry.Selected += BackMenuEntrySelected;

      if ((levelSet.Count - currentLevel) > 0) {
          items.Add(nextMenuEntry);
      } 

      if ((currentLevel - numberOfLevelsPerPage) > 0) {
          items.Add(backMenuEntry);
      }

      items.Add(exitMenuEntry);
    }

    /// <summary>
    /// Handle user Input.
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
    /// Update the Screen.
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
    /// Draw all the Level menu entries to the screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);
      menuTemplate.Draw(spriteBatch, gameTime);
    }

    #endregion

    #region Handle Input

    /// <summary>
    /// Event handler for when the Exit menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnCancel() {
      ExitScreen();
      ScreenList.AddScreen(new MainMenuScreen());
    }

    /// <summary>
    /// Event handler for when the Level menu entry is selected.
    /// </summary>
    void ALevelMenuEntrySelected(int selected) {
      if (currentLevel > numberOfLevelsPerPage * (numberOfTimesNextSelected)) {
        selected = selected + (numberOfLevelsPerPage * (numberOfTimesNextSelected));
      } else {
        selected = selected + currentLevel - setOfLevels;
      }
      LevelInfo level = levelSet.ElementAt<LevelInfo>(selected);
      ScreenList.AddScreen(new LevelStatusScreen(level));
    }

    void NextMenuEntrySelected() {
      turnPage(false);
      numberOfTimesNextSelected++;
    }

    void BackMenuEntrySelected() {
      turnPage(true);
    }

    #endregion

    private void turnPage(bool back) {
      items.Clear();
      if (back) {
        currentLevel = currentLevel - numberOfLevelsPerPage - setOfLevels;
        LoadContent(content);
        menuTemplate.SelectedItem = 0;//currentLevel - setOfLevels;
      } else {
        LoadContent(content);
        menuTemplate.SelectedItem = 0;
      }
    }


  }
}
