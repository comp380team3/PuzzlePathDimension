using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  /// <summary>
  /// Applies a translational effect to a graphical cursor.
  /// </summary>
  class OffsetEffect : IEffect<GraphicsCursor> {
    /// <summary>
    /// The X-offset to apply.
    /// </summary>
    public float X { get; set; }
    /// <summary>
    /// The Y-offset to apply.
    /// </summary>
    public float Y { get; set; }

    // The X and Y offsets as a vector.
    public Vector2 Offset {
      get { return new Vector2(X, Y); }
      set { X = value.X; Y = value.Y; }
    }

    public OffsetEffect(float x, float y) {
      X = x;
      Y = y;
    }

    public GraphicsCursor ApplyTo(GraphicsCursor cursor) {
      cursor.Position += Offset;
      return cursor;
    }
  }
}