﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzlePathDimension {
  class Level {
    public Goal Goal { get; set; }
    public Launcher Launcher { get; set; }
    public List<Platform> Platforms { get; set; }
    public List<Treasure> Treasures { get; set; }
    public List<DeathTrap> DeathTraps { get; set; }

    public Level() {
      Platforms = new List<Platform>();
      Treasures = new List<Treasure>();
      DeathTraps = new List<DeathTrap>();
    }
  }
}
