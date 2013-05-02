﻿using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics.Contacts;

namespace PuzzlePathDimension {
  /// <summary>
  /// The LevelScoreData structure contains data about the user's performance on
  /// a given attempt of the level.
  /// </summary>
  public struct LevelScoreData {
    /// <summary>
    /// The number of treasures collected in the level.
    /// </summary>
    public int TreasuresCollected { get; set; }
    /// <summary>
    /// The total number of treasures, both collected and uncollected, that
    /// are in the level.
    /// </summary>
    public int TreasuresInLevel { get; set; }
    /// <summary>
    /// The number of balls that the user had remaining when the level was
    /// completed.
    /// </summary>
    public int BallsLeft { get; set; }
    /// <summary>
    /// The time spent in the simulation phase of the level, in seconds.
    /// </summary>
    public int TimeSpent { get; set; }
    /// <summary>
    /// The par time of the level, in seconds.
    /// </summary>
    public int ParTime { get; set; }
    /// <summary>
    /// The score that the user achieved on that level.
    /// </summary>
    public int Score { get; set; }
  }

  /// <summary>
  /// The current state of the simulation.
  /// </summary>
  public enum SimulationState {
    /// <summary>
    /// The level has neither been completed nor failed.
    /// </summary>
    Active,
    /// <summary>
    /// The level has been completed.
    /// </summary>
    Completed,
    /// <summary>
    /// The level has been failed.
    /// </summary>
    Failed
  }

  class Simulation : IRestartable {
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

    /// <summary>
    /// The simulation's ball.
    /// </summary>
    private Ball _ball;

    /// <summary>
    /// The simulation's launcher.
    /// </summary>
    private Launcher _launcher;

    /// <summary>
    /// The simulation's goal.
    /// </summary>
    private Goal _goal;

    /// <summary>
    /// The simulation's list of platforms.
    /// </summary>
    private List<Platform> _platforms;


    private List<Platform> _moveablePlatforms;
    /// <summary>
    /// The simulation's list of treasures.
    /// </summary>
    private List<Treasure> _treasures;

    /// <summary>
    /// The simulation's list of death traps.
    /// </summary>
    private List<DeathTrap> _deathTraps;

    /// <summary>
    /// Gets the simulation's ball.
    /// </summary>
    public Ball Ball {
      get { return _ball; }
    }

    /// <summary>
    /// Gets the simulation's launcher.
    /// </summary>
    public Launcher Launcher {
      get { return _launcher; }
    }

    /// <summary>
    /// Gets the simulation's goal.
    /// </summary>
    public Goal Goal {
      get { return _goal; }
    }

    /// <summary>
    /// Gets the simulation's list of platforms.
    /// </summary>
    public List<Platform> Platforms {
      get { return _platforms; }
    }

    public List<Platform> MoveablePlatforms {
      get { return _moveablePlatforms; }
    }

    /// <summary>
    /// Gets the simulation's list of treasures.
    /// </summary>
    public List<Treasure> Treasures {
      get { return _treasures; }
    }

    /// <summary>
    /// Gets the simulation's list of deathtraps.
    /// </summary>
    public List<DeathTrap> DeathTraps {
      get { return _deathTraps; }
    }

    /// <summary>
    /// The number of attempts that the player starts with.
    /// </summary>
    private int _startingAttempts;
    /// <summary>
    /// The number of attempts that the player has left.
    /// </summary>
    private int _attemptsLeft;

    /// <summary>
    /// Gets the number of attempts that the player has left.
    /// </summary>
    public int AttemptsLeft {
      get { return _attemptsLeft; }
    }


    /// <summary>
    /// The maximum number of platforms the player can add.
    /// </summary>
    private int _additionsAllowed;

    /// <summary>
    /// The number of platforms the player can still add.
    /// </summary>
    public int AdditionsLeft {
      get { return _additionsAllowed - _moveablePlatforms.Count; }
    }

    /// <summary>
    /// The number of treasures that have been collected.
    /// </summary>
    private int _collectedTreasures;

    /// <summary>
    /// Gets the number of treasures that have been collected.
    /// </summary>
    public int CollectedTreasures {
      get { return _collectedTreasures; }
    }

    /// <summary>
    /// The number of times that the ball has bounced off a platform or wall.
    /// </summary>
    private int _bounces;

    /// <summary>
    /// Gets the number of times that the ball has bounced off a platform or wall.
    /// </summary>
    public int Bounces {
      get { return _bounces; }
    }

    /// <summary>
    /// The time that elapsed since the start of the simulation phase.
    /// </summary>
    private float _elapsedTime;
    /// <summary>
    /// Gets the time that elapsed since the start of the simulation phase.
    /// </summary>
    public float ElapsedTime {
      get { return _elapsedTime; }
    }

    /// <summary>
    /// The par time of the level.
    /// </summary>
    private int _parTime;
    /// <summary>
    /// Gets the par time of the level.
    /// </summary>
    public int ParTime {
      get { return _parTime; }
    }

    /// <summary>
    /// The current state of the simulation.
    /// </summary>
    private SimulationState _currentState;

    /// <summary>
    /// Gets the current state of the simulation.
    /// </summary>
    public SimulationState CurrentState {
      get { return _currentState; }
    }

    /// <summary>
    /// Gets or sets the background of the playing field.
    /// </summary>
    public Texture2D Background { get; set; }

    /// <summary>
    /// The ball texture to use.
    /// </summary>
    private Texture2D _ballTex;

    /// <summary>
    /// This delegate is called when the user completes a level.
    /// </summary>
    /// <param name="scoreData">A structure containing information about
    /// the user's performance.</param>
    public delegate void LevelCompleted(LevelScoreData scoreData);
    /// <summary>
    /// This delegate is called when the user fails a level.
    /// </summary>
    public delegate void LevelFailed();
    /// <summary>
    /// This delegate is called when something touches a wall.
    /// </summary>
    public delegate void WallTouch();

    /// <summary>
    /// Occurs when the level has been completed.
    /// </summary>
    public event LevelCompleted OnCompletion;
    /// <summary>
    /// Occurs when the level has been failed.
    /// </summary>
    public event LevelFailed OnFailure;
    /// <summary>
    /// Occurs when a wall has been touched.
    /// </summary>
    public event WallTouch OnWallCollision;

    /// <summary>
    /// Constructs a Simulation object.
    /// </summary>
    /// <param name="level">The Level to use to create the simulation.</param>
    /// <param name="content">The ContentManager to use to load the ball texture.</param>
    public Simulation(Level level, ContentManager content) {
      // TODO: This clones the list, but references the same platforms.
      // If gameplay may modify properties of platforms (or the goal,
      // or the launcher), a deep copy is necessary instead.
      _platforms = new List<Platform>(level.Platforms);
      _treasures = new List<Treasure>(level.Treasures);
      _deathTraps = new List<DeathTrap>(level.DeathTraps);
      _goal = level.Goal;
      _launcher = level.Launcher;
      _moveablePlatforms = new List<Platform>();

      // Initialize various stats.
      _startingAttempts = level.Attempts;
      _parTime = level.ParTime;
      _attemptsLeft = _startingAttempts;
      _collectedTreasures = 0;
      _bounces = 0;
      //hard coded amount of additions.
      _additionsAllowed = 3;

      Background = content.Load<Texture2D>("Texture/GameScreen");

      // Load the ball's texture and store it.
      _ballTex = content.Load<Texture2D>("Texture/ball");
      // Create the physics simulation.
      //InitWorld();

      // Allow the user to interact with the simulation, and start the timer.
      _currentState = SimulationState.Active;
      _elapsedTime = 0f;
    }

    /// <summary>
    /// Initializes the World object and everything in it.
    /// </summary>
    public void InitWorld() {
      //Change to public so it can be called from the gameplay screen.
      //the world wont exist till it is class from the GameplayScreen

      // Create the World object.
      Vector2 gravity = new Vector2(0f, 9.8f);
      _world = new World(gravity);

      // Make sure that the level has boundaries.
      CreateWalls();
      // Make sure that bounces against walls are counted.
      OnWallCollision += IncrementBounces;

      // Create a launcher with a ball in it.
      _ball = new Ball(_ballTex);
      _ball.InitBody(_world);
      _launcher.LoadBall(_ball);

      // Add the goal to the world.
      _goal.InitBody(_world);
      // When the ball touches the goal, conclude the simulation phase.
      _goal.OnGoalCollision += Complete;

      // Add the platforms to the world.
      foreach (Platform plat in _platforms) {
        plat.InitBody(_world);
        plat.OnPlatformCollision += IncrementBounces;
      }
      foreach (Platform plat in _moveablePlatforms) {
        plat.InitBody(_world);
        plat.OnPlatformCollision += IncrementBounces;
      }
      // Add the treasures to the world.
      foreach (Treasure treasure in _treasures) {
        treasure.InitBody(_world);
        treasure.OnTreasureCollect += IncrementCollected;
      }

      // Add the death traps to the world.
      foreach (DeathTrap trap in _deathTraps) {
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

      // Associate these Body objects with walls.
      left.UserData = "wall";
      right.UserData = "wall";
      top.UserData = "wall";
      bottom.UserData = "wall";

      // Have each wall listen for collision.
      left.OnCollision += HandleWallCollision;
      right.OnCollision += HandleWallCollision;
      top.OnCollision += HandleWallCollision;
      bottom.OnCollision += HandleWallCollision;
    }

    /// <summary>
    /// Called when a collision with a wall occurs.
    /// </summary>
    /// <param name="fixtureA">The first fixture that has collided.</param>
    /// <param name="fixtureB">The second fixture that has collided.</param>
    /// <param name="contact">The Contact object that contains information about the collision.</param>
    /// <returns>Whether the collision should still happen.</returns>
    private bool HandleWallCollision(Fixture fixtureA, Fixture fixtureB, Contact contact) {
      // Check if one of the Fixtures belongs to a ball.
      bool causedByBall = (string)fixtureA.Body.UserData == "ball" || (string)fixtureB.Body.UserData == "ball";

      // A subtle fact about the OnCollision event is that it is only called
      // when the associated Contact object is changed from not-touching to touching.
      // While two objects are still touching each other, OnCollision won't be called again.
      if (contact.IsTouching() && causedByBall) {
        // Call any methods that are listening to this event.
        if (OnWallCollision != null) {
          OnWallCollision();
        }
        // The ball's trajectory should definitely be affected by the wall, so tell
        // the physics engine that by returning true.
        return true;
      }
      // If it's not a ball, then we don't really need to worry about it. Hopefully.
      return false;
    }

    /// <summary>
    /// Advances the simulation by one step.
    /// </summary>
    /// <param name="time">The amount of time that has passed since the last update.</param>
    public void Step(float time) {
      // Let the World do stuff.
      _world.Step(time);

      // Add to the timer.
      _elapsedTime += time;

      // Checks if a launched ball has no velocity, which ends the current attempt.
      if (_ball.Velocity.Equals(Vector2.Zero) && !_launcher.Movable
        && _currentState == SimulationState.Active) {
        EndAttempt();
      }
    }

    /// <summary>
    /// Increments the number of bounces by one.
    /// </summary>
    private void IncrementBounces() {
      IncrementBounces(false);
    }

    /// <summary>
    /// Increments the number of bounces by one. This particular overload of the method
    /// is for the PlatformTouched delegate.
    /// </summary>
    private void IncrementBounces(bool breakable) {
      _bounces++;
    }

    /// <summary>
    /// Increments the number of collected treasures by one.
    /// </summary>
    private void IncrementCollected() {
      _collectedTreasures++;
    }

    /// <summary>
    /// Completes the level.
    /// </summary>
    private void Complete() {
      // Stop the ball and don't accept any more input for the simulation.
      _currentState = SimulationState.Completed;
      _ball.Stop(_world);

      // Do anything that needs to be done when the level is completed.
      if (OnCompletion != null) {
        OnCompletion(CreateScoreData());
      }
    }

    /// <summary>
    /// Creates a LevelScoreData structure containing information about the
    /// user's performance on a level.
    /// </summary>
    /// <returns>Information about the user's score.</returns>
    private LevelScoreData CreateScoreData() {
      LevelScoreData clearData = new LevelScoreData();

      clearData.TreasuresCollected = _collectedTreasures;
      clearData.TreasuresInLevel = _treasures.Count;
      clearData.BallsLeft = _attemptsLeft;
      clearData.TimeSpent = (int)_elapsedTime; // Cut off the fractional part
      clearData.ParTime = _parTime;

      // 500 per treasure, 150 per ball left, 100 for beating the par time, 42 for level completion
      int treasureScore = 500 * _collectedTreasures;
      int ballsLeftScore = 150 * _attemptsLeft;
      int parTimeScore = clearData.TimeSpent <= clearData.ParTime ? 100 : 0;
      clearData.Score = treasureScore + ballsLeftScore + parTimeScore + 42;

      return clearData;
    }

    /// <summary>
    /// Called when someone presses the Confirm button.
    /// </summary>
    public void HandleConfirm() {
      // If the level has been completed or failed, do nothing.
      if (_currentState != SimulationState.Active) {
        return;
      }

      // If the launcher is currently movable, then pressing Confirm
      // shall cause the ball to launch.
      if (_launcher.Movable) {
        _launcher.LaunchBall();
        _attemptsLeft -= 1;
      }
        // While the ball is moving, the user can hit Confirm to destroy
        // the ball.
      else if (_currentState == SimulationState.Active) {
        EndAttempt();
      }
    }

    /// <summary>
    /// Restarts the simulation phase.
    /// </summary>
    public void Restart() {
      // Reset the number of attempts, the various statistics, and lets the user provide input again.
      _attemptsLeft = _startingAttempts;
      _elapsedTime = 0f;
      _collectedTreasures = 0;
      _bounces = 0;
      _currentState = SimulationState.Active;

      // Restore all breakable platforms and treasures.
      foreach (Platform platform in _platforms) {
        platform.Reset();
      }
      foreach (Platform platform in _moveablePlatforms) {
        platform.Reset();
      }
      foreach (Treasure treasure in _treasures) {
        treasure.Reset();
      }

      // If the ball happens to be moving, stop it and put it back into
      // the launcher.
      if (!_launcher.Movable) {
        _ball.Stop(_world);
        _launcher.LoadBall(Ball);
      }
    }

    /// <summary>
    /// Called at the end of every attempt.
    /// </summary>
    private void EndAttempt() {
      // Stop the ball.
      _ball.Stop(_world);

      // Don't load a new ball if the player ran out of balls.
      if (_attemptsLeft > 0) {
        _launcher.LoadBall(Ball);
      } else {
        _currentState = SimulationState.Failed;
        Console.WriteLine("You lose!");

        // Do anything that needs to be done when the level is failed.
        if (OnFailure != null) {
          OnFailure();
        }
      }
    }


  }
}