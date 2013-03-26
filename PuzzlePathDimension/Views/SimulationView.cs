using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class SimulationView : View<Simulation> {
    private SpriteBatch _spriteBatch = null;

    public Simulation BackingModel { get; private set; }

    public SimulationView(Simulation simulation, SpriteBatch spriteBatch) {
      this._spriteBatch = spriteBatch;
      this.BackingModel = simulation;
    }

    public void Draw() {
      _spriteBatch.Draw(BackingModel.Background, Vector2.Zero, Color.White);

      // Draw the goal on the canvas
      BackingModel.Goal.Draw(_spriteBatch);

      // Draw the platform on the canvas
      foreach (Platform platform in BackingModel.Platforms) {
        platform.Draw(_spriteBatch);
      }

      // Draw the treasures on the canvas
      foreach (Treasure treasure in BackingModel.Treasures) {
        treasure.Draw(_spriteBatch);
      }

      // Draw the ball onto the canvas
      BackingModel.Ball.Draw(_spriteBatch);

      // Draw the launcher on the canvas
      BackingModel.Launcher.Draw(_spriteBatch);
    }
  }
}