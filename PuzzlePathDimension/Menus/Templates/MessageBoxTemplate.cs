using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// A template representing some textual information and a small set of buttons.
  /// This is for a small message box.
  /// </summary>
  class MessageBoxTemplate {
    /// <summary>
    /// Labels for each of the three buttons in this view.
    /// </summary>
    public enum Selection { Left = 0, Middle, Right };

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
    /// The list of lines to be shown in this view.
    /// </summary>
    public IList<IMenuLine> Lines { get; private set; }

    /// <summary>
    /// A small set of buttons to be shown in this view.
    /// </summary>
    public IDictionary<Selection, MenuButton> Buttons { get; private set; }

    public string message;

    /// <summary>
    /// The currently selected button.
    /// </summary>
    public Selection SelectedItem { get; set; }

    public MessageBoxTemplate(string message) {
      Lines = new List<IMenuLine>();
      Buttons = new Dictionary<Selection, MenuButton>();
      TransitionPosition = 1.0f;
      this.message = message;
    }

    /// <summary>
    /// Update the state of the view over time.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime) {
      foreach (Selection label in Buttons.Keys) {
        MenuButton button = Buttons[label];

        button.Update(label == SelectedItem, gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, SpriteFont font, float transitionAlpha,
                     Color color, Texture2D gradientTexture) {
      float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
      Vector2 origin = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2, 0);

      // Darken down any other screens that were drawn beneath the popup.
      spriteBatch.FadeBackBufferToBlack(transitionAlpha * 2 / 3);

      GraphicsCursor cursor = new GraphicsCursor();
      cursor.Position = origin;

      spriteBatch.Begin();

      // Center the message text in the viewport.
      Viewport viewport = spriteBatch.GraphicsDevice.Viewport;
      Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);

      //Draw Message Box.
      Vector2 textSize = font.MeasureString(message);
      Vector2 textPosition = (viewportSize - textSize) / 2;

      // The background includes a border somewhat larger than the text itself.
      const int hPad = 32;
      const int vPad = 16;

      Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                    (int)textPosition.Y - vPad,
                                                    (int)textSize.X + hPad * 2,
                                                    (int)textSize.Y + vPad * 6);

      // Draw the background rectangle.
      spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

      // Draw the message box text.
      //spriteBatch.DrawString(font, message, textPosition, color);
      
      // Draw the title
      cursor.Y = 80.0f;
      //-2 * font.LineSpacing;

      if (Title != null) {
        GraphicsCursor titleCursor = cursor;

        //Shift the title based on the current transition state.
        titleCursor = (new OffsetEffect(0, -transitionOffset * 100)).ApplyTo(titleCursor);

        Title.Draw(spriteBatch, titleCursor, gameTime);
      }

      //Draw the content
      cursor.Y = 175.0f; //+2 * font.LineSpacing;
      {
        GraphicsCursor lineCursor = cursor;

        // Modify the alpha to fade text out during transitions.
        lineCursor = (new AlphaEffect(1.0f - TransitionPosition)).ApplyTo(lineCursor);

        // Shift the text based on the current transition state.
        lineCursor = (new OffsetEffect(-transitionOffset * 256, 0)).ApplyTo(lineCursor);

        foreach (IMenuLine line in Lines)
          lineCursor.Y += line.Draw(spriteBatch, lineCursor, gameTime);
      }

      // Draw the buttons
      var labels = new Selection[] { Selection.Left, Selection.Middle, Selection.Right };

      cursor.X = origin.X / 3;
      cursor.Y = 550;

      foreach (Selection label in labels) {
        MenuButton button;
        Buttons.TryGetValue(label, out button);

       if (button != null) {
          GraphicsCursor buttonCursor = cursor;

          // Modify the alpha to fade text out during transitions.
          buttonCursor = (new AlphaEffect(1.0f - TransitionPosition)).ApplyTo(buttonCursor);

          button.Draw(spriteBatch, buttonCursor, label == SelectedItem, gameTime);
        }

        cursor.X += 2 * origin.X / 3;
      }

      spriteBatch.End();
    }

    /// <summary>
    /// Select the next available menu button.
    /// </summary>
    public void SelectNext() {
      //TODO: Find a better way to do this.
      if (SelectedItem == Selection.Left) {
        if (Buttons.ContainsKey(Selection.Middle))
          SelectedItem = Selection.Middle;
        else if (Buttons.ContainsKey(Selection.Right))
          SelectedItem = Selection.Right;
      } else if (SelectedItem == Selection.Middle) {
        if (Buttons.ContainsKey(Selection.Right))
          SelectedItem = Selection.Right;
        else if (Buttons.ContainsKey(Selection.Left))
          SelectedItem = Selection.Left;
      } else if (SelectedItem == Selection.Right) {
        if (Buttons.ContainsKey(Selection.Left))
          SelectedItem = Selection.Left;
        else if (Buttons.ContainsKey(Selection.Middle))
          SelectedItem = Selection.Middle;
      }
    }

    /// <summary>
    /// Select the previous menu button.
    /// </summary>
    public void SelectPrev() {
      // TODO: Find a better way to do this.
      if (SelectedItem == Selection.Left) {
        if (Buttons.ContainsKey(Selection.Right))
          SelectedItem = Selection.Right;
        else if (Buttons.ContainsKey(Selection.Middle))
          SelectedItem = Selection.Middle;
      } else if (SelectedItem == Selection.Middle) {
        if (Buttons.ContainsKey(Selection.Left))
          SelectedItem = Selection.Left;
        else if (Buttons.ContainsKey(Selection.Right))
          SelectedItem = Selection.Right;
      } else if (SelectedItem == Selection.Right) {
        if (Buttons.ContainsKey(Selection.Middle))
          SelectedItem = Selection.Middle;
        else if (Buttons.ContainsKey(Selection.Left))
          SelectedItem = Selection.Left;
      }
    }

    /// <summary>
    /// Confirm the currently selected menu button.
    /// </summary>
    public void Confirm() {
      MenuButton button;
      Buttons.TryGetValue(SelectedItem, out button);
      if (button == null)
        return;

      button.OnSelectEntry(PlayerIndex.One);
    }
  }
}
