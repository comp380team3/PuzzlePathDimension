using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class Simulation {
    public Ball Ball { get; set; }
    public List<Platform> Platforms { get; set; }
    public Goal Goal { get; set; }
    public Launcher Launcher { get; set; }

    public Texture2D Background { get; set; }
  }
}