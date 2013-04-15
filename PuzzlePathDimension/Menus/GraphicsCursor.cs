using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// A cursor representing the rendering effects currently active.
  /// </summary>
  struct GraphicsCursor {
    /// <summary>
    /// The horizontal offset of graphics drawn at this cursor.
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// The vertical offset of graphics drawn at this cursor.
    /// </summary>
    public float Y { get; set; }

    float scaling;
    /// <summary>
    /// The scaling factor to apply to graphics drawn at this cursor.
    /// </summary>
    public float Scaling {
      get {
        return scaling + 1.0f;
      }
      set {
        scaling = value - 1.0f;
      }
    }

    float alpha;
    /// <summary>
    /// The transparency value of graphics drawn at this cursor.
    /// </summary>
    public float Alpha {
      get {
        return 1.0f - alpha;
      }

      set {
        alpha = 1.0f - MathHelper.Clamp(value, 0.0f, 1.0f);
      }
    }

    /// <summary>
    /// The X and Y offsets as a vector.
    /// </summary>
    public Vector2 Position {
      get { return new Vector2(X, Y); }
      set { X = value.X; Y = value.Y; }
    }
  }
}