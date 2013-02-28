using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class Ball {
    /// <summary>
    /// The texture that the ball uses.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// The position of the ball.
    /// </summary>
    private Vector2 _position;

    /// <summary>
    /// The Viewport object that the ball will be drawn to.
    /// </summary>
    private Viewport _viewport;

    /// <summary>
    /// Whether the ball is active.
    /// </summary>
    private bool _active;

    /// <summary>
    /// The ball's horizontal speed.
    /// </summary>
    private float _xVelocity;

    /// <summary>
    /// The ball's vertical speed.
    /// </summary>
    private float _yVelocity;

    /// <summary>
    /// Gets the position of the ball.
    /// </summary>
    public Vector2 Position {
      get { return _position; }
    }

    /// <summary>
    /// Gets the height of the ball.
    /// </summary>
    public int Height {
      get { return _texture.Height; }
    }

    /// <summary>
    /// Gets the width of the ball.
    /// </summary>
    public int Width {
      get { return _texture.Width; }
    }

    /// <summary>
    /// Gets the ball's horizontal speed.
    /// </summary>
    public float XVelocity {
      get { return _xVelocity; }
    }

    /// <summary>
    /// Gets the ball's vertical speed.
    /// </summary>
    public float YVelocity {
      get { return _yVelocity; }
    }

    /// <summary>
    /// Gets whether the ball is active.
    /// </summary>
    public bool Active {
      get { return _active; }
    }

    /// <summary>
    /// Initializes a ball.
    /// </summary>
    /// <param name="viewport">The screen that the ball will be drawn on.</param>
    /// <param name="texture">The texture that the ball will be drawn with.</param>
    /// <param name="position">The initial position of the ball.</param>
    public void Initialize(Viewport viewport, Texture2D texture, Vector2 position) {
      // TODO: add exceptions

      // Set the texture of the ball
      _texture = texture;

      // Set the position of the ball
      _position = position;

      // Ball will be stationary at first
      _active = false;

      _viewport = viewport;

      // Ball's velocity
      _xVelocity = 0f;
      _yVelocity = 0f;
    }

    /// <summary>
    /// Updates the ball's state.
    /// </summary>
    public void Update() {
      _position.X = Position.X + _xVelocity;
      _position.Y = Position.Y + _yVelocity;

      // Check if the ball is heading off the screen
      if ((Position.X + _texture.Width / 2) > _viewport.Width) {
        _xVelocity = -1 * _xVelocity;
      } else if (Position.X <= 0) {
        _xVelocity = -1 * _xVelocity;
      }

      if ((Position.Y + _texture.Height / 2) > _viewport.Height) {
        _yVelocity = -1 * _yVelocity;
      } else if (Position.Y <= 0) {
        _yVelocity = -1 * _yVelocity;
      }

      // TODO: add friction
    }

    /// <summary>
    /// Draws the ball to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Flips the ball's velocity on the X axis.
    /// </summary>
    public void FlipXDirection() {
      _xVelocity = -1 * _xVelocity;
    }

    /// <summary>
    /// Flips the ball's velocity on the Y axis.
    /// </summary>
    public void FlipYDirection() {
      _yVelocity = -1 * _yVelocity;
    }

    /// <summary>
    /// Launches the ball.
    /// </summary>
    public void LaunchBall() {
      _active = true;

      _xVelocity = 5f;
      _yVelocity = 5f;
    }
  }
}
