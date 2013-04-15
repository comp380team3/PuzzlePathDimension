namespace PuzzlePathDimension {
  /// <summary>
  /// Combines multiple effects into a single effect.
  /// </summary>
  /// <typeparam name="T">The type of cursor that this effect can be applied to.</typeparam>
  class EffectGroup<T> : IEffect<T> {
    private IEffect<T>[] Effects { get; set; }
    public EffectGroup(params IEffect<T>[] effects) {
      Effects = effects;
    }

    public T ApplyTo(T cursor) {
      foreach (IEffect<T> effect in Effects)
        cursor = effect.ApplyTo(cursor);
      return cursor;
    }
  }
}