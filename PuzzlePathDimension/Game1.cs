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

namespace PuzzlePathDimensionSampleDemo {
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    // The ball for our game
    Ball ball;
    // The platforms for the game
    Platform [] platforms;
    Platform platform1;
    Platform platform2;

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
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
      ball = new Ball();

      // Create a new platform
      //platform1 = new Platform();
      //platform2 = new Platform();

      platforms = new Platform[2];
      for (int i = 0; i < platforms.Length; i++)
        platforms[i] = new Platform();
      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      // TODO: use this.Content to load your game content here

      // Find a safe position to place the ball
      Vector2 ballPosition = new Vector2(100, 300);
      // Create a new ball 
      ball.Initialize(GraphicsDevice.Viewport, Content.Load<Texture2D>("Ball"), ballPosition);

      // Find a safe position to place the platform
      Vector2 platform1Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 5,
          GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

      // Create a new platform
      platforms[0].Initialize(Content.Load<Texture2D>("platform"), platform1Position);

      // Find a safe position to place the platform
      Vector2 platform2Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 1.5f,
          GraphicsDevice.Viewport.TitleSafeArea.Height / 5);

      // Create a new platform
      platforms[1].Initialize(Content.Load<Texture2D>("platform"), platform2Position);

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
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      // TODO: Add your update logic here
      // Update the balls position
      ball.Update();

      // Update the collision
      UpdateCollision();

      base.Update(gameTime);
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
            if(y==top || y==bottom-1)
              ball.flipYDirection();
            if (x == left || x == right-1)
              ball.flipXDirection();
            // then an intersection has been found
            return true;
          }
        }
      }

      // No intersection found
      return false;
    }


    private void UpdateCollision() {
      // Use the Rectangle's built-in intersect function to
      // determine if two objects collide
      Rectangle ballRectangle;
      Rectangle platformRectangle;

      ballRectangle = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, ball.Width, ball.Height);



      for (int i = 0; i < platforms.Length; i++) {
        platformRectangle = new Rectangle((int)platforms[i].Position.X, (int)platforms[i].Position.Y, platforms[i].Width, platforms[i].Height);
        Boolean intersect = IntersectPixels(ballRectangle, ball.ballTextureData, platformRectangle, platforms[i].platformTextureData);
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

      // Draw the platform on the canvas
      for (int i = 0; i < platforms.Length; i++)
        platforms[i].Draw(spriteBatch);

      // Draw the ball onto the canvas
      ball.Draw(spriteBatch);

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
