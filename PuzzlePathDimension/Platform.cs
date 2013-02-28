using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimensionSampleDemo {
  /// <summary>
  /// The Platform class describes a platform.
  /// </summary>
  class Platform {
    /// <summary>
    /// The texture that the platform uses.
    /// </summary>
    public Texture2D PlatformTexture;

    /// <summary>
    /// The pixel coordinates of the upper left corner of the platform.
    /// </summary>
    private Vector2 _upperLeftCorner;
    /// <summary>
    /// The pixel coordinates of the lower right corner of the platform.
    /// </summary>
    private Vector2 _lowerRightCorner;

    /// <summary>
    /// Gets the pixel coordinates of the upper left corner of the platform.
    /// </summary>
    public Vector2 UpperLeftCorner {
      get { return _upperLeftCorner; }
    }

    /// <summary>
    /// Gets the pixel coordinates of the lower right corner of the platform.
    /// </summary>
    public Vector2 LowerRightCorner {
      get { return _lowerRightCorner; }
    }

    /// <summary>
    /// Gets the height of the platform in pixels.
    /// </summary>
    public int Height {
      get { return (int) Math.Abs(_upperLeftCorner.Y - _lowerRightCorner.Y); }
    }

    /// <summary>
    /// Gets the width of the platform in pixels.
    /// </summary>
    public int Width {
      get { return (int) Math.Abs(_upperLeftCorner.X - _lowerRightCorner.X); }
    }

    /// <summary>
    /// Whether the platform is active.
    /// </summary>
    public bool Active;

    // Shouldn't the below code be in a constructor? Or am I missing something?
    // Also, should the Initialize() method accept grid coordinates or pixel
    // coordinates? I have it accepting grid coordinates right now, but if you
    // make sure that anything you pass into Initialize() is divisible by 20,
    // then I can easily change the code to do that instead. -Jorenz

    /// <summary>
    /// Initializes a platform.
    /// </summary>
    /// <param name="texture">The texture to use as a platform.</param>
    /// <param name="origin">The position of the upper-left corner of the vector.
    /// This must be between (0, 0) and (39, 29); in other words, it must be in the
    /// 40x30 grid. </param>
    /// <param name="length">The length of the platform in both directions.
    /// Again, this must correspond to the 40x30 grid.</param>
    public void Initialize(Texture2D texture, Vector2 origin, Vector2 length) {
      // Well...yeah. :p
      if (texture == null || origin == null || length == null) {
        throw new ArgumentNullException("Please don't pass in a null value :( -Jorenz");
      }

      if (!InBounds(origin)) {
        throw new ArgumentOutOfRangeException("The origin of the platform must be in bounds.");
      }

      // You can't have a platform that is of zero length in either direction;
      // a platform needs to occupy at least one 20x20 tile.
      if (length.X < 1) {
        throw new ArgumentOutOfRangeException("Please check the Vector2's X value; it must be at least 1.");
      } else if (length.Y < 1) {
        throw new ArgumentOutOfRangeException("Please check the Vector2's Y value; it must be at least 1.");
      }

      // Routine stuff.
      PlatformTexture = texture;
      Active = true;

      // Convert both vectors to the pixel coordinates by multiplying each vector
      // by a scalar of 20.
      origin = origin * 20;
      length = length * 20;

      // The upper left corner is easy to figure out.
      _upperLeftCorner = origin;
      // For the lower right corner, the length needs to be added.
      _lowerRightCorner = _upperLeftCorner + length;
    }

    public void Update() {
    }

    /// <summary>
    /// Draws the platform to the screen.
    /// </summary>
    /// <param name="spriteBatch"></param>
    public void Draw(SpriteBatch spriteBatch) {
      // Scale the texture appropriately to the platform's size.
      Vector2 scale = new Vector2(Width / 20, Height / 20);

      // Draw it!
      spriteBatch.Draw(PlatformTexture, _upperLeftCorner, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Checks if the origin of the platform is within the level boundaries.
    /// </summary>
    /// <param name="v">The origin.</param>
    /// <returns>Whether the origin of the platform is inside the level.</returns>
    private bool InBounds(Vector2 v) {
      return v.X >= 0 && v.X <= 39 && v.Y >= 0 && v.Y <= 29;
    }
  }
}
