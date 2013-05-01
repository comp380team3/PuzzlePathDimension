using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PuzzlePathDimension {
  public class LevelEntry {
    [XmlAttribute("path")]
    public string Path { get; set; }
    [XmlAttribute("id")]
    public string Id { get; set; }
  }

  [XmlRoot("Levels")]
  public class LevelGroup {
    [XmlElement("Level")]
    public List<LevelEntry> Entries { get; set; }
  }
}
