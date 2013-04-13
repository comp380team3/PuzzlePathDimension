using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  class LinesTemplate {
    public enum Selection { Left = 0, Middle, Right };

    public event EventHandler<PlayerIndexEventArgs> Cancelled;

    public float TransitionPosition { get; set; }

    public IMenuLine Title { get; set; }
    public IList<IMenuLine> Lines { get; private set; }
    public IDictionary<Selection, MenuButton> Buttons { get; private set; }

    public Selection SelectedItem { get; set; }

    public LinesTemplate() {
      Lines = new List<IMenuLine>();
      Buttons = new Dictionary<Selection, MenuButton>();
      TransitionPosition = 1.0f;
    }

    public void Update(bool isSelected, GameTime gameTime) {
      foreach (Selection label in Buttons.Keys) {
        MenuButton button = Buttons[label];
        button.Update(label == SelectedItem, gameTime);
      }
    }

    public void SelectNext() {
      // TODO
    }

    public void SelectPrev() {
      // TODO
    }

    public void Confirm() {
      MenuButton button;
      Buttons.TryGetValue(SelectedItem, out button);
      if (button == null)
        return;

      button.OnSelectEntry(PlayerIndex.One);
    }

    public void Cancel() {
      if (Cancelled != null)
        Cancelled(this, new PlayerIndexEventArgs(PlayerIndex.One));
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

      spriteBatch.Begin();

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

      // Draw the buttons
      var labels = new Selection[] { Selection.Left, Selection.Middle, Selection.Right};

      cursor = new Vector2(origin.X / 3, 550);
      foreach (Selection label in labels) {
        MenuButton button;
        Buttons.TryGetValue(label, out button);

        if (button != null) {
          Color color = (label == SelectedItem) ? Color.Yellow : Color.White;
          color *= (1.0f - TransitionPosition);

          cursor.X -= button.GetWidth() / 2;
          button.Position = cursor;
          button.Color = color;
          button.Draw(spriteBatch, label == SelectedItem, gameTime);
          cursor.X += button.GetWidth() / 2;
        }

        cursor.X += 2 * origin.X / 3;
      }

      spriteBatch.End();
    }
  }
}