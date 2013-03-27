using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class Simulation {
    /// <summary>
    /// The width of the playing field.
    /// </summary>
    public static readonly int FieldWidth = 800;
    /// <summary>
    /// The height of the playing field.
    /// </summary>
    public static readonly int FieldHeight = 600;

    public Ball Ball { get; set; }
    public List<Platform> Platforms { get; set; }
    public List<Treasure> Treasures { get; set; }
    public List<DeathTrap> DeathTraps { get; set; }
    public Goal Goal { get; set; }
    public Launcher Launcher { get; set; }

    public Texture2D Background { get; set; }
  }
}