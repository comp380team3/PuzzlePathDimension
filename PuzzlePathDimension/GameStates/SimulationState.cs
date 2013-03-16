using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  class Simulation {
    public Ball Ball { get; set; }
    public List<Platform> Platforms { get; set; }
    public Goal Goal { get; set; }
    public Launcher Launcher { get; set; }

    public Texture2D Background { get; set; }
  }

  interface View<T> {
    T BackingModel { get; }
  }

  class SimulationView : View<Simulation> {
    private SpriteBatch _spriteBatch = null;

    public Simulation BackingModel { get; private set; }

    public SimulationView(Simulation simulation, SpriteBatch spriteBatch) {
      this._spriteBatch = spriteBatch;
      this.BackingModel = simulation;
    }

    public void Draw() {
      _spriteBatch.Draw(BackingModel.Background, Vector2.Zero, Color.White);

      // Draw the goal on the canvas
      BackingModel.Goal.Draw(_spriteBatch);

      // Draw the platform on the canvas
      foreach (Platform platform in BackingModel.Platforms) {
        platform.Draw(_spriteBatch);
      }

      // Draw the ball onto the canvas
      BackingModel.Ball.Draw(_spriteBatch);

      // Draw the launcher on the canvas
      BackingModel.Launcher.Draw(_spriteBatch);
    }
  }

  class SimulationState : GameState {
    /// <summary>
    /// Contains all loaded assets.
    /// </summary>
    Dictionary<string, Texture2D> _graphicContent = new Dictionary<string, Texture2D>();

    Game1 game1;
    SpriteBatch spriteBatch;

    Simulation _simulation;
    SimulationView _view;

    public SimulationState(Game1 game1, SpriteBatch spriteBatch) {
      this.game1 = game1;
      this.spriteBatch = spriteBatch;

      LoadContent(game1.Content);

      this._simulation = SetupTestLevel(game1.GraphicsDevice.Viewport);
      this._view = new SimulationView(this._simulation, spriteBatch);
    }

    public void LoadContent(ContentManager theContent) {
      // Add all the graphic assets to the dictionary.
      _graphicContent.Add("ball", theContent.Load<Texture2D>("ball_new"));
      _graphicContent.Add("platform", theContent.Load<Texture2D>("platform_new"));
      _graphicContent.Add("goal", theContent.Load<Texture2D>("goal"));
      _graphicContent.Add("launcher", theContent.Load<Texture2D>("launcher"));
    }


    //Update all of the elements that need updating in the Title Screen        
    public void Update(GameTime theTime) {
      Launcher launcher = _simulation.Launcher;
      Ball ball = _simulation.Ball;

      // Route user input to the approproate action
      if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
        launcher.LaunchBall();
      } else if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
        launcher.AdjustAngle((float)Math.PI / 64);
      } else if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
        launcher.AdjustAngle((float)-Math.PI / 64);
      }

      // TODO: remove this test code
      if (Keyboard.GetState().IsKeyDown(Keys.F)) {
        Console.WriteLine(launcher);
      } else if (Keyboard.GetState().IsKeyDown(Keys.G)) {
        Console.WriteLine(ball);
      } else if (Keyboard.GetState().IsKeyDown(Keys.R)) {
        if (!launcher.Active) { // Some crude restart mechanism
          ball.Stop();
          launcher.LoadBall(ball);
        }
      }

      MouseState mouse = Mouse.GetState();
      if (mouse.LeftButton == ButtonState.Pressed) {
        Console.WriteLine("Mouse click at: " + mouse.X + ", " + mouse.Y);
      }

      //Check to see if the Player one controller has pressed the "B" button, if so, then
      //call the screen event associated with this screen
      if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.B) == true) {
        game1.PopState();
      }

      // Update the launcher's state
      launcher.Update();

      // Update the balls position
      ball.Update();

      // Update the collision
      UpdateCollision();
    }

    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    private Simulation SetupTestLevel(Viewport viewport) {
      Simulation simulation = new Simulation();

      simulation.Background = game1.Content.Load<Texture2D>("GameScreen");

      // Adds a launcher to the level
      Launcher launcher = new Launcher();
      Vector2 launchPos = new Vector2(34 * Game1.GridSize, 29 * Game1.GridSize);
      launcher.Initialize(_graphicContent["launcher"], launchPos);
      simulation.Launcher = launcher;

      // Adds a ball to the level
      Ball ball = new Ball();
      Vector2 ballPos = new Vector2(400f, 300f);
      ball.Initialize(viewport, _graphicContent["ball"], ballPos);
      simulation.Ball = ball;

      // Load the ball into the launcher
      launcher.LoadBall(ball);

      List<Platform> platforms = new List<Platform>();
      simulation.Platforms = platforms;

      // Adds a platform to the level
      Platform platform0 = new Platform();
      Vector2 platformPos = new Vector2(5 * Game1.GridSize, 5 * Game1.GridSize);
      Vector2 platformLen = new Vector2(20 * Game1.GridSize, 2 * Game1.GridSize);
      platform0.Initialize(_graphicContent["platform"], platformPos, platformLen);
      platforms.Add(platform0);

      // ...and another one.
      Platform platform1 = new Platform();
      platformPos = new Vector2(20 * Game1.GridSize, 20 * Game1.GridSize);
      platformLen = new Vector2(10 * Game1.GridSize, 8 * Game1.GridSize);
      platform1.Initialize(_graphicContent["platform"], platformPos, platformLen);
      platforms.Add(platform1);

      // Adds a goal to the level
      Goal goal = new Goal();
      Vector2 goalPos = new Vector2(10 * Game1.GridSize, 1 * Game1.GridSize);
      goal.Initialize(_graphicContent["goal"], goalPos);
      simulation.Goal = goal;

      return simulation;
    }

    private bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB) {
      Ball ball = _simulation.Ball;

      // Check if the two objects are near each other. 
      // If they are not then return false for no intersection.
      if (!rectangleA.Intersects(rectangleB)) {
        return false;
      }

      // Find the bounds of the rectangle intersection
      int top = Math.Max(rectangleA.Top, rectangleB.Top);
      int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
      int left = Math.Max(rectangleA.Left, rectangleB.Left);
      int right = Math.Min(rectangleA.Right, rectangleB.Right);

      // Check every point within the intersection bounds
      for (int y = top; y < bottom; y++) {
        for (int x = left; x < right; x++) {
          // Get the color of both pixels at this point
          //Console.WriteLine("Index to fetch: " + (((x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width) % 400));
          Console.WriteLine("Length of dataB: " + dataB.Length);
          Color colorA = dataA[((x - rectangleA.Left) +
                               (y - rectangleA.Top) * rectangleA.Width) % dataA.Length];
          Color colorB = dataB[((x - rectangleB.Left) +
                               (y - rectangleB.Top) * rectangleB.Width) % dataB.Length]; // lol maybe?

          // If both pixels are not completely transparent,
          if (colorA.A != 0 && colorB.A != 0) {
            if (y == top || y == bottom - 1)
              ball.FlipYDirection();
            if (x == left || x == right - 1)
              ball.FlipXDirection();
            // then an intersection has been found
            return true;
          }
        }
      }

      // No intersection found
      return false;
    }

    private void UpdateCollision() {
      Ball ball = _simulation.Ball;

      Rectangle ballRectangle = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, ball.Width, ball.Height);

      foreach (Platform platform in _simulation.Platforms) {
        Rectangle platformRectangle = new Rectangle(
            (int)platform.Position.X,
            (int)platform.Position.Y,
            platform.Width,
            platform.Height);

        IntersectPixels(ballRectangle, ball.GetColorData(), platformRectangle, platform.GetColorData());
      }
    }

    public void Draw(SpriteBatch theBatch) {
      _view.Draw();
    }
  }
}
