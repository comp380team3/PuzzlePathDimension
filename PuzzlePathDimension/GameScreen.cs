using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  class GameScreen : Screen {
    /// <summary>
    /// Contains all loaded assets.
    /// </summary>
    private Dictionary<string, Texture2D> _graphicContent;

    //The ball for our game
    Ball ball;
    //The platform fo the game
    Platform platform1;
    Platform platform2;
    // The goal for the game
    Goal goal;
    // The launcher for the game
    Launcher launcher;

    //Background texture for the Title screen
    Texture2D mGameScreenBackground;
    
    public GameScreen(ContentManager theContent, Viewport theViewport, EventHandler theScreenEvent)
      : base(theScreenEvent) {
      //Load the background texture for the screen
      mGameScreenBackground = theContent.Load<Texture2D>("GameScreen");

      // Create a new ball
      ball = new Ball();

      // Create a new platform
      platform1 = new Platform();
      platform2 = new Platform();

      LoadContent(theViewport, theContent);
    }

    public void LoadContent(Viewport viewport, ContentManager theContent) {
      // Add all the graphic assets to the dictionary.
      _graphicContent = new Dictionary<string, Texture2D>();
      _graphicContent.Add("ball", theContent.Load<Texture2D>("ball_new"));
      _graphicContent.Add("platform", theContent.Load<Texture2D>("platform_new"));
      _graphicContent.Add("goal", theContent.Load<Texture2D>("goal"));
      _graphicContent.Add("launcher", theContent.Load<Texture2D>("launcher"));


      // Find a safe position to place the ball
      Vector2 ballPosition = new Vector2(viewport.TitleSafeArea.X + viewport.TitleSafeArea.Width / 2,
          viewport.TitleSafeArea.Y + viewport.TitleSafeArea.Height / 2);
      // Create a new ball 
      ball.Initialize(viewport, theContent.Load<Texture2D>("Ball"), ballPosition);

      SetupTestLevel(viewport);
    }


    //Update all of the elements that need updating in the Title Screen        
    public override void Update(GameTime theTime) {
      // TODO: remove this test code
      if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
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

      MouseState mouse = Mouse.GetState();
      if (mouse.LeftButton == ButtonState.Pressed) {
        Console.WriteLine("Mouse click at: " + mouse.X + ", " + mouse.Y);
      }

      //Check to see if the Player one controller has pressed the "B" button, if so, then
      //call the screen event associated with this screen
      if (GamePad.GetState(PlayerOne).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.B) == true) {
        ScreenEvent.Invoke(this, new EventArgs());
      }

      // Update the launcher's state
      launcher.Update();

      // Update the balls position
      ball.Update();

      // Update the collision
      UpdateCollision();

      base.Update(theTime);
    }

    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    private void SetupTestLevel(Viewport viewport) {
      // Adds a launcher to the level
      launcher = new Launcher();
      Vector2 launchPos = new Vector2(10 * Game1.GridSize, 29 * Game1.GridSize);
      launcher.Initialize(_graphicContent["launcher"], launchPos);

      // Adds a ball to the level
      ball = new Ball();
      Vector2 ballPos = new Vector2(400f, 300f);
      ball.Initialize(viewport, _graphicContent["ball"], ballPos);
      // Load the ball into the launcher
      launcher.LoadBall(ball);

      // Adds a platform to the level
      platform1 = new Platform();
      Vector2 platformPos = new Vector2(5 * Game1.GridSize, 5 * Game1.GridSize);
      Vector2 platformLen = new Vector2(20 * Game1.GridSize, 2 * Game1.GridSize);
      platform1.Initialize(_graphicContent["platform"], platformPos, platformLen);

      // ...and another one.
      platform2 = new Platform();
      platformPos = new Vector2(20 * Game1.GridSize, 20 * Game1.GridSize);
      platformLen = new Vector2(10 * Game1.GridSize, 8 * Game1.GridSize);
      platform2.Initialize(_graphicContent["platform"], platformPos, platformLen);

      // Adds a goal to the level
      goal = new Goal();
      Vector2 goalPos = new Vector2(10 * Game1.GridSize, 1 * Game1.GridSize);
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
        if ((ballRectangle.Bottom >= platformRectangle.Top) && ball.YVelocity > 0) {
          ball.FlipYDirection();
        } else if ((ballRectangle.Top <= platformRectangle.Bottom && ball.YVelocity < 0)) {
          ball.FlipYDirection();
        } else if ((ballRectangle.Right) >= (platformRectangle.Left) && ball.XVelocity > 0) {
          ball.FlipXDirection();
        } else if (ballRectangle.Left <= (platformRectangle.Right) && ball.XVelocity < 0) {
          ball.FlipXDirection();
        }
      }

      platformRectangle = new Rectangle((int)platform2.Position.X, (int)platform2.Position.Y, platform2.Width, platform2.Height);

      if (ballRectangle.Intersects(platformRectangle)) {
        if ((ballRectangle.Bottom >= platformRectangle.Top) && ball.YVelocity > 0) {
          ball.FlipYDirection();
        } else if ((ballRectangle.Top <= platformRectangle.Bottom && ball.YVelocity < 0)) {
          ball.FlipYDirection();
        } else if ((ballRectangle.Right) >= (platformRectangle.Left) && ball.XVelocity > 0) {
          ball.FlipXDirection();
        } else if (ballRectangle.Left <= (platformRectangle.Right) && ball.XVelocity < 0) {
          ball.FlipXDirection();
        }
      }
    }

    public override void Draw(SpriteBatch theBatch) {
      theBatch.Draw(mGameScreenBackground, Vector2.Zero, Color.White);

      // Draw the goal on the canvas
      goal.Draw(theBatch);

      // Draw the platform on the canvas
      platform1.Draw(theBatch);
      platform2.Draw(theBatch);

      // Draw the ball onto the canvas
      ball.Draw(theBatch);

      // Draw the launcher on the canvas
      launcher.Draw(theBatch);

      base.Draw(theBatch);
    }
  }
}
