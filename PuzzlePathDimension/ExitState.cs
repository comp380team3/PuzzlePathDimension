using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  public class ExitState : Screen {
    Game1 game1;

    public ExitState(Game1 game1)
      : base(new EventHandler((o, e) => { })) {
      this.game1 = game1;
    }

    public override void Update(GameTime gameTime) {
      game1.Exit();
    }

    public override void Draw(SpriteBatch graphics) {
    }
  }
}
