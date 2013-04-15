namespace PuzzlePathDimension {
  interface IEffect<T> {
    T ApplyTo(T cursor);
  }
}