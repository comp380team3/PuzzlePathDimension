using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PuzzlePathDimension {
  /// <summary>
  /// The UserPrefs class stores the user's preferences, which are adjusted in the options
  /// screen.
  /// </summary>
  public class UserPrefs {
    /// <summary>
    /// Gets or sets whether sounds should be played.
    /// </summary>
    [XmlAttribute("sounds-enabled")]
    public bool PlaySounds { get; set; }

    /// <summary>
    /// Gets or sets the enum value that represents the currently
    /// selected controller.
    /// </summary>
    [XmlIgnore]
    public InputType ControllerType { get; set; }

    /// Constructs a UserPrefs object.
    /// </summary>
    public UserPrefs() {
      PlaySounds = true;
      ControllerType = 0;
    }
  }
}
