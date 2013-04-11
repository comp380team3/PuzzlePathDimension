using System;

namespace PuzzlePathDimension {
#if WINDOWS || XBOX
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string[] args) {
      using (PuzzlePathGame game = new PuzzlePathGame()) {
        game.Run();
      }
    }
  }
#endif
}

