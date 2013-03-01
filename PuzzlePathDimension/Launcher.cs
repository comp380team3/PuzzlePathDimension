using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Launcher class represents the level's launcher.
  /// </summary>
  class Launcher {
    /// <summary>
    /// The texture that the launcher will be drawn with.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// The position of the launcher.
    /// </summary>
    private Vector2 _position;

    /// <summary>
    /// The position of the tip of the launcher.
    /// </summary>
    private Vector2 _tip;

    /// <summary>
    /// Whether the launcher is active and movable.
    /// </summary>
    private bool _active;

    /// <summary>
    /// The ball that the launcher is holding.
    /// </summary>
    private Ball _ball;

    /// <summary>
    /// The angle that the launcher is pointing at.
    /// </summary>
    private float _angle;

    /// <summary>
    /// Gets the position of the launcher.
    /// </summary>
    public Vector2 Position {
      get { return _position; }
    }

    /// <summary>
    /// Gets whether the launcher is active and movable.
    /// </summary>
    public bool Active {
      get { return _active; }
    }

    /// <summary>
    /// Gets the angle that the launcher is pointing at.
    /// </summary>
    public float Angle {
      get { return _angle; }
    }

    /// <summary>
    /// Initializes the launcher.
    /// </summary>
    /// <param name="texture">The texture that the launcher will be drawn with.</param>
    /// <param name="position">The position of the launcher.</param>
    public void Initialize(Texture2D texture, Vector2 position) {
      _texture = texture;
      _position = position;
      _active = false;
      _ball = null;

      // Initialize the angle and calculate the initial position of the tip.
      _angle = (float)Math.PI / 4; // 45 degrees
      _tip = new Vector2();
      CalculateNewTip();
    }

    /// <summary>
    /// Updates the launcher's state.
    /// </summary>
    public void Update() {
      // Move the ball with the launcher's tip if a ball is going to be fired.
      if (_ball != null) {
        // TODO: Use the ball's center instead of its upper-left corner
        _ball.Position = new Vector2(_tip.X, _tip.Y);
      }
    }

    /// <summary>
    /// Adjusts the launcher by a given angle. Only works if the launcher is active.
    /// </summary>
    /// <param name="delta">The value to adjust the angle by.</param>
    public void AdjustAngle(float delta) {
      // We don't want the launcher moving when the ball has already been launched.
      if (!_active) {
        return;
      }

      // Adjust the angle
      _angle += delta;

      // Bounds checking
      // TODO: prevent shooting the ball off the map if the launcher is placed in a corner
      if (_angle < 0f) {
        _angle = 0f;
      } else if (_angle > Math.PI) { // 180 degrees
        _angle = (float)Math.PI;
      }

      // Calculate the new position of the launcher's tip.
      CalculateNewTip();
    }

    /// <summary>
    /// Loads a ball into the launcher.
    /// </summary>
    /// <param name="ball">The ball that will be eventually launched.</param>
    public void LoadBall(Ball newBall) {
      if (_ball != null) {
        throw new InvalidOperationException("There is already a ball that is ready to be launched.");
      }

      // The launcher now contains the ball, and the user can now aim it.
      _ball = newBall;
      _active = true;
    }

    /// <summary>
    /// Launches the ball in the launcher.
    /// </summary>
    public void LaunchBall() {
      // Don't do anything if no ball is being launched.
      if (!_active) {
        return;
      }

      // Stop the player from moving the launcher.
      _active = false;

      // Calculate the velocity of the ball based on the launcher's angle.
      float xVelocity = 10f * (float) Math.Cos(-1 * _angle);
      float yVelocity = 10f * (float) Math.Sin(-1 * _angle);
      _ball.Launch(xVelocity, yVelocity);

      // The launcher no longer owns the ball.
      _ball = null;
    }

    /// <summary>
    /// Calculates the new position of the tip of the launcher.
    /// </summary>
    private void CalculateNewTip() {
      /* There are parts here that may seem a bit confusing at first, especially if you
       * forgot your trigonometry like I did :p
       * 
       * First, the unit circle has a radius of 1. You can think of a launcher's
       * arc as a half-circle with a radius of 80, which is the texture's width.
       * The relevant formulas are:
       * - x = rcos(theta)
       * - y = rsin(theta)
       * so that is why the texture's width is still used for calculating the y-coordinate 
       * instead of the height.
       * 
       * But what is the center of the circle? That question is why the launcher's position
       * is added to both the X and Y components of the tip. Think of it as an offset.
       * The launcher's position, however, is technically the upper-left corner of the launcher.
       * This is not a problem on the x-axis, but it makes the y-coordinate inaccurate. Adding half
       * of the launcher's height to the y-coordinate solves that. Subtracting is wrong since
       * the y-axis is inverted, which means that 0 is the top of the screen and 600 is the bottom.
       * 
       * The flipped y-axis is also the reason why the angle passed to Math.Sin() is multiplied by -1.
       * 
       * I think that helps you and my future self figure out what's going on here :p 
       * After all, who knows if we have to change this later? -Jorenz
       */
      _tip.X = (float)(_texture.Width * Math.Cos(_angle) + _position.X);
      _tip.Y = (float)(_texture.Width * Math.Sin(-1 * _angle) + _position.Y + _texture.Height / 2.0);
    }

    /// <summary>
    /// Draws the launcher to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when launching the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      // The unit circle goes counter-clockwise, but the rotation parameter goes clockwise, so flip it.
      float rotateAngle = -1 * _angle;

      // Rotate at the midpoint of the upper-left and lower-left corners, for better precision
      Vector2 rotatePos = new Vector2(0f, _texture.Height / 2);

      // Draw the launcher!
      spriteBatch.Draw(_texture, _position, null, Color.White, rotateAngle, rotatePos, 1f, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Returns a string representation of the Launcher object.
    /// </summary>
    /// <returns>Information about the launcher.</returns>
    public override string ToString() {
      return "Tip position: " + _tip.X + ", " + _tip.Y + " | " 
        + "Angle: " + (_angle * 180 / Math.PI) + ", in radians: " + _angle;
    }
  }
}
