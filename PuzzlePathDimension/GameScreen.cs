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
    List<Platform> platforms;
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
      platforms = new List<Platform>();
      platforms.Add(new Platform());
      platforms.Add(new Platform());

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
      Platform platform0 = platforms[0];
      Vector2 platformPos = new Vector2(5 * Game1.GridSize, 5 * Game1.GridSize);
      Vector2 platformLen = new Vector2(20 * Game1.GridSize, 2 * Game1.GridSize);
      platform0.Initialize(_graphicContent["platform"], platformPos, platformLen);

      // ...and another one.
      Platform platform1 = platforms[1];
      platformPos = new Vector2(20 * Game1.GridSize, 20 * Game1.GridSize);
      platformLen = new Vector2(10 * Game1.GridSize, 8 * Game1.GridSize);
      platform1.Initialize(_graphicContent["platform"], platformPos, platformLen);

      // Adds a goal to the level
      goal = new Goal();
      Vector2 goalPos = new Vector2(10 * Game1.GridSize, 1 * Game1.GridSize);
      goal.Initialize(_graphicContent["goal"], goalPos);
    }

    private bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB) {
      // Check if the two objects are near each other. 
      // If they are not then return false for no intersection.
      if (rectangleA.Top > rectangleB.Bottom || rectangleB.Top > rectangleA.Bottom) {
        return false;
      }
      if (rectangleB.Left > rectangleA.Right || rectangleA.Left > rectangleB.Right) {
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
          Color colorA = dataA[(x - rectangleA.Left) +
                               (y - rectangleA.Top) * rectangleA.Width];
          Color colorB = dataB[(x - rectangleB.Left) +
                               (y - rectangleB.Top) * rectangleB.Width];

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
      Rectangle ballRectangle;
      Rectangle platformRectangle;

      ballRectangle = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, ball.Width, ball.Height);

      for (int i = 0; i < platforms.Count; i++) {
        platformRectangle = new Rectangle((int)platforms[i].Position.X, (int)platforms[i].Position.Y, platforms[i].Width, platforms[i].Height);
        Boolean intersect = IntersectPixels(ballRectangle, ball.GetColorData(), platformRectangle, platforms[i].GetColorData());
      }
    }

    public override void Draw(SpriteBatch theBatch) {
      theBatch.Draw(mGameScreenBackground, Vector2.Zero, Color.White);

      // Draw the goal on the canvas
      goal.Draw(theBatch);

      // Draw the platform on the canvas
      foreach (Platform platform in platforms) {
        platform.Draw(theBatch);
      }

      // Draw the ball onto the canvas
      ball.Draw(theBatch);

      // Draw the launcher on the canvas
      launcher.Draw(theBatch);

      base.Draw(theBatch);
    }
  }
}
