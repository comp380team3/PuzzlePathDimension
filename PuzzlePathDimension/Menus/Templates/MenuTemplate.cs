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
      TransitionPosition = 1.0f;
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


    public void Update(GameTime gameTime) {
      for (var i = 0; i < Items.Count; ++i) {
        MenuButton button = Items[i];

        if (SelectedItem == i)
          button.Color = Color.Yellow;
        else
          button.Color = Color.White;
        button.Update(SelectedItem == i, gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
      Vector2 origin = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2, 0);

      GraphicsCursor cursor = new GraphicsCursor();
      cursor.Position = origin;
      cursor.Alpha = 1.0f;

      spriteBatch.Begin();

      // Draw the title
      cursor.Y = 80;
      if (Title != null) {
        GraphicsCursor titleCursor = cursor;

        // Center the title text.
        // TODO: This really belongs in a MenuLine subclass.
        titleCursor = (new OffsetEffect(-Title.Width / 2, 0)).ApplyTo(titleCursor);

        // Shift the title based on the current transition state.
        titleCursor = (new OffsetEffect(0, -transitionOffset * 100)).ApplyTo(titleCursor);

        Title.Draw(spriteBatch, titleCursor.Position, gameTime);
      }

      // Draw the menu items
      cursor.Y = 175;
      for (var i = 0; i < Items.Count; ++i) {
        MenuButton button = Items[i];
        GraphicsCursor buttonCursor = cursor;

        // Center the button text.
        // TODO: This really belongs in MenuButton.
        buttonCursor = (new OffsetEffect(-button.GetWidth() / 2, 0)).ApplyTo(buttonCursor);

        // Shift the button based on the current transition state.
        buttonCursor = (new OffsetEffect(-transitionOffset * 256, 0)).ApplyTo(buttonCursor);

        // Modify the alpha to fade text out during transitions.
        buttonCursor = (new AlphaEffect(1.0f - TransitionPosition)).ApplyTo(buttonCursor);

        // Draw the button.
        button.Draw(spriteBatch, buttonCursor, SelectedItem == i, gameTime);

        // not an effect
        cursor.Y += button.GetHeight();
      }

      spriteBatch.End();
    }
  }
}