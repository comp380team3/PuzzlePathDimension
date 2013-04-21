using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzlePathDimension {
  /// <summary>
  /// The UserPrefs class stores the user's preferences, which are adjusted in the options
  /// screen.
  /// </summary>
  public class UserPrefs {
    /// <summary>
    /// Gets or sets whether sounds should be played.
    /// </summary>
    public bool PlaySounds { get; set; }
    /// <summary>
    /// Gets or sets an integer (maybe an enum?) that represents the currently
    /// selected controller.
    /// </summary>
    public int ControllerType { get; set; }
    /// <summary>
    /// Gets or sets whether the active controller type was changed. This is mainly for
    /// the virtual controller's use.
    /// </summary>
    public bool ControllerChanged { get; set; }

    /// <summary>
    /// Constructs a UserPrefs object.
    /// </summary>
    public UserPrefs() {
      PlaySounds = true;
      ControllerType = 0;
      ControllerChanged = false;
    }
  }
}
