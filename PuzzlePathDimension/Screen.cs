using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  public abstract class Screen {
    //The event associated with the Screen. This event is used to raise events
    //back in the main game class to notify the game that something has changed
    //or needs to be changed
    protected EventHandler ScreenEvent;

    public Screen(EventHandler theScreenEvent) {
      ScreenEvent = theScreenEvent;
    }

    // Update the game world based on external stimuli (time, input, etc.)
    public abstract void Update(GameTime theTime);

    // Draw any objects specific to the screen
    public abstract void Draw(SpriteBatch theBatch);
  }
}
