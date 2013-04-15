using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  /// <summary>
  /// Applies a transparency effect to a graphical cursor.
  /// </summary>
  class AlphaEffect : IEffect<GraphicsCursor> {
    float alpha;
    /// <summary>
    /// The transparency factor to apply.
    /// </summary>
    public float Alpha {
      get { return alpha; }
      set { alpha = MathHelper.Clamp(value, 0.0f, 1.0f); }
    }

    public AlphaEffect(float alpha) {
      Alpha = alpha;
    }

    public GraphicsCursor ApplyTo(GraphicsCursor cursor) {
      cursor.Alpha *= Alpha;
      return cursor;
    }
  }
}