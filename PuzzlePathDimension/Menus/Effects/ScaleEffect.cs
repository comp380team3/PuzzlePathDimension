namespace PuzzlePathDimension {
  /// <summary>
  /// Applies a scaling effect to a graphical cursor.
  /// </summary>
  class ScaleEffect : IEffect<GraphicsCursor> {
    /// <summary>
    /// The scaling factor to apply.
    /// </summary>
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