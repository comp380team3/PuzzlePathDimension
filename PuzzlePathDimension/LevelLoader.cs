using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// Provides methods for deserializing Levels from XML files.
  /// </summary>
  static class LevelLoader {
    /// <summary>
    /// Load an XML file from disk, retrieving textures through Content
    /// as necessary.
    /// </summary>
    /// <param name="filename">Path to the file</param>
    /// <param name="Content">Resource manager to load from</param>
    /// <returns>A deserialized level</returns>
    public static Level Load(string filename, ContentManager Content) {
      XmlDocument doc = new XmlDocument();
      doc.Load(filename);

      return LoadLevel(doc, Content);
    }

    private static Level LoadLevel(XmlDocument doc, ContentManager Content) {
      Level level = new Level();
      
      XmlElement levelNode = (XmlElement)doc.GetElementsByTagName("level")[0];

      if (levelNode.Attributes["balls"] == null) { // Fall back
        level.Attempts = 3;
        Console.WriteLine("Warning: the number of attempts wasn't specified.");
      } else {
        level.Attempts = Convert.ToInt16(levelNode.Attributes["balls"].Value);
      }

      if (levelNode.Attributes["par-seconds"] == null) { // Fall back
        level.ParTime = 60;
        Console.WriteLine("Warning: the par time wasn't specified.");
      } else {
        level.ParTime = Convert.ToInt16(levelNode.Attributes["par-seconds"].Value);
      }

      if (levelNode.Attributes["toolbox-types"] == null) {
        level.AllowedPlatTypes = "RBHV";
        Console.WriteLine("Warning: the allowed platform types in the editor wasn't specified.");
      } else {
        Console.WriteLine("this was reached");
        level.AllowedPlatTypes = Convert.ToString(levelNode.Attributes["toolbox-types"].Value);
      }

      foreach (XmlElement node in doc.GetElementsByTagName("platform")) {
        level.Platforms.Add(LoadPlatform(node, Content));
      }
      foreach (XmlElement node in doc.GetElementsByTagName("treasure")) {
        level.Treasures.Add(LoadTreasure(node, Content));
      }
      foreach (XmlElement node in doc.GetElementsByTagName("deathtrap")) {
        level.DeathTraps.Add(LoadDeathTrap(node, Content));
      }

      level.Launcher = LoadLauncher((XmlElement)doc.GetElementsByTagName("launcher")[0], Content);
      level.Goal = LoadGoal((XmlElement)doc.GetElementsByTagName("goal")[0], Content);

      return level;
    }

    private static Platform LoadPlatform(XmlElement node, ContentManager Content) {
      Vector2 position = new Vector2();
      position.X = Convert.ToInt16(node.Attributes["x"].Value);
      position.Y = Convert.ToInt16(node.Attributes["y"].Value);

      Vector2 size = new Vector2();
      size.X = Convert.ToInt16(node.Attributes["width"].Value);
      size.Y = Convert.ToInt16(node.Attributes["height"].Value);

      bool breakable = Convert.ToBoolean(node.Attributes["breakable"].Value);
      Texture2D texture = LoadPlatformTexture(size, breakable, Content);

      Platform platform = new Platform(texture, position, size, breakable);

      return platform;
    }

    /// <summary>
    /// Loads the appropriate texture for a Platform object, based on its size and
    /// whether it is breakable or not.
    /// </summary>
    /// <param name="size">The dimensions of the platform's size.</param>
    /// <param name="breakable">Whether the platform is breakable.</param>
    /// <param name="Content">The resource manager to load from.</param>
    /// <returns>The texture that the platform will be drawn with.</returns>
    private static Texture2D LoadPlatformTexture(Vector2 size, bool breakable, ContentManager Content) {
      // Based on the platform's breakability, figure out which set of textures to look in.
      Dictionary<Vector2, string> dictToUse = breakable ? 
        Platform.BreakablePlatNames : Platform.NormalPlatNames;

      if (dictToUse.ContainsKey(size)) {
        return Content.Load<Texture2D>(dictToUse[size]);
      } else { // fallback
        Console.WriteLine("Warning: This should never be reached.");
        return breakable ? Content.Load<Texture2D>("Texture/platform_breakable") : 
          Content.Load<Texture2D>("Texture/platform");
      }
    }

    private static Treasure LoadTreasure(XmlElement node, ContentManager Content) {
      Vector2 position = new Vector2();
      position.X = Convert.ToInt16(node.Attributes["x"].Value);
      position.Y = Convert.ToInt16(node.Attributes["y"].Value);

      Treasure treasure = new Treasure(Content.Load<Texture2D>("Texture/treasure"), position);

      return treasure;
    }

    private static DeathTrap LoadDeathTrap(XmlElement node, ContentManager Content) {
      Vector2 position = new Vector2();
      position.X = Convert.ToInt16(node.Attributes["x"].Value);
      position.Y = Convert.ToInt16(node.Attributes["y"].Value);

      DeathTrap deathtrap = new DeathTrap(Content.Load<Texture2D>("Texture/deathtrap"), position);

      return deathtrap;
    }

    private static Launcher LoadLauncher(XmlElement node, ContentManager Content) {
      Vector2 position = new Vector2();
      position.X = Convert.ToInt16(node.Attributes["x"].Value);
      position.Y = Convert.ToInt16(node.Attributes["y"].Value);

      Texture2D[] launcherTextures = {Content.Load<Texture2D>("Texture/launcher"),
                                      Content.Load<Texture2D>("Texture/LauncherBase"),
                                      Content.Load<Texture2D>("Texture/power_meter")};

      Launcher launcher = new Launcher(launcherTextures, position);

      return launcher;
    }

    private static Goal LoadGoal(XmlElement node, ContentManager Content) {
      Vector2 position = new Vector2();
      position.X = Convert.ToInt16(node.Attributes["x"].Value);
      position.Y = Convert.ToInt16(node.Attributes["y"].Value);

      Goal goal = new Goal(Content.Load<Texture2D>("Texture/goal"), position);

      return goal;
    }
  }
}



