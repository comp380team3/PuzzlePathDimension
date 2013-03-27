using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace PuzzlePathDimension {
  class GameScreen : Screen {
    /// <summary>
    /// Contains all loaded assets.
    /// </summary>
    private Dictionary<string, Texture2D> _graphicContent;
    World world;
    public const float unitToPixel = 100.0f;
    public const float pixelToUnit = 1 / unitToPixel;



    //The ball for our game
    Ball ball;

    //The platform fo the game
    List<Platform> platforms;
    List<Platform> walls;
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
      world = new World(new Vector2(0, 9.8f));
      walls = new List<Platform>();
      // Create a new platform

      LoadContent(theViewport, theContent);
    }

    public void CreateBox(Viewport viewport, ContentManager theContent) {
      for (int i = 0; i < 4; i++) {
        if (i < 2) {
          Texture2D texture = theContent.Load<Texture2D>("TopBottom");
          Platform temp = new Platform(world, texture, new Vector2(texture.Width, texture.Height), 1, new Vector2(viewport.Width / 2f, viewport.Height * i));
          walls.Add(temp);
        } else {
          Texture2D texture = theContent.Load<Texture2D>("SideWall");
          Platform temp = new Platform(world, texture, new Vector2(texture.Width, texture.Height), 1, new Vector2(viewport.Width * (i - 2), viewport.Height / 2f));
          walls.Add(temp);
        }
      }
    }

    public void LoadContent(Viewport viewport, ContentManager theContent) {
      // Add all the graphic assets to the dictionary.
      _graphicContent = new Dictionary<string, Texture2D>();
      _graphicContent.Add("ball", theContent.Load<Texture2D>("ball_new"));
      _graphicContent.Add("platform", theContent.Load<Texture2D>("platform_new"));
      _graphicContent.Add("goal", theContent.Load<Texture2D>("goal"));
      _graphicContent.Add("launcher", theContent.Load<Texture2D>("launcher"));
      Texture2D texture = theContent.Load<Texture2D>("Ball");
      ball = new Ball(world, texture, new Vector2(texture.Width, texture.Height), 1);
      CreateBox(viewport, theContent);
      texture = theContent.Load<Texture2D>("platform");
      platforms = new List<Platform>();
      Platform platform = new Platform(world, texture, new Vector2(texture.Width, texture.Height), 1, new Vector2(400, 200));
      platforms.Add(platform);
      platforms.Add(new Platform(world, texture, new Vector2(texture.Width, texture.Height), 1, new Vector2(100, 500)));

      // Find a safe position to place the ball
      Vector2 ballPosition = new Vector2(viewport.TitleSafeArea.X + viewport.TitleSafeArea.Width / 2,
          viewport.TitleSafeArea.Y + viewport.TitleSafeArea.Height / 2);


      launcher = new Launcher();
      Vector2 launchPos = new Vector2(34 * Game1.GridSize, 29 * Game1.GridSize);
      launcher.Initialize(_graphicContent["launcher"], launchPos);
      goal = new Goal();
      Vector2 goalPos = new Vector2(10 * Game1.GridSize, 1 * Game1.GridSize);
      goal.Initialize(_graphicContent["goal"], goalPos);


      //SetupTestLevel(viewport);
    }


    //Update all of the elements that need updating in the Title Screen        
    public override void Update(GameTime theTime) {
      // TODO: remove this test code
      world.Step((float)theTime.ElapsedGameTime.TotalSeconds);

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



      //Check to see if the Player one controller has pressed the "B" button, if so, then
      //call the screen event associated with this screen
      if (GamePad.GetState(PlayerOne).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.B) == true) {
        ScreenEvent.Invoke(this, new EventArgs());
      }

      // Update the launcher's state
      launcher.Update();


      // Update the collision
      //UpdateCollision();

      base.Update(theTime);
    }

    /// <summary>
    /// Sets up a hard-coded level. This is for testing purposes.
    /// </summary>
    /// 
    public override void Draw(SpriteBatch theBatch) {
      theBatch.Draw(mGameScreenBackground, Vector2.Zero, Color.White);

      // Draw the goal on the canvas
      goal.Draw(theBatch);

      // Draw the platform on the canvas
      foreach (Platform platform in platforms) {
        platform.Draw(theBatch);
      }

      foreach (Platform wall in walls) {
        wall.Draw(theBatch);
      }

      // Draw the ball onto the canvas
      ball.Draw(theBatch);

      // Draw the launcher on the canvas
      launcher.Draw(theBatch);

      base.Draw(theBatch);
    }
  }
}
