using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

    /// <summary>
    /// Whether the player can interact with the simulation.
    /// </summary>
    private bool _active;

    /// <summary>
    /// Gets whether the player can interact with the simulation.
    /// </summary>
    public bool Active {
      get { return _active; }
    }

    /// <summary>
    /// Gets or sets the background of the playing field.
    /// </summary>
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
      // TODO: Hard-coded for now; this should be loaded from the level
      Attempts = 3;
      _active = true;

      // Create the physics simulation.
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

      // Add the goal to the world.
      Goal.InitBody(_world);
      // When the ball touches the goal, conclude the simulation phase.
      Goal.OnGoalCollision += Complete;

      // Add the platforms to the world.
      foreach (Platform plat in Platforms) {
        plat.InitBody(_world);
      }

      // Add the treasures to the world.
      foreach (Treasure treasure in Treasures) {
        treasure.InitBody(_world);
      }

      // Add the death traps to the world.
      foreach (DeathTrap trap in DeathTraps) {
        trap.InitBody(_world);
        trap.OnTrapCollision += EndAttempt;
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
      // Let the World do stuff.
      _world.Step(time);

      // Checks if a launched ball has no velocity, which ends the current attempt.
      if (Ball.Velocity.Equals(Vector2.Zero) && !Launcher.Movable && _active) {
        EndAttempt();
      }
    }

    /// <summary>
    /// Completes the level.
    /// </summary>
    private void Complete() {
      _active = false;
      Ball.Stop(_world);

      Console.WriteLine("You're winner!");
      int collected = 0;
      foreach (Treasure treasure in Treasures) {
        if (treasure.Collected) {
          collected++;
        }
      }
      Console.WriteLine("Treasures obtained: " + collected + "/" + Treasures.Count);
    }

    /// <summary>
    /// Called when someone presses the Confirm button.
    /// </summary>
    public void HandleConfirm() {
      // If the level has been completed or failed, do nothing.
      if (!_active) {
        return;
      }

      // If the launcher is currently movable, then pressing Confirm
      // shall cause the ball to launch.
      if (Launcher.Movable) {
        Launcher.LaunchBall();
        Attempts -= 1;
        Console.WriteLine("Attempts left: " + Attempts);
      }
      // While the ball is moving, the user can hit Confirm to destroy
      // the ball.
      else if (_active) {
        EndAttempt();
      }
    }

    /// <summary>
    /// Restarts the simulation phase.
    /// </summary>
    public void Restart() {
      Attempts = 3;
      _active = true;

      foreach (Platform platform in Platforms) {
        platform.Reset();
      }

      foreach (Treasure treasure in Treasures) {
        treasure.Reset();
      }

      if (!Launcher.Movable) {
        Ball.Stop(_world);
        Launcher.LoadBall(Ball);
      }
    }

    /// <summary>
    /// Called at the end of every attempt.
    /// </summary>
    private void EndAttempt() {
      // Stop the ball.
      Ball.Stop(_world);

      // Don't load a new ball if the player ran out of balls.
      if (Attempts > 0) {
        Launcher.LoadBall(Ball);
      } else {
        _active = false;
        Console.WriteLine("You lose!");
      }
    }
  }
}