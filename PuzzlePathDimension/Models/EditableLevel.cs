using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics.Contacts;

namespace PuzzlePathDimension {

  class EditableLevel : IRestartable {
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

    /// <summary>
    /// The Levels moveble platforms
    /// </summary>
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
    /// Gets or sets the name of the level being edited.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the simulation's launcher.
    /// </summary>
    public Launcher Launcher {
      get { return _launcher; }
      set { _launcher = value;}
    }

    /// <summary>
    /// Gets the simulation's goal.
    /// </summary>
    public Goal Goal {
      get { return _goal; }
      set { _goal = value;}
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
    /// The maximum number of platforms the player can add.
    /// </summary>
    private int _additionsAllowed;

    /// <summary>
    /// The number of platforms the player can still add.
    /// </summary>
    public int AdditionsLeft {
      get { return _additionsAllowed - _moveablePlatforms.Count; }
    }

    private int _attempts;
    public int Attempts { get { return _attempts; } }

    private int _parTime;
    public int ParTime { get { return _parTime; } }

    /// <summary>
    /// Gets or sets the background of the playing field.
    /// </summary>
    public Texture2D Background { get; set; }


    private Boolean _custom;
    public Boolean Custom {
      get {return _custom;}
    }

    /// <summary>
    /// The ball texture to use.
    /// </summary>
    private Texture2D _ballTex;



    private String typesAllowed;

    public String TypesAllowed { get { return typesAllowed; } }

    public EditableLevel(ContentManager content) {
      _platforms = new List<Platform>();
      _treasures = new List<Treasure>();
      _deathTraps = new List<DeathTrap>();
      _moveablePlatforms = new List<Platform>();
      _attempts = 4;
      _parTime = 1;
      _additionsAllowed = 30;
      typesAllowed = "RBHV";
      _ballTex = content.Load<Texture2D>("Texture/ball");
      Background = content.Load<Texture2D>("Texture/GameScreen");
      _custom = true;
    }

    /// <summary>
    /// Constructs a EditableLevel object.
    /// </summary>
    /// <param name="level">The Level to use to create the simulation.</param>
    /// <param name="content">The ContentManager to use to load the ball texture.</param>
    public EditableLevel(Level level, ContentManager content) {
      // TODO: This clones the list, but references the same platforms.
      // If gameplay may modify properties of platforms (or the goal,
      // or the launcher), a deep copy is necessary instead.
      _platforms = new List<Platform>(level.Platforms);
      _treasures = new List<Treasure>(level.Treasures);
      _deathTraps = new List<DeathTrap>(level.DeathTraps);
      _goal = level.Goal;
      _launcher = level.Launcher;
      _moveablePlatforms = new List<Platform>();

      Name = level.Name;

      _attempts = level.Attempts;
      _parTime = level.ParTime;
      
      //hard coded amount of additions.
      _additionsAllowed = 3;

      typesAllowed = level.AllowedPlatTypes;

      // Load the ball's texture and store it.
      _ballTex = content.Load<Texture2D>("Texture/ball");
      _custom = false;
      // Allow the user to interact with the simulation, and start the timer.
      // _currentState = SimulationState.Active;
    }




    /// <summary>
    /// Remove all added platforms.
    /// </summary>
    public void Restart() {
      _moveablePlatforms = new List<Platform>();
    }



    public Boolean FindCollision() {
      List<Rectangle> rects = new List<Rectangle>();
      foreach (Platform platform in _platforms) {
        rects.Add(new Rectangle((int)platform.Origin.X, (int)platform.Origin.Y, platform.Width, platform.Height));
      }
      foreach (Platform platform in _moveablePlatforms) {
        rects.Add(new Rectangle((int)platform.Origin.X, (int)platform.Origin.Y, platform.Width, platform.Height));
      }
      Rectangle launcherBoundingBox = new Rectangle((int)(_launcher.Position.X - 100), (int)(_launcher.Position.Y - 100), 200, 100);
      for (int i = 0; i < rects.Count; i++) {
        for (int j = i + 1; j < rects.Count; j++) {
          if (rects[i].Intersects(rects[j]))
            return true;
        }
        if (rects[i].Intersects(launcherBoundingBox))
          return true;
      }



      //not accurate but it works for now.
      Rectangle circle;
      foreach (DeathTrap deathTrap in DeathTraps) {
        circle = new Rectangle((int)deathTrap.Origin.X, (int)deathTrap.Origin.Y, deathTrap.Width, deathTrap.Height);
        foreach (Rectangle rect in rects) {
          if (circle.Intersects(rect)) {
            return true;
          }
        }
      }

      return false;
    }

  }
}