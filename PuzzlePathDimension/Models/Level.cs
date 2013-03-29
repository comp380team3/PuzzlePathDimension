using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzlePathDimension {
  class Level {
    public Goal Goal { get; set; }
    public Launcher Launcher { get; set; }
    public List<Platform> Platforms { get; set; }
  }
}
