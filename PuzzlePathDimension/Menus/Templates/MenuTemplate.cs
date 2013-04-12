using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class MenuTemplate {
    public event EventHandler<PlayerIndexEventArgs> Cancelled;

    public IMenuLine Title { get; set; }
    public IList<MenuButton> Items { get; private set; }
    public int SelectedItem { get; private set; }

    public MenuTemplate() {
      Items = new List<MenuButton>();
    }

    public void Update(MenuScreen screen, bool isSelected, GameTime gameTime) {
      for (var i = 0; i < Items.Count; ++i) {
        MenuButton button = Items[i];
        button.Update(screen, SelectedItem == i, gameTime);
      }
    }

    public void HandleInput(VirtualController vtroller) {
      // Move to the previous menu entry?
      if (vtroller.CheckForRecentRelease(VirtualButtons.Up)) {
        // Modulo on negative numbers behaves strangely, so add Items.Count
        // to ensure that we're never dealing with a negative number.
        SelectedItem = (SelectedItem - 1 + Items.Count) % Items.Count;
      }

      // Move to the next menu entry?
      if (vtroller.CheckForRecentRelease(VirtualButtons.Down)) {
        SelectedItem = (SelectedItem + 1) % Items.Count;
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        Items[SelectedItem].OnSelectEntry(PlayerIndex.One);
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        if (Cancelled != null)
          Cancelled(this, new PlayerIndexEventArgs(PlayerIndex.One));
      }
    }

    public void Draw(MenuScreen screen, SpriteBatch spriteBatch, bool isSelected, GameTime gameTime) {
      Vector2 origin = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2, 0);
      Vector2 cursor = origin;

      spriteBatch.Begin();

      if (Title != null) {
        cursor.X = origin.X - Title.Width / 2;
        cursor.Y = 80;
        Title.Draw(spriteBatch, cursor, gameTime);
      }

      cursor.Y = 175;
      for (var i = 0; i < Items.Count; ++i) {
        MenuButton button = Items[i];

        cursor.X = origin.X - button.GetWidth(screen) / 2;

        button.Position = cursor;
        button.Draw(screen, spriteBatch, SelectedItem == i, gameTime);

        cursor.Y += button.GetHeight(screen);
      }

      spriteBatch.End();
    }
  }
}