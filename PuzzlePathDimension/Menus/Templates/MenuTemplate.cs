using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// A template representing a vertical series of selectable buttons.
  /// </summary>
  class MenuTemplate {
    /// <summary>
    /// The amount of transition that has been done.
    /// 0.0f means "fully transitioned".
    /// 1.0f means "not transitioned at all".
    /// </summary>
    public float TransitionPosition { get; set; }


    /// <summary>
    /// The view's title.
    /// </summary>
    public IMenuLine Title { get; set; }

    /// <summary>
    /// The list of menu buttons to be displayed.
    /// </summary>
    public IList<MenuButton> Items { get; private set; }

    /// <summary>
    /// The currently selected button.
    /// </summary>
    public int SelectedItem { get; set; }

    private IList<Tuple<Rectangle, MenuButton>> ItemRects { get; set; }


    public MenuTemplate() {
      Items = new List<MenuButton>();
      ItemRects = new List<Tuple<Rectangle, MenuButton>>();
      TransitionPosition = 1.0f;
    }

    /// <summary>
    /// Update the state of the view over time.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime) {
      for (var i = 0; i < Items.Count; ++i) {
        MenuButton button = Items[i];

        button.Update(SelectedItem == i, gameTime);
      }
    }

    /// <summary>
    /// Draw the current state of this view to the screen.
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="gameTime"></param>
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
      Vector2 origin = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2, 0);

      GraphicsCursor cursor = new GraphicsCursor();
      cursor.Position = origin;

      spriteBatch.Begin();

      // Draw the title
      cursor.Y = 80;
      if (Title != null) {
        GraphicsCursor titleCursor = cursor;

        // Shift the title based on the current transition state.
        titleCursor = (new OffsetEffect(0, -transitionOffset * 100)).ApplyTo(titleCursor);

        Title.Draw(spriteBatch, titleCursor, gameTime);
      }

      ItemRects.Clear();

      // Draw the menu items
      cursor.Y = 175;
      for (var i = 0; i < Items.Count; ++i) {
        MenuButton button = Items[i];
        GraphicsCursor buttonCursor = cursor;

        // Shift the button based on the current transition state.
        buttonCursor = (new OffsetEffect(-transitionOffset * 256, 0)).ApplyTo(buttonCursor);

        // Modify the alpha to fade text out during transitions.
        buttonCursor = (new AlphaEffect(1.0f - TransitionPosition)).ApplyTo(buttonCursor);

        // Draw the button.
        Rectangle box = button.Draw(spriteBatch, buttonCursor, SelectedItem == i, gameTime);

        ItemRects.Add(Tuple.Create(box, button));

        cursor.Y += box.Height;
      }

      spriteBatch.End();
    }


    /// <summary>
    /// Select the next available menu button.
    /// </summary>
    public void SelectNext() {
      SelectedItem = (SelectedItem + 1) % Items.Count;
    }

    /// <summary>
    /// Select the previous menu button.
    /// </summary>
    public void SelectPrev() {
      // Modulo on negative numbers behaves strangely, so add Items.Count
      // to ensure that we're never dealing with a negative number.
      SelectedItem = (SelectedItem - 1 + Items.Count) % Items.Count;
    }

    /// <summary>
    /// Confirm the currently selected menu button.
    /// </summary>
    public void Confirm() {
      Items[SelectedItem].OnSelectEntry();
    }

    public void SelectAtPoint(Point pointer) {
      MenuButton button = null;

      foreach (var entry in ItemRects) {
        Rectangle rect = entry.Item1;
        if (rect.Contains(pointer)) {
          button = entry.Item2;
          break;
        }
      }

      if (button == null)
        return;

      for (int i = 0; i < Items.Count; ++i) {
        if (button == Items[i])
          SelectedItem = i;
      }
    }
  }
}