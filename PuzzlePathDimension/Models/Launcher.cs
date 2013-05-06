using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace PuzzlePathDimension {
  /// <summary>
  /// The Launcher class represents the level's launcher.
  /// </summary>
  public class Launcher : ILevelObject {
    /// <summary>
    /// The hard-coded length of the launcher.
    /// </summary>
    private const int _length = 80;

    // TODO: add height and width for collision purposes in the editor.
    // It'll probably be a width of 200 and a height of 100 so that the
    // launcher has room to rotate throughout the entire range of valid
    // angles.

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
    /// The texture that the launcher's hand will be drawn with.
    /// </summary>
    private Texture2D _rotatorTex;

    /// <summary>
    /// The texture that the base of the launcher will be drawn with.
    /// </summary>
    private Texture2D _baseTex;

    /// <summary>
    /// The texture that the power meter will be drawn with.
    /// </summary>
    private Texture2D _meterTex;

    /// <summary>
    /// The position of the launcher. This is the upper left corner
    /// of the "hand" of the launcher when it is completely horizontal.
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
      set{ _position = value;}
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
    /// This delegate is called when the ball is launched.
    /// </summary>
    public delegate void BallLaunch();
    /// <summary>
    /// Occurs when the ball is launched.
    /// </summary>
    public event BallLaunch OnBallLaunch;

    /// <summary>
    /// Constructs a Launcher object.
    /// </summary>
    /// <param name="textures">An array of textures that will be used to draw the launcher's 
    /// parts. The texture in index 0 is the "stick", the texture in index 1 is the base, and 
    /// the texture in index 2 is the power meter. The "stick" should be of length 80, the
    /// base should be 120x120, and the power meter should be 80x10.</param>
    /// <param name="position">The position of the launcher.</param>
    public Launcher(Texture2D[] textures, Vector2 position) {
      // Set the textures.
      if (textures != null) {
        _rotatorTex = textures[0];
        _baseTex = textures[1];
        _meterTex = textures[2];
      }

      // TODO: add texture dimensions check

      if (position.X < 0 || position.Y < 0) {
        throw new ArgumentOutOfRangeException("The position should not be negative.");
      }

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

    /// <summary>
    /// Updates the position of the ball.
    /// </summary>
    private void UpdateBallPos() {
      // Move the ball with the launcher's tip if a ball is going to be fired.
      if (_ball != null && _ball.BodyActive) {
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
      float xVelocity = _magnitude * (float)Math.Cos(-1 * _angle);
      float yVelocity = _magnitude * (float)Math.Sin(-1 * _angle);
      _ball.Launch(xVelocity, yVelocity);

      // Call any methods that are waiting for the ball to launch.
      if (OnBallLaunch != null) {
        OnBallLaunch();
      }

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
      Vector2 rotatePos = new Vector2(0f, _rotatorTex.Height / 2.0f);

      // Draw the launcher's "hand", for lack of a better term.
      spriteBatch.Draw(_rotatorTex, _position, null, Color.White, rotateAngle, rotatePos, 1f, SpriteEffects.None, 0f);

      // Draw the base after the "hand", but figure out where to draw it in the first place.
      Vector2 basePos = new Vector2(_position.X - 60, _position.Y - 115); // Based on the hand's position
      spriteBatch.Draw(_baseTex, basePos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

      // Last, display the power meter if the user is currently aiming the launcher.
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

    public Boolean IsSelected(Point ms) {
      Rectangle launcherBoundingBox = new Rectangle((int)(Position.X - 100), (int)(Position.Y - 100), 200, 100);
      if (ms.X > launcherBoundingBox.X && ms.X < launcherBoundingBox.X + 200) {
        if (ms.Y > launcherBoundingBox.Y && ms.Y < launcherBoundingBox.Y + 100) {
          return true;
        }
      }
      return false;
    }

    private Side CheckLeftRight(Vector2 v) {
      // Subtract 1 to account for the fact that the origin is at (0,0).
      if (v.X - 100 < 5) {
        return Side.Left;
      }
      if (v.X + 100 > Simulation.FieldWidth - 5) {
        return Side.Right;
      }
      //else if(v.X >=5 && v.X+Width <= Simulation.FieldWidth - 5 && v.Y >= 5 && v.Y+Height <= Simulation.FieldHeight - 5)
      //  return Side.None; 
      return Side.None;
    }
    //Vector2 originalPosition;
    public enum Side { Top, Right, Bottom, Left, None };


    public void Move(Vector2 change) {
      Vector2 newPosition = Position + change;
      Side horizontal = CheckLeftRight(newPosition);

      if (horizontal == Side.Left) {
        change.X = 5 - Position.X + 100;
      } else if (horizontal == Side.Right) {
        change.X = (Simulation.FieldWidth - 5) - (Position.X + 100);
      }

      change.Y = 0;

      Position = Position + change;

    }


  }
}
