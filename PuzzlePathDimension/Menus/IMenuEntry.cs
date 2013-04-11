using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  interface IMenuEntry {
    event EventHandler<PlayerIndexEventArgs> Selected;

    Vector2 Position { get; set; }

    void OnSelectEntry(PlayerIndex playerIndex);

    int GetWidth(MenuScreen menu);
    int GetHeight(MenuScreen menu);

    void Update(MenuScreen screen, bool isSelected, GameTime gameTime);
    void Draw(MenuScreen screen, SpriteBatch spriteBatch,
              bool isSelected, GameTime gameTime);
  }
}