using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzlePathDimension {
  class Level {
    public string Name { get; set; }

    public int Attempts { get; set; }
    public int ParTime { get; set; }

    public Goal Goal { get; set; }
    public Launcher Launcher { get; set; }
    public List<Platform> Platforms { get; set; }
    public List<Treasure> Treasures { get; set; }
    public List<DeathTrap> DeathTraps { get; set; }

    /// <summary>
    /// The string that represents the platform types that can
    /// be used in the editor phase of gameplay.
    /// </summary>
    public string AllowedPlatTypes { get; set; }

    public Level() {
      Platforms = new List<Platform>();
      Treasures = new List<Treasure>();
      DeathTraps = new List<DeathTrap>();
    }
  }
}
