using System;
using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  // Please, please, please ensure that this does not become a god object.
  public class TopLevelModel {
    public VirtualController Controller { get; set; }
    public UserPrefs Prefs { get; set; }
    public Game Game { get; set; }
    public Scene Scene { get; set; }

    public IObservable<VirtualControllerState> Input { get; set; }
  }
}
