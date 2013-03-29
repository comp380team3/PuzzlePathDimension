using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  ///   Provides methods for deserializing Levels from XML files.
  /// </summary>
  static class LevelLoader {
    /// <summary>
    ///   Load an XML file from disk, retrieving textures through Content
    ///   as necessary.
    /// </summary>
    /// <param name="filename">Path to the file</param>
    /// <param name="Content">Resource manger to load from</param>
    /// <returns>A deserialized level</returns>
    public static Level Load(string filename, ContentManager Content) {
      XmlDocument doc = new XmlDocument();
      doc.Load(filename);

      return LoadLevel(doc, Content);
    }

    private static Level LoadLevel(XmlDocument doc, ContentManager Content) {
      Level level = new Level();

      foreach (XmlElement node in doc.GetElementsByTagName("platform")) {
        level.Platforms.Add(LoadPlatform(node, Content));
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
      size.Y = Convert.ToInt16(node.Attributes["length"].Value);

      Platform platform = new Platform();
      platform.Initialize(Content.Load<Texture2D>("platform_new"), position, size);

      return platform;
    }

    private static Launcher LoadLauncher(XmlElement node, ContentManager Content) {
      Vector2 position = new Vector2();
      position.X = Convert.ToInt16(node.Attributes["x"].Value);
      position.Y = Convert.ToInt16(node.Attributes["y"].Value);

      Launcher launcher = new Launcher();
      launcher.Initialize(Content.Load<Texture2D>("launcher"), position);

      return launcher;
    }

    private static Goal LoadGoal(XmlElement node, ContentManager Content) {
      Vector2 position = new Vector2();
      position.X = Convert.ToInt16(node.Attributes["x"].Value);
      position.Y = Convert.ToInt16(node.Attributes["y"].Value);

      Goal goal = new Goal();
      goal.Initialize(Content.Load<Texture2D>("goal"), position);

      return goal;
    }
  }
}



