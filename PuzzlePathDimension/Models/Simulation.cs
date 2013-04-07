using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Factories;

namespace PuzzlePathDimension {
  class Simulation {
    /// <summary>
    /// The width of the playing field, in pixels.
    /// </summary>
    public static readonly int FieldWidth = 800;
    /// <summary>
    /// The height of the playing field, in pixels.
    /// </summary>
    public static readonly int FieldHeight = 600;

    /// <summary>
    /// The World object that represents the simulation phase of the level.
    /// </summary>
    private World _world;

    /// <summary>
    /// Gets the world object that represents the simulation phase of the level.
    /// </summary>
    public World World {
      get { return _world; }
    }

    public Ball Ball { get; set; }
    public List<Platform> Platforms { get; set; }
    public List<Treasure> Treasures { get; set; }
    public List<DeathTrap> DeathTraps { get; set; }
    public Goal Goal { get; set; }
    public Launcher Launcher { get; set; }
    public int Attempts { get; set; }
    public bool Completed { get; set; }

    public Texture2D Background { get; set; }

    /// <summary>
    /// Constructs a Simulation object.
    /// </summary>
    /// <param name="level"></param>
    public Simulation(Level level) {
      // TODO: This clones the list, but references the same platforms.
      // If gameplay may modify properties of platforms (or the goal,
      // or the launcher), a deep copy is necessary instead.
      Platforms = new List<Platform>(level.Platforms);
      Treasures = new List<Treasure>(level.Treasures);
      DeathTraps = new List<DeathTrap>(level.DeathTraps);
      Goal = level.Goal;
      Launcher = level.Launcher;
      // TODO: Hard-coded for now
      Attempts = 3;
      Completed = false;

      InitWorld();
    }

    /// <summary>
    /// Initializes the World object.
    /// </summary>
    private void InitWorld() {
      // Create the World object.
      Vector2 gravity = new Vector2(0f, 9.8f);
      _world = new World(gravity);

      // Make sure that the level has boundaries.
      CreateWalls();

      // Add the platforms to the world.
      foreach (Platform plat in Platforms) {
        plat.InitBody(_world);
      }
    }

    /// <summary>
    /// Create the walls for the level.
    /// </summary>
    private void CreateWalls() {
      // Convert everything we need to meters first.
      float wallThickness = UnitConverter.ToMeters(10f);
      float fieldWidthMeters = UnitConverter.ToMeters(FieldWidth);
      float fieldHeightMeters = UnitConverter.ToMeters(FieldHeight);

      // Remember that the physics engine treats the center as the position of the Body,
      // so get the midpoint of the wall and use that as the position.
      Body left = BodyFactory.CreateRectangle(_world, wallThickness, fieldHeightMeters, 1);
      left.Position = new Vector2(0, fieldHeightMeters / 2);

      Body right = BodyFactory.CreateRectangle(_world, wallThickness, fieldHeightMeters, 1);
      right.Position = new Vector2(fieldWidthMeters, fieldHeightMeters / 2);

      Body top = BodyFactory.CreateRectangle(_world, fieldWidthMeters, wallThickness, 1);
      top.Position = new Vector2(fieldWidthMeters / 2, 0);

      Body bottom = BodyFactory.CreateRectangle(_world, fieldWidthMeters, wallThickness, 1);
      bottom.Position = new Vector2(fieldWidthMeters / 2, fieldHeightMeters);
    }

    /// <summary>
    /// Advances the simulation by one step.
    /// </summary>
    /// <param name="time">The amount of time that has passed since the last update.</param>
    public void Step(float time) {
      _world.Step(time);
    }

    /// <summary>
    /// Restarts the simulation phase. (This might not be needed if we make a deep
    /// copy of the List objects.)
    /// </summary>
    public void Restart() {
      Attempts = 3;
      Completed = false;

      foreach (Platform platform in Platforms) {
        platform.Visible = true;
      }

      foreach (Treasure treasure in Treasures) {
        treasure.Active = true;
      }
    }
  }
}