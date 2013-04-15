using Microsoft.Xna.Framework;

namespace PuzzlePathDimension {
  class OffsetEffect : IEffect<GraphicsCursor> {
    public float X { get; set; }
    public float Y { get; set; }

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