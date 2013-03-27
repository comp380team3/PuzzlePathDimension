using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PuzzlePathDimension {
  public class SimulationState : GameState {
    Game1 game1;
    SpriteBatch spriteBatch;

    Simulation _simulation;
    SimulationView _view;

    public SimulationState(Game1 game1, SpriteBatch spriteBatch) {
      this.game1 = game1;
      this.spriteBatch = spriteBatch;

      this._simulation = game1.CreateTestLevel();
      this._view = new SimulationView(this._simulation, spriteBatch);
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

      // Update the ball's position
      ball.Update();

      // Update the collision
      UpdateCollision();
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
