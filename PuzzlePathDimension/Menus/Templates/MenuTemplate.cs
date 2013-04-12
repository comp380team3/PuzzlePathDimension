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

    public float TransitionPosition { get; set; }

    public MenuTemplate() {
      Items = new List<MenuButton>();
      TransitionPosition = 0.0f;
    }

    public void SelectNext() {
      SelectedItem = (SelectedItem + 1) % Items.Count;
    }

    public void SelectPrev() {
      // Modulo on negative numbers behaves strangely, so add Items.Count
      // to ensure that we're never dealing with a negative number.
      SelectedItem = (SelectedItem - 1 + Items.Count) % Items.Count;
    }

    public void Confirm() {
      Items[SelectedItem].OnSelectEntry(PlayerIndex.One);
    }

    public void Cancel() {
      if (Cancelled != null)
        Cancelled(this, new PlayerIndexEventArgs(PlayerIndex.One));
    }


    public void Update(MenuScreen screen, bool isSelected, GameTime gameTime) {
      for (var i = 0; i < Items.Count; ++i) {
        MenuButton button = Items[i];
        button.Update(SelectedItem == i, gameTime);
      }
    }

    public void Draw(MenuScreen screen, SpriteBatch spriteBatch, bool isSelected, GameTime gameTime) {
      Vector2 origin = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2, 0);
      Vector2 cursor = origin; // The current drawing location

      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

      spriteBatch.Begin();

      // Draw the title
      if (Title != null) {
        cursor.X = origin.X - Title.Width / 2;
        cursor.Y = 80 - transitionOffset * 100;
        Title.Draw(spriteBatch, cursor, gameTime);
      }

      // Draw the menu items
      cursor.Y = 175;
      for (var i = 0; i < Items.Count; ++i) {
        MenuButton button = Items[i];

        cursor.X = origin.X - button.GetWidth() / 2;
        cursor.X -= transitionOffset * 256;

        button.Position = cursor;
        button.Draw(screen, spriteBatch, SelectedItem == i, gameTime);

        cursor.Y += button.GetHeight();
      }

      spriteBatch.End();
    }
  }
}