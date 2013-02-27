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

namespace PuzzlePathDimensionSampleDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // The ball for our game
        Ball ball;
        // The platforms for the game
        Platform platform1;
        Platform platform2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            // Sets the resolution to 800x600
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            // Tells the game where the Content directory is
            Content.RootDirectory = "Content";

            // Makes the mouse cursor visible.
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Create a new ball
            ball = new Ball();

            // Create a new platform
            platform1 = new Platform();
            platform2 = new Platform();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Find a safe position to place the ball
            Vector2 ballPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, 
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            // Create a new ball 
            ball.Initialize(GraphicsDevice.Viewport, Content.Load<Texture2D>("Ball"), ballPosition);

            // Find a safe position to place the platform
            Vector2 platform1Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 5,
                GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

            // Create a new platform
            platform1.Initialize(Content.Load<Texture2D>("platform"), platform1Position);

            // Find a safe position to place the platform
            Vector2 platform2Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 1.5f,
                GraphicsDevice.Viewport.TitleSafeArea.Height / 5);

            // Create a new platform
            platform2.Initialize(Content.Load<Texture2D>("platform"), platform2Position);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit (for the Xbox 360 controller only)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            // TODO: Add your update logic here
            // Update the balls position
            ball.Update();
            
            // Update the collision
            UpdateCollision();

            base.Update(gameTime);
        }

        private void UpdateCollision()
        {
            // Use the Rectangle's built-in intersect function to
            // determine if two objects collide
            Rectangle ballRectangle;
            Rectangle platformRectangle;

            ballRectangle = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, ball.Width, ball.Height);

            platformRectangle = new Rectangle((int)platform1.Position.X, (int)platform1.Position.Y, platform1.Width, platform1.Height);

            if (ballRectangle.Intersects(platformRectangle))
            {
                if ((ballRectangle.Bottom >= platformRectangle.Top) && ball.ballYVelocity < 0)
                {
                    ball.flipYDirection();
                }
                else if ((ballRectangle.Top <= platformRectangle.Bottom && ball.ballYVelocity > 0))
                {
                    ball.flipYDirection();
                }
                else if ((ballRectangle.Right) <= (platformRectangle.Left))
                {
                    ball.flipXDirection();
                }
                else if (ballRectangle.Left >= (platformRectangle.Right))
                {
                    ball.flipXDirection();
                }
            }

            platformRectangle = new Rectangle((int)platform2.Position.X, (int)platform2.Position.Y, platform2.Width, platform2.Height);

            if (ballRectangle.Intersects(platformRectangle))
            {
                if ((ballRectangle.Bottom >= platformRectangle.Top) && ball.ballYVelocity < 0)
                {
                    ball.flipYDirection();
                }
                else if ((ballRectangle.Top <= platformRectangle.Bottom && ball.ballYVelocity > 0))
                {
                    ball.flipYDirection();
                }
                else if ((ballRectangle.Right) <= (platformRectangle.Left))
                {
                    ball.flipXDirection();
                }
                else if (ballRectangle.Left >= (platformRectangle.Right))
                {
                    ball.flipXDirection();
                }
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // Draw the platform on the canvas
            platform1.Draw(spriteBatch);
            platform2.Draw(spriteBatch);

            // Draw the ball onto the canvas
            ball.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
