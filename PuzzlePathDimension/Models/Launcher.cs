using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Launcher class represents the level's launcher.
  /// </summary>
  public class Launcher {
    /// <summary>
    /// The hard-coded length of the launcher.
    /// </summary>
    private const int _length = 80;

    // TODO: add height and width for collision purposes in the editor.
    // It'll probably be a width of 200 and a height of 100.

    /// <summary>
    /// The minimum velocity that the ball will be launched with.
    /// </summary>
    private const float _minVelocity = 5f;
    /// <summary>
    /// The maximum velocity that the ball will be launched with.
    /// </summary>
    private const float _maxVelocity = 15f;
    /// <summary>
    /// The minimum angle that the launcher can face.
    /// </summary>
    private const float _minAngle = 0f;
    /// <summary>
    /// The maximum angle that the launcher can face.
    /// </summary>
    private const float _maxAngle = (float)Math.PI;

    /// <summary>
    /// The texture that the launcher will be drawn with.
    /// </summary>
    private Texture2D _texture;
    
    /// <summary>
    /// The texture that the power meter will be drawn with.
    /// </summary>
    private Texture2D _meterTex;

    /// <summary>
    /// The position of the launcher.
    /// </summary>
    private Vector2 _position;

    /// <summary>
    /// The position of the tip of the launcher.
    /// </summary>
    private Vector2 _tip;

    /// <summary>
    /// Whether the launcher is movable.
    /// </summary>
    private bool _movable;

    /// <summary>
    /// The ball that the launcher is holding.
    /// </summary>
    private Ball _ball;

    /// <summary>
    /// The angle that the launcher is pointing at.
    /// </summary>
    private float _angle;

    /// <summary>
    /// The magnitude that the ball will be launched with.
    /// </summary>
    private float _magnitude;

    /// <summary>
    /// Gets the position of the launcher.
    /// </summary>
    public Vector2 Position {
      get { return _position; }
    }

    /// <summary>
    /// Gets whether the launcher is movable.
    /// </summary>
    public bool Movable {
      get { return _movable; }
    }

    /// <summary>
    /// Gets the angle that the launcher is pointing at.
    /// </summary>
    public float Angle {
      get { return _angle; }
    }

    /// <summary>
    /// Gets the magnitude that the ball will be launched with.
    /// </summary>
    public float Magnitude {
      get { return _magnitude; }
    }

    /// <summary>
    /// Constructs a Launcher object.
    /// </summary>
    /// <param name="texture">The texture that the launcher will be drawn with.</param>
    /// <param name="meterTex">The texture that the power meter will be drawn with.</param>
    /// <param name="position">The position of the launcher.</param>
    public Launcher(Texture2D texture, Texture2D meterTex, Vector2 position) {
      // Set the textures.
      _texture = texture;
      _meterTex = meterTex;

      // TODO: add texture dimensions check

      // Set the launcher's position.
      _position = position;

      // At first, the launcher will be empty, and the launcher is only movable
      // when there's a ball in it.
      _movable = false;
      _ball = null;

      // Initialize the angle and magnitude, and calculate the initial position of the tip.
      _angle = (float)Math.PI / 4; // 45 degrees
      _magnitude = 10f;
      _tip = new Vector2();
      CalculateNewTip();

      // Position the ball.
      UpdateBallPos();
    }

    [Obsolete("This doesn't do anything anymore. Use the constructor! -Jorenz", true)]
    public void Initialize(Texture2D texture, Vector2 position) {

    }

    /// <summary>
    /// Updates the position of the ball.
    /// </summary>
    private void UpdateBallPos() {
      // Move the ball with the launcher's tip if a ball is going to be fired.
      if (_ball != null) {
        _ball.Center = new Vector2(_tip.X, _tip.Y);
      }
    }

    /// <summary>
    /// Adjusts the launcher by a given angle. Only works if the launcher is active.
    /// </summary>
    /// <param name="delta">The value to adjust the angle by.</param>
    public void AdjustAngle(float delta) {
      // We don't want the launcher moving when the ball has already been launched.
      if (!_movable) {
        return;
      }

      // Adjust the angle.
      _angle += delta;

      // Make sure the angle stays within the boundaries.
      // TODO: prevent shooting the ball off the map if the launcher is placed in a corner
      if (_angle < _minAngle) {
        _angle = _minAngle;
      } else if (_angle > _maxAngle) {
        _angle = (float)_maxAngle;
      }

      // Calculate the new position of the launcher's tip and update the position of the ball.
      CalculateNewTip();
      UpdateBallPos();
    }

    /// <summary>
    /// Adjusts the magnitude of the force that the ball will be launched with. Only
    /// works if the launcher is active.
    /// </summary>
    /// <param name="delta">The value to adjust the magnitude by.</param>
    public void AdjustMagnitude(float delta) {
      // We don't want the launcher's magnitude changing when the ball has already been launched.
      if (!_movable) {
        return;
      }

      // Adjust the magnitude.
      _magnitude += delta;

      // Make sure the magnitude stays within the boundaries.
      if (_magnitude < _minVelocity) {
        _magnitude = _minVelocity;
      } else if (_magnitude > _maxVelocity) {
        _magnitude = _maxVelocity;
      }
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
      _movable = true;
      // Make sure that it actually gets displayed.
      UpdateBallPos();
    }

    /// <summary>
    /// Launches the ball in the launcher.
    /// </summary>
    public void LaunchBall() {
      // Don't do anything if no ball is in the launcher.
      if (!_movable) {
        return;
      }

      // Stop the player from moving the launcher.
      _movable = false;

      // Calculate the velocity of the ball based on the launcher's angle and magnitude.
      float xVelocity = _magnitude * (float) Math.Cos(-1 * _angle);
      float yVelocity = _magnitude * (float) Math.Sin(-1 * _angle);
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
       * The flipped y-axis is also the reason why the angle passed to Math.Sin() is multiplied by -1.
       * 
       * I think that helps you and my future self figure out what's going on here :p 
       * After all, who knows if we have to change this later? -Jorenz
       */
      _tip.X = (float)(_length * Math.Cos(_angle) + _position.X);
      _tip.Y = (float)(_length * Math.Sin(-1 * _angle) + _position.Y);
    }

    /// <summary>
    /// Draws the launcher to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when launching the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      // The unit circle goes counter-clockwise, but the rotation parameter goes clockwise, so flip it.
      float rotateAngle = -1 * _angle;

      // Rotate at the midpoint of the upper-left and lower-left corners, for better precision
      Vector2 rotatePos = new Vector2(0f, _texture.Height / 2.0f);

      // Draw the launcher!
      spriteBatch.Draw(_texture, _position, null, Color.White, rotateAngle, rotatePos, 1f, SpriteEffects.None, 0f);

      // Display the power meter if the user is currently aiming the launcher.
      if (_movable) {
        // Determine how much of the texture to display based on the current magnitude.
        int displayedWidth = 10 + (int)(7 * (_magnitude - _minVelocity));
        Rectangle powerMeterRect = new Rectangle(0, 0, displayedWidth, _meterTex.Height);
        // Position the upper-left corner of the meter just below and to the left of the launcher.
        Vector2 displayedPos = new Vector2(_position.X - 40, _position.Y);

        // Draw the power meter!
        spriteBatch.Draw(_meterTex, displayedPos, powerMeterRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      }
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
