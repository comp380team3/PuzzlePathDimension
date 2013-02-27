using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension
{
    class GameScreen : Screen
    {
        //The ball for our game
        Ball ball;
        //The platform fo the game
        Platform platform1;
        Platform platform2;

        
        //Background texture for the Title screen

        Texture2D mGameScreenBackground;

        public GameScreen(ContentManager theContent, Viewport viewport, EventHandler theScreenEvent): base(theScreenEvent)
        {
            //Load the background texture for the screen
            mGameScreenBackground = theContent.Load<Texture2D>("GameScreen");
            // Create a new ball
            ball = new Ball();

            // Create a new platform
            platform1 = new Platform();
            platform2 = new Platform();

            LoadContent(viewport, theContent);
        }

        public void LoadContent(Viewport viewport, ContentManager theContent)
        {
            // Find a safe position to place the ball
            Vector2 ballPosition = new Vector2(viewport.TitleSafeArea.X + viewport.TitleSafeArea.Width / 2,
                viewport.TitleSafeArea.Y + viewport.TitleSafeArea.Height / 2);
            // Create a new ball 
            ball.Initialize(viewport, theContent.Load<Texture2D>("Ball"), ballPosition);

            // Find a safe position to place the platform
            Vector2 platform1Position = new Vector2(viewport.TitleSafeArea.Width / 5,
                viewport.TitleSafeArea.Height / 2);

            // Create a new platform
            platform1.Initialize(theContent.Load<Texture2D>("platform"), platform1Position);

            // Find a safe position to place the platform
            Vector2 platform2Position = new Vector2(viewport.TitleSafeArea.Width / 1.5f,
                viewport.TitleSafeArea.Height / 5);

            // Create a new platform
            platform2.Initialize(theContent.Load<Texture2D>("platform"), platform2Position);
        }





        //Update all of the elements that need updating in the Title Screen        

        public override void Update(GameTime theTime)
        {
            //Check to see if the Player one controller has pressed the "B" button, if so, then

            //call the screen event associated with this screen

            if (GamePad.GetState(PlayerOne).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.B) == true)
            {
                ScreenEvent.Invoke(this, new EventArgs());
            }
            else
            {
                // Update the balls position
                ball.Update();

                // Update the collision
                UpdateCollision();
            }
            base.Update(theTime);
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
                if ((ballRectangle.Bottom >= platformRectangle.Top) && ball.ballYVelocity > 0)
                {
                    ball.flipYDirection();
                }
                else if ((ballRectangle.Top <= platformRectangle.Bottom && ball.ballYVelocity < 0))
                {
                    ball.flipYDirection();
                }
                else if ((ballRectangle.Right) >= (platformRectangle.Left) && ball.ballXVelocity > 0)
                {
                    ball.flipXDirection();
                }
                else if (ballRectangle.Left <= (platformRectangle.Right) && ball.ballXVelocity < 0)
                {
                    ball.flipXDirection();
                }
            }

            platformRectangle = new Rectangle((int)platform2.Position.X, (int)platform2.Position.Y, platform2.Width, platform2.Height);

            if (ballRectangle.Intersects(platformRectangle))
            {
                if ((ballRectangle.Bottom >= platformRectangle.Top) && ball.ballYVelocity > 0)
                {
                    ball.flipYDirection();
                }
                else if ((ballRectangle.Top <= platformRectangle.Bottom && ball.ballYVelocity < 0))
                {
                    ball.flipYDirection();
                }
                else if ((ballRectangle.Right) >= (platformRectangle.Left) && ball.ballXVelocity > 0)
                {
                    ball.flipXDirection();
                }
                else if (ballRectangle.Left <= (platformRectangle.Right) && ball.ballXVelocity < 0)
                {
                    ball.flipXDirection();
                }
            }

        }

        public override void Draw(SpriteBatch theBatch)
        {

            theBatch.Draw(mGameScreenBackground, Vector2.Zero, Color.White);

            // Draw the platform on the canvas
            platform1.Draw(theBatch);
            platform2.Draw(theBatch);

            // Draw the ball onto the canvas
            ball.Draw(theBatch);

            base.Draw(theBatch);

        }

    }
}
