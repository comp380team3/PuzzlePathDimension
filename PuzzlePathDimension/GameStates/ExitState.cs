using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  public class ExitState : GameState {
    Game1 game1;

    public ExitState(Game1 game1) {
      this.game1 = game1;
    }

    public void Update(GameTime gameTime) {
      game1.Exit();
    }

    public void Draw(GameTime gameTime, SpriteBatch graphics) {
    }
  }
}
