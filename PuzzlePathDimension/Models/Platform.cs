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
    /// The texture's color data.
    /// </summary>
    private Color[] _colorData;

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

    /// <summary>
    /// Gets the texture's color data.
    /// </summary>
    /// <returns>The texture's color data as an array.</returns>
    public Color[] GetColorData() {
      // See http://msdn.microsoft.com/en-us/library/0fss9skc.aspx for why
      // this is not a property.
      return (Color[])_colorData.Clone();
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

      // Get the texture's color data, which is used for per-pixel collision
      _colorData = new Color[_texture.Width * _texture.Height];
      _texture.GetData<Color>(_colorData);

      /*// Get the texture's color data, which is used for per-pixel collision
      // TODO: remove some assumptions that this code is making 
      // (that is, everything is a multiple of 20, and only one color is being used for it)
      
      // Make sure the the Color array can hold enough pixels for the whole platform
      _colorData = new Color[(_texture.Width * (int) length.X) * (_texture.Height * (int) length.Y)];
      int pixels = _texture.Width * _texture.Height; // Number of pixels in platform_new.png
      // Store platform_new.png's color data in a temporary array
      Color[] temp = new Color[pixels];
      _texture.GetData<Color>(temp);

      // Copy over the pixel data as many times as needed.
      for (int i = 0; i < _colorData.Length; i += pixels) {
        Array.Copy(temp, 0, _colorData, i, Math.Min(_colorData.Length - i, 400));
        Console.WriteLine("blah");
      }*/

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
