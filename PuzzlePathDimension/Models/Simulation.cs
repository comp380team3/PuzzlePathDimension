using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class Simulation {
    public Ball Ball { get; set; }
    public List<Platform> Platforms { get; set; }
    public Goal Goal { get; set; }
    public Launcher Launcher { get; set; }

    public Texture2D Background { get; set; }

    public Simulation(Level level) {
      // TODO: This clones the list, but references the same platforms.
      // If gameplay may modify properties of platforms (or the goal,
      // or the launcher), a deep copy is necessary instead.
      Platforms = new List<Platform>(level.Platforms);
      Goal = level.Goal;
      Launcher = level.Launcher;
    }
  }
}