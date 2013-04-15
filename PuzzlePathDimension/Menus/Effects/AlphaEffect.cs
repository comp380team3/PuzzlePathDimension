using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  class AlphaEffect : IEffect<GraphicsCursor> {
    float alpha;
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