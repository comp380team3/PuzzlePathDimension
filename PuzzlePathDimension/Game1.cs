using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PuzzlePathDimension {
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game {
    /// <summary>
    /// The size of one square on the level grid.
    /// </summary>
    public static readonly float GridSize = 20f;

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    // The ball for our game
    Ball ball;
    // The platforms for the game
    Platform platform1;
    Platform platform2;
    // The goal for the game
    Goal goal;
    // The launcher for the game
    Launcher launcher;

    /// <summary>
    /// Contains all loaded assets.
    /// </summary>
    private Dictionary<string, Texture2D> _graphicContent;

    /// <summary>
    /// Whether the game has started.
    /// </summary>
    private bool Started;

    /// <summary>
    /// Creates a Game1 object.
    /// </summary>
    public Game1() {
      graphics = new GraphicsDeviceManager(this);

      Started = false;

      // Set the resolution to 800x600
      graphics.PreferredBackBufferWidth = 800;
      graphics.PreferredBackBufferHeight = 600;
      graphics.ApplyChanges();

      // Make the mouse visible
      this.IsMouseVisible = true;

      // Tells the game where the content directory is
      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize() {
      // TODO: Add your initialization logic here

      // Create a new ball
      // ball = new Ball();

      // Create a new platform
      // platform1 = new Platform();
      // platform2 = new Platform();

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      // Add all the graphic assets to the dictionary.
      _graphicContent = new Dictionary<string, Texture2D>();
      _graphicContent.Add("ball", Content.Load<Texture2D>("ball_new"));
      _graphicContent.Add("platform", Content.Load<Texture2D>("platform_new"));
      _graphicContent.Add("goal", Content.Load<Texture2D>("goal"));
      _graphicContent.Add("launcher", Content.Load<Texture2D>("launcher"));

      // TODO: use this.Content to load your game content here

      // Find a safe position to place the ball
      // Vector2 ballPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
      //    GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
      // Create a new ball 
      // ball.Initialize(GraphicsDevice.Viewport, Content.Load<Texture2D>("Ball"), ballPosition);

      // Find a safe position to place the platform
      // Vector2 platform1Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 5,
      //    GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

      // Create a new platform
      // platform1.Initialize(Content.Load<Texture2D>("platform"), platform1Position);

      // Find a safe position to place the platform
      // Vector2 platform2Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 1.5f,
      //    GraphicsDevice.Viewport.TitleSafeArea.Height / 5);

      // Create a new platform
      // platform2.Initialize(Content.Load<Texture2D>("platform"), platform2Position);

    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent() {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      if (!Started) { // TODO: remove this test level
        SetupTestLevel();
      }

      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
          Keyboard.GetState().IsKeyDown(Keys.Escape)) {
        this.Exit();
      }
      // TODO: remove this test code
      else if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
        launcher.LaunchBall();
      } else if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
        launcher.AdjustAngle((float)Math.PI / 64);
      } else if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
        launcher.AdjustAngle((float)-Math.PI / 64);
      } else if (Keyboard.GetState().IsKeyDown(Keys.F)) {
        Console.WriteLine(launcher);
      } else if (Keyboard.GetState().IsKeyDown(Keys.G)) {
        Console.WriteLine(ball);
      } else if (Keyboard.GetState().IsKeyDown(Keys.R)) {
        if (!launcher.Active) { // Some crude restart mechanism
          ball.Stop();
          launcher.LoadBall(ball);
        }
      }

      // TODO: Add your update logic here
      // Update the launcher's state
      launcher.Update();
      // Update the ball's position
      ball.Update();

      // Update the collision
      UpdateCollision();

      base.Update(gameTime);
    }

    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    private void SetupTestLevel() {
      Started = true;

      // Adds a launcher to the level
      launcher = new Launcher();
      Vector2 launchPos = new Vector2(10 * GridSize, 29 * GridSize);
      launcher.Initialize(_graphicContent["launcher"], launchPos);

      // Adds a ball to the level
      ball = new Ball();
      Vector2 ballPos = new Vector2(400f, 300f);
      ball.Initialize(GraphicsDevice.Viewport, _graphicContent["ball"], ballPos);
      // Load the ball into the launcher
      launcher.LoadBall(ball);

      // Adds a platform to the level
      platform1 = new Platform();
      Vector2 platformPos = new Vector2(5 * GridSize, 5 * GridSize);
      Vector2 platformLen = new Vector2(20 * GridSize, 2 * GridSize);
      platform1.Initialize(_graphicContent["platform"], platformPos, platformLen);

      // ...and another one.
      platform2 = new Platform();
      platformPos = new Vector2(20 * GridSize, 20 * GridSize);
      platformLen = new Vector2(10 * GridSize, 8 * GridSize);
      platform2.Initialize(_graphicContent["platform"], platformPos, platformLen);

      // Adds a goal to the level
      goal = new Goal();
      Vector2 goalPos = new Vector2(10 * GridSize, 1 * GridSize);
      goal.Initialize(_graphicContent["goal"], goalPos);
    }

    private void UpdateCollision() {
      // Use the Rectangle's built-in intersect function to
      // determine if two objects collide
      Rectangle ballRectangle;
      Rectangle platformRectangle;

      ballRectangle = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, ball.Width, ball.Height);

      platformRectangle = new Rectangle((int)platform1.Position.X, (int)platform1.Position.Y, platform1.Width, platform1.Height);

      if (ballRectangle.Intersects(platformRectangle)) {
        if ((ballRectangle.Bottom >= platformRectangle.Top) && ball.YVelocity < 0) {
          ball.FlipYDirection();
        } else if ((ballRectangle.Top <= platformRectangle.Bottom && ball.YVelocity > 0)) {
          ball.FlipYDirection();
        } else if ((ballRectangle.Right) >= (platformRectangle.Left)) {
          ball.FlipXDirection();
        } else if (ballRectangle.Left <= (platformRectangle.Right)) {
          ball.FlipXDirection();
        }
      }

      platformRectangle = new Rectangle((int)platform2.Position.X, (int)platform2.Position.Y, platform2.Width, platform2.Height);

      if (ballRectangle.Intersects(platformRectangle)) {
        if ((ballRectangle.Bottom >= platformRectangle.Top) && ball.YVelocity < 0) {
          ball.FlipYDirection();
        } else if ((ballRectangle.Top <= platformRectangle.Bottom && ball.YVelocity > 0)) {
          ball.FlipYDirection();
        } else if ((ballRectangle.Right) >= (platformRectangle.Left)) {
          ball.FlipXDirection();
        } else if (ballRectangle.Left <= (platformRectangle.Right)) {
          ball.FlipXDirection();
        }
      }

    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.White);

      // TODO: Add your drawing code here
      spriteBatch.Begin();

      // Draw the goal on the canvas
      goal.Draw(spriteBatch);

      // Draw the platform on the canvas
      platform1.Draw(spriteBatch);
      platform2.Draw(spriteBatch);

      // Draw the ball onto the canvas
      ball.Draw(spriteBatch);

      // Draw the launcher on the canvas
      launcher.Draw(spriteBatch);

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
