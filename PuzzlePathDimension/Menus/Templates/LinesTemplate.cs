using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class LinesTemplate : IMenuEntry {
    public event EventHandler<PlayerIndexEventArgs> Selected;

    public Vector2 Position { get; set; }
    public IList<IMenuLine> Lines { get; private set; }

    public LinesTemplate() {
      Lines = new List<IMenuLine>();
    }

    /// <summary>
    /// Method for raising the Selected event.
    /// </summary>
    public void OnSelectEntry(PlayerIndex playerIndex) {
      if (Selected != null)
        Selected(this, new PlayerIndexEventArgs(playerIndex));
    }

    public int GetWidth(MenuScreen screen) {
      return Lines.Aggregate(0, (acc, credit) => Math.Max(acc, credit.Width));
    }

    public int GetHeight(MenuScreen screen) {
      return Lines.Aggregate(0, (acc, credit) => acc + credit.Height);
    }

    public void Update(MenuScreen screen, bool isSelected, GameTime gameTime) {
    }

    /// <summary>
    /// Draw onto the screen the names in the members list,
    /// organizations list, and the individual contributions list.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Draw(MenuScreen screen, SpriteBatch spriteBatch, bool isSelected, GameTime gameTime) {
      Vector2 position = Position;
      foreach (IMenuLine credit in Lines)
        position.Y += credit.Draw(spriteBatch, position, gameTime);
    }
  }
}