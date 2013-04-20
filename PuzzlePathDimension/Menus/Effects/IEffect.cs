namespace PuzzlePathDimension {
  /// <summary>
  /// Represents an effect that can be applied to a cursor.
  /// </summary>
  /// <typeparam name="T">The type of cursor that this effect can be applied to.</typeparam>
  interface IEffect<T> {
    /// <summary>
    /// Apply this effect to a cursor.
    /// </summary>
    /// <param name="cursor">The cursor to apply this effect to.</param>
    /// <returns>A cursor with this effect applied.</returns>
    T ApplyTo(T cursor);
  }
}