using System;

namespace PuzzlePathDimension {
  /// <summary>
  /// Only applies the given effect if a predicate evaluates to true.
  /// </summary>
  /// <typeparam name="T">The type of cursor that this effect can be applied to.</typeparam>
  class EffectGuard<T> : IEffect<T> {
    private IEffect<T> Effect { get; set; }
    public Func<bool> Predicate { get; set; }

    public EffectGuard(Func<bool> predicate, IEffect<T> effect) {
      Predicate = predicate;
      Effect = effect;
    }

    public T ApplyTo(T cursor) {
      if (Predicate.Invoke())
        cursor = Effect.ApplyTo(cursor);
      return cursor;
    }
  }
}