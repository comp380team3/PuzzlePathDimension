using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace PuzzlePathDimension {
  /// <summary>
  /// The LevelEntry class contains a level's filename and identifier.
  /// </summary>
  public class LevelEntry {
    /// <summary>
    /// The level's filename. 
    /// </summary>
    /// <remarks>
    /// "path" is really a bad name for the XML attribute, but I don't
    /// want to change it right now.
    /// </remarks>
    [XmlAttribute("path")]
    public string FileName { get; set; }

    /// <summary>
    /// The full path to the level file.
    /// </summary>
    [XmlIgnore]
    public string FullPath {
      get { return Configuration.UserDataPath + Path.DirectorySeparatorChar + "Level" + Path.DirectorySeparatorChar + FileName; }
    }

    /// <summary>
    /// The identifier of the level.
    /// </summary>
    [XmlAttribute("id")]
    public string Id { get; set; }

    /// <summary>
    /// Constructs a new LevelEntry object.
    /// </summary>
    public LevelEntry() { }
  }

  /// <summary>
  /// The LevelGroup class is a ordered list of level entries that
  /// can be serialized to an XML file. It determines the ordering of the
  /// levels, especially for level unlocking purposes.
  /// </summary>
  [XmlRoot("Levels")]
  public class LevelGroup {
    /// <summary>
    /// The list of level entries.
    /// </summary>
    [XmlElement("Level")]
    public List<LevelEntry> Entries { get; set; }

    /// <summary>
    /// Constructs a new LevelGroup object.
    /// </summary>
    public LevelGroup() {
      Entries = new List<LevelEntry>();
    }

    /// <summary>
    /// Given the name of the current level, find the next level in the
    /// LevelGroup.
    /// </summary>
    /// <param name="levelName">The level id of the current level.</param>
    /// <returns>The LevelEntry of the next level, or null if the
    /// current level was the last level or if no LevelEntry with
    /// the given levelName exists.</returns>
    public LevelEntry FindNextLevel(string levelName) {
      int currentLevelIndex = -1;

      // Check each entry for a level entry that matches the current
      // level's name.
      for (int i = 0; i < Entries.Count; i++) {
        LevelEntry entry = Entries[i];
        if (entry.Id == levelName) {
          currentLevelIndex = i;
          break;
        }
      }

      // If a level with that name isn't found, end early and return null.
      if (currentLevelIndex == -1) {
        return null;
      }

      // The index of the next level is n + 1.
      int nextLevelIndex = currentLevelIndex + 1;
      // If there is no next level, return null.
      if (nextLevelIndex >= Entries.Count) {
        return null;
      } else {
        // Otherwise, return the next level's LevelEntry.
        return Entries[nextLevelIndex];
      }
    }

    /// <summary>
    /// Loads a LevelGroup object from an XML file.
    /// </summary>
    /// <param name="filename">The name of the XML file to load the level group from.</param>
    /// <returns>The deserialized LevelGroup object.</returns>
    public static LevelGroup Load(string filename) {
      XmlSerializer serializer = new XmlSerializer(typeof(LevelGroup));

      using (XmlReader reader = new XmlTextReader(filename)) {
        return (LevelGroup)serializer.Deserialize(reader);
      }
    }

    /// <summary>
    /// Saves the LevelGroup object to an XML file.
    /// </summary>
    /// <param name="filename">The filename to use.</param>
    public void Save(string filename) {
      XmlSerializer serializer = new XmlSerializer(typeof(LevelGroup));

      using (XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8)) {
        writer.Formatting = Formatting.Indented;
        serializer.Serialize(writer, this);
      }
    }

    // Below is the code that I used to generate the initial levellist.xml file.
    // I'll keep it here in case I need it in the future, which I probably will. - Jorenz

    /******************************************
    public static void GenerateLevelGroup() {
      LevelGroup levels = new LevelGroup();

      string basePath = Content.RootDirectory + Path.DirectorySeparatorChar + "Level";

      string[] levelPaths = Directory.GetFiles(basePath);
      foreach (string pathName in levelPaths) {
        XmlDocument doc = new XmlDocument();
        doc.Load(pathName);

        XmlElement node = (XmlElement)doc.GetElementsByTagName("level")[0];
        string id = Convert.ToString(node.Attributes["name"].Value);

        string fileName = Path.GetFileName(pathName);

        LevelEntry entry = new LevelEntry();
        entry.Path = fileName;
        entry.Id = id;
        levels.Entries.Add(entry);
      }

      levels.Save("Content/levellist.xml");
    }
    *******************************************/
  }
}
