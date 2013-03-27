using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class Ball {
    /// <summary>
    /// The hard-coded height of the ball, in pixels.
    /// </summary>
    private const int _height = 25;

    /// <summary>
    /// The hard-coded width of the ball, in pixels.
    /// </summary>
    private const int _width = 25;

    /// <summary>
    /// The texture that the ball will be drawn with.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// The texture's color data.
    /// </summary>
    private Color[] _colorData;

    /// <summary>
    /// The position of the ball.
    /// </summary>
    private Vector2 _position;

    /// <summary>
    /// The velocity of the ball.
    /// </summary>
    private Vector2 _velocity;

    /// <summary>
    /// The Viewport object that the ball will be drawn to.
    /// </summary>
    private Viewport _viewport;

    /// <summary>
    /// Whether the ball is active.
    /// </summary>
    private bool _active;

    /// <summary>
    /// Gets the texture that the ball will be drawn with.
    /// </summary>
    public Texture2D Texture {
      get { return _texture; }
    }

    /// <summary>
    /// Gets or sets the position of the ball.
    /// </summary>
    public Vector2 Position {
      get { return _position; }
      set { _position = value; }
    }

    public Vector2 Velocity {
      get { return _velocity; }
      set { _velocity = value; }
    }

    /// <summary>
    /// Gets the height of the ball.
    /// </summary>
    public int Height {
      get { return _height; }
    }

    /// <summary>
    /// Gets the width of the ball.
    /// </summary>
    public int Width {
      get { return _width; }
    }

    /// <summary>
    /// Gets the ball's horizontal speed.
    /// </summary>
    public float XVelocity {
      get { return _velocity.X; }
      set { _velocity.X = value; }
    }

    /// <summary>
    /// Gets the ball's vertical speed.
    /// </summary>
    public float YVelocity {
      get { return _velocity.Y; }
      set { _velocity.Y = value; }
    }

    /// <summary>
    /// Gets whether the ball is active.
    /// </summary>
    public bool Active {
      get { return _active; }
    }

    /// <summary>
    /// Gets the texture's color data.
    /// </summary>
    /// <returns>The texture's color data as an array.</returns>
    public Color[] GetColorData() {
      // See http://msdn.microsoft.com/en-us/library/0fss9skc.aspx for why
      // this is not a property.
      return (Color[])_colorData.Clone();
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

      // Get the texture's color data, which is used for per-pixel collision
      _colorData = new Color[_texture.Width * _texture.Height];
      _texture.GetData<Color>(_colorData);

      // Check to make sure that the visual representation of the ball is actually the right
      // size, and print a warning to the console if that isn't the case.
      if (_texture != null &&_texture.Width * _texture.Height != _width * _height) {
        Console.WriteLine("Warning: the ball's texture does not have the expected dimensions.");
        Console.WriteLine("Expected: " + _width + ", " + _height);
        Console.WriteLine("...but the texture is: " + _texture.Width + ", " + _texture.Height);
      }

      // Set the position of the ball
      _position = position;

      // Set the initial velocity of the ball
      _velocity = Vector2.Zero;

      // Ball will be stationary at first
      _active = false;

      _viewport = viewport;
    }

    /// <summary>
    /// Updates the ball's state.
    /// </summary>
    public void Update() {
      Vector2 destination = new Vector2(Position.X, Position.Y);
      destination.X += XVelocity;
      destination.Y += YVelocity;
      Position = destination;

      // Check if the ball is heading off the screen
      if (Position.X + _texture.Width / 2 > _viewport.Width) {
        XVelocity = -1 * XVelocity;
      } else if (Position.X <= 0) {
        XVelocity = -1 * XVelocity;
      }

      if (Position.Y + _texture.Height / 2 > _viewport.Height) {
        YVelocity = -1 * YVelocity;
      } else if (Position.Y <= 0) {
        YVelocity = -1 * YVelocity;
      }

      // TODO: make this friction better
      /*if (Math.Abs(_xVelocity) < 0.01f) {
        _xVelocity = 0;
      }
      if (Math.Abs(_yVelocity) < 0.01f) {
        _yVelocity = 0;
      }
      _xVelocity *= 0.998f;
      _yVelocity *= 0.998f;*/
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
      XVelocity = -1 * XVelocity;
    }

    /// <summary>
    /// Flips the ball's velocity on the Y axis.
    /// </summary>
    public void FlipYDirection() {
      YVelocity = -1 * YVelocity;
    }

    /// <summary>
    /// Launches the ball.
    /// </summary>
    public void Launch(float x, float y) {
      _active = true;

      XVelocity = x;
      YVelocity = y;
    }

    /// <summary>
    /// Stops the ball.
    /// </summary>
    public void Stop() {
      _active = false;

      XVelocity = 0f;
      YVelocity = 0f;
    }

    /// <summary>
    /// Returns a string representation of the Ball object.
    /// </summary>
    /// <returns>Information about the ball.</returns>
    public override string ToString() {
      return "Position: " + Position.X + ", " + Position.Y + " | " +
        "Velocity: " + XVelocity + ", " + YVelocity;
    }
  }
}
