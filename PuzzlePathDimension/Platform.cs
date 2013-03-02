using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Platform class describes a platform.
  /// </summary>
  class Platform {
    /// <summary>
    /// The texture that the platform uses.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// The pixel coordinates of the upper left corner of the platform.
    /// </summary>
    private Vector2 _upperLeftCorner;
    /// <summary>
    /// The pixel coordinates of the lower right corner of the platform.
    /// </summary>
    private Vector2 _lowerRightCorner;

    /// <summary>
    /// Whether the platform is active.
    /// </summary>
    private bool _active;

    /// <summary>
    /// Gets the position, which is the upper-left corner, of the platform.
    /// </summary>
    public Vector2 Position {
      get { return _upperLeftCorner; }
    }

    /// <summary>
    /// Gets the pixel coordinates of the upper-left corner of the platform.
    /// </summary>
    public Vector2 UpperLeftCorner {
      get { return _upperLeftCorner; }
    }

    /// <summary>
    /// Gets the pixel coordinates of the lower-right corner of the platform.
    /// </summary>
    public Vector2 LowerRightCorner {
      get { return _lowerRightCorner; }
    }

    /// <summary>
    /// Gets the height of the platform in pixels.
    /// </summary>
    public int Height {
      get { return (int)Math.Abs(_upperLeftCorner.Y - _lowerRightCorner.Y); }
    }

    /// <summary>
    /// Gets the width of the platform in pixels.
    /// </summary>
    public int Width {
      get { return (int)Math.Abs(_upperLeftCorner.X - _lowerRightCorner.X); }
    }

    /// <summary>
    /// Gets whether the platform is active.
    /// </summary>
    public bool Active {
      get { return _active; }
      set { _active = value; }
    }

    // Shouldn't the below code be in a constructor? Or am I missing something?
    // Also, should the Initialize() method accept grid coordinates or pixel
    // coordinates? I have it accepting pixel coordinates right now... -Jorenz

    /// <summary>
    /// Initializes a platform.
    /// </summary>
    /// <param name="texture">The texture to use for the platform.</param>
    /// <param name="origin">The position of the upper-left corner of the vector in pixel coordinates. </param>
    /// <param name="length">The length of the platform, in pixels, in both directions.</param>
    public void Initialize(Texture2D texture, Vector2 origin, Vector2 length) {
      // Complain if something's wrong.
      if (texture == null || origin == null || length == null) {
        throw new ArgumentNullException("Please don't pass in a null value :( -Jorenz");
      } else if (!InBounds(origin)) {
        throw new ArgumentOutOfRangeException("The origin of the platform must be in bounds.");
      } else if (length.X < 1 * Game1.GridSize) { // You can't have a platform of length 0.
        throw new ArgumentOutOfRangeException("Please check the Vector2's X value; it must be at least 20.");
      } else if (length.Y < 1 * Game1.GridSize) {
        throw new ArgumentOutOfRangeException("Please check the Vector2's Y value; it must be at least 20.");
      }

      // Routine stuff.
      _texture = texture;
      _active = true;

      // The upper left corner is easy to figure out.
      _upperLeftCorner = origin;
      // For the lower right corner, the length needs to be added.
      _lowerRightCorner = _upperLeftCorner + length;

      // TODO: Clip platform sizes to the 20x20 grid.
    }

    /// <summary>
    /// Updates the platform's state.
    /// </summary>
    public void Update() {
    }

    /// <summary>
    /// Draws the platform to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      // Scale the texture appropriately to the platform's size.
      Vector2 scale = new Vector2(Width / 20, Height / 20);

      // Draw it!
      spriteBatch.Draw(_texture, _upperLeftCorner, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Checks if the origin of the platform is within the level boundaries.
    /// </summary>
    /// <param name="v">The origin.</param>
    /// <returns>Whether the origin of the platform is inside the level.</returns>
    private bool InBounds(Vector2 v) {
      // It's probably best if these numbers aren't hard-coded. -Jorenz
      return v.X >= 0 && v.X <= 799 && v.Y >= 0 && v.Y <= 599;
    }
  }
}
