using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class Ball {
    // Animation representing the ball
    public Texture2D BallTexture;

    // The position of the ball
    public Vector2 Position;

    // The size of the window where the ball will bounce
    public Viewport viewport;

    // The heigt of the ball
    public int Height {
      get { return BallTexture.Height; }
    }

    // The width of the ball
    public int Width {
      get { return BallTexture.Width; }
    }

    // The balls vertical speed
    public float ballXVelocity;

    // The balls horizontal speed
    public float ballYVelocity;

    // The ball is active
    public bool Active;

    public void Initialize(Viewport viewport, Texture2D texture, Vector2 position) {
      // Set the texture of the ball
      BallTexture = texture;

      // Set the position of the ball
      Position = position;

      // Ball will be active to move
      Active = true;

      this.viewport = viewport;

      // Ball's velocity
      ballXVelocity = 5f;
      ballYVelocity = 5f;
    }

    public void Update() {
      Position.X = Position.X + ballXVelocity;
      Position.Y = Position.Y + ballYVelocity;

      // Check if the ball is heading off the screen
      if ((Position.X + BallTexture.Width / 2) > viewport.Width) {
        ballXVelocity = -ballXVelocity;
      } else if ((Position.X) <= 0) {
        ballXVelocity = -ballXVelocity;
      }

      if ((Position.Y + BallTexture.Height / 2) > viewport.Height) {
        ballYVelocity = -ballYVelocity;
      } else if ((Position.Y) <= 0) {
        ballYVelocity = -ballYVelocity;
      }
    }

    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(BallTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }

    public void flipXDirection() {
      ballXVelocity = -ballXVelocity;
    }

    public void flipYDirection() {
      ballYVelocity = -ballYVelocity;
    }
  }
}
