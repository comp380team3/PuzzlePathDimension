using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  public interface GameState {
    // Update the game world based on external stimuli (time, input, etc.)
    void Update(GameTime theTime);

    // Draw any objects specific to the screen
    void Draw(SpriteBatch theBatch);
  }
}
