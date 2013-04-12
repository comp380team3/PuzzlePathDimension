using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  interface IMenuEntry {
    event EventHandler<PlayerIndexEventArgs> Selected;

    Vector2 Position { get; set; }

    void OnSelectEntry(PlayerIndex playerIndex);

    int GetWidth();
    int GetHeight();

    void Update(MenuScreen screen, bool isSelected, GameTime gameTime);
    void Draw(SpriteBatch spriteBatch, bool isSelected, GameTime gameTime);
  }
}