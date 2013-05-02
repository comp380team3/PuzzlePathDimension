using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace PuzzlePathDimension {
  /// <summary>
  /// The LevelEntry class contains a level's filename and identifier.
  /// </summary>
  public class LevelEntry {
    /// <summary>
    /// The path to the level's file. For built-in levels, it's the file
    /// name itself, so if needed, make sure to add Content/Level/ to it.
    /// </summary>
    [XmlAttribute("path")]
    public string Path { get; set; }
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
