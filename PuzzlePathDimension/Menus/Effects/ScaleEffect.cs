namespace PuzzlePathDimension {
  class ScaleEffect : IEffect<GraphicsCursor> {
    public float Scaling { get; set; }
    public ScaleEffect(float factor) {
      Scaling = factor;
    }

    public GraphicsCursor ApplyTo(GraphicsCursor cursor) {
      cursor.Scaling *= Scaling;
      return cursor;
    }
  }
}