using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class LinesTemplate : IMenuEntry {
    public event EventHandler<PlayerIndexEventArgs> Selected;

    public Vector2 Position { get; set; }
    public float TransitionPosition { get; set; }

    public IMenuLine Title { get; set; }
    public IList<IMenuLine> Lines { get; private set; }

    public LinesTemplate() {
      Lines = new List<IMenuLine>();
      TransitionPosition = 1.0f;
    }

    /// <summary>
    /// Method for raising the Selected event.
    /// </summary>
    public void OnSelectEntry(PlayerIndex playerIndex) {
      if (Selected != null)
        Selected(this, new PlayerIndexEventArgs(playerIndex));
    }

    public int GetWidth() {
      return 0;
    }

    public int GetHeight() {
      return Lines.Aggregate(0, (acc, credit) => acc + credit.Height);
    }

    public void Update(MenuScreen screen, bool isSelected, GameTime gameTime) {
    }

    /// <summary>
    /// Draw onto the screen the names in the members list,
    /// organizations list, and the individual contributions list.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Draw(SpriteBatch spriteBatch, bool isSelected, GameTime gameTime) {
      Vector2 origin = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2, 0);
      Vector2 cursor = origin; // The current drawing location

      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

      // Draw the title
      if (Title != null) {
        cursor = new Vector2(origin.X - Title.Width / 2, 80 - 100 * transitionOffset);
        Title.Draw(spriteBatch, cursor, gameTime);
      }

      // Draw the content
      int width = Lines.Aggregate(0, (acc, credit) => Math.Max(acc, credit.Width)) / 2;

      cursor = new Vector2(origin.X - width - 256*transitionOffset, 175.0f);
      foreach (IMenuLine credit in Lines) {
        Color tmp = credit.Color;
        credit.Color = tmp * (1.0f - TransitionPosition);

        cursor.Y += credit.Draw(spriteBatch, cursor, gameTime);

        credit.Color = tmp;
      }
    }
  }
}