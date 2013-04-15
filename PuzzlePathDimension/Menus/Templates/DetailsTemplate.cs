﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// A template representing some textual information and a small set of buttons.
  /// </summary>
  class DetailsTemplate {
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

    /// <summary>
    /// The currently selected button.
    /// </summary>
    public Selection SelectedItem { get; set; }


    public DetailsTemplate() {
      Lines = new List<IMenuLine>();
      Buttons = new Dictionary<Selection, MenuButton>();
      TransitionPosition = 1.0f;
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
      cursor.Alpha = 1.0f;

      spriteBatch.Begin();

      // Draw the title
      cursor.Y = 80.0f;
      if (Title != null) {
        GraphicsCursor titleCursor = cursor;

        // Center the title text.
        // TODO: This really belongs in a MenuLine subclass.
        titleCursor = (new OffsetEffect(-Title.Width / 2, 0)).ApplyTo(titleCursor);

        // Shift the title based on the current transition state.
        titleCursor = (new OffsetEffect(0, -transitionOffset * 100)).ApplyTo(titleCursor);

        Title.Draw(spriteBatch, titleCursor, gameTime);
      }

      // Draw the content
      cursor.Y = 175.0f;
      {
        GraphicsCursor lineCursor = cursor;

        // Modify the alpha to fade text out during transitions.
        lineCursor = (new AlphaEffect(1.0f - TransitionPosition)).ApplyTo(lineCursor);

        // Center the text.
        // TODO: This really belongs in the MenuLine class.
        int width = Lines.Aggregate(0, (acc, credit) => Math.Max(acc, credit.Width));
        lineCursor = (new OffsetEffect(-width / 2, 0)).ApplyTo(lineCursor);

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

          // Center the button text.
          // TODO: This really belongs in MenuButton.
          buttonCursor = (new OffsetEffect(-button.GetWidth() / 2, 0)).ApplyTo(buttonCursor);

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
      // TODO: Find a better way to do this.
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