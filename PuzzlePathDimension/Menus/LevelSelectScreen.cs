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
    MenuTemplate menuTemplate = new MenuTemplate();

    /// <summary>
    /// Menu entries for the Level Select Screen.
    /// </summary>
    MenuButton aLevelMenuEntry;
    MenuButton exitMenuEntry;

    public struct LevelInfo {

      public string LevelName { get; set; }

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

    List<LevelInfo> levelSet;
    LevelInfo levelInfo; 

    #region Initialization

    /// <summary>
    /// Contructor
    /// Read an xml file and obtain information for each level in the xml file.
    /// </summary>
    public LevelSelectScreen(ContentManager Content) {
      // Add the levels to the screen
      // Note: need xml file format to be completed to add level information
      XmlDocument doc;
      XmlElement node;
      string[] levels = Directory.GetFiles(Content.RootDirectory + "\\Level");
      levelSet = new List<LevelInfo>();
      levelInfo = new LevelInfo();


      foreach (string name in levels) {
        doc = new XmlDocument();
        doc.Load(name);

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
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("Font/menufont");

      menuTemplate.Title = new TextLine("Select A Level", font, new Color(192, 192, 192));


      IList<MenuButton> items = menuTemplate.Items;

      foreach(LevelInfo level in levelSet) {
        aLevelMenuEntry = new MenuButton(level.LevelName, font);
        items.Add(aLevelMenuEntry);
        aLevelMenuEntry.Selected += delegate(object sender, PlayerIndexEventArgs e) { ALevelMenuEntrySelected(sender, e, menuTemplate.SelectedItem); };
      }

      exitMenuEntry = new MenuButton("Exit", font);
      exitMenuEntry.Selected += OnCancel;
      items.Add(exitMenuEntry);
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

    #endregion

    #region Handle Input

    void OnCancel(object sender, PlayerIndexEventArgs e) {
      ExitScreen();
      ScreenList.AddScreen(new MainMenuScreen(), e.PlayerIndex);
    }

    /// <summary>
    /// Event handler for when the Level menu entry is selected.
    /// </summary>
    void ALevelMenuEntrySelected(object sender, PlayerIndexEventArgs e,  int selected) {
      LevelInfo level = levelSet.ElementAt<LevelInfo>(selected);
      ScreenList.AddScreen(new LevelStatusScreen(level.Completed, level.LevelScore, level.LevelName, level.CompletionTime), e.PlayerIndex);
    }

    #endregion
  }
}
