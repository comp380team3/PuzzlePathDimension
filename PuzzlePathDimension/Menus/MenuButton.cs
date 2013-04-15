//-----------------------------------------------------------------------------
// MenuEntry.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// Helper class represents a single entry in a MenuScreen. By default this
  /// just draws the entry text string, but it can be customized to display menu
  /// entries in different ways. This also provides an event that will be raised
  /// when the menu entry is selected.
  /// </summary>
  class MenuButton {
    /// <summary>
    /// Tracks a fading selection effect on the entry.
    /// </summary>
    /// <remarks>
    /// The entries transition out of the selection effect when they are deselected.
    /// </remarks>
    float selectionFade;

    /// <summary>
    /// Gets or sets the text of this menu entry.
    /// </summary>
    public string Text { get; set; }

    public SpriteFont Font { get; set; }

    public Color Color { get; set; }

    /// <summary>
    /// Gets or sets the position at which to draw this menu entry. This is set by the
    /// MenuScreen each frame in Update.
    /// </summary>
    public Vector2 Position { get; set; }


    /// <summary>
    /// Event raised when the menu entry is selected.
    /// </summary>
    public event EventHandler<PlayerIndexEventArgs> Selected;


    /// <summary>
    /// Method for raising the Selected event.
    /// </summary>
    public virtual void OnSelectEntry(PlayerIndex playerIndex) {
      if (Selected != null)
        Selected(this, new PlayerIndexEventArgs(playerIndex));
    }


    /// <summary>
    /// Constructs a new menu entry with the specified text.
    /// </summary>
    public MenuButton(string text, SpriteFont font) {
      Text = text;
      Font = font;
    }


    /// <summary>
    /// Updates the menu entry.
    /// </summary>
    public void Update(bool isSelected, GameTime gameTime) {
      // When the menu selection changes, entries gradually fade between
      // their selected and deselected appearance, rather than instantly
      // popping to the new state.
      float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

      if (isSelected)
        selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
      else
        selectionFade = Math.Max(selectionFade - fadeSpeed, 0);

      Color = isSelected ? Color.Yellow : Color.White;
    }

    /// <summary>
    /// Draws the menu entry. This can be overridden to customize the appearance.
    /// </summary>
    public virtual void Draw(SpriteBatch spriteBatch, GraphicsCursor cursor, bool isSelected, GameTime gameTime) {
      // Vertically center text on the middle of each line.
      Vector2 origin = new Vector2(0, Font.LineSpacing / 2);

      // Pulsate the size of the selected menu entry.
      double time = gameTime.TotalGameTime.TotalSeconds;
      float pulsate = (float)Math.Sin(time * 6) + 1;
      float scale = 1 + pulsate * 0.05f * selectionFade;

      cursor = (new ScaleEffect(scale)).ApplyTo(cursor);
      cursor = (new OffsetEffect(-(Font.MeasureString(Text).X / 2) * (scale - 1), 0)).ApplyTo(cursor);

      Color color = Color * cursor.Alpha;
      spriteBatch.DrawString(Font, Text, cursor.Position, color, 0,
                             origin, cursor.Scaling, SpriteEffects.None, 0);
    }


    /// <summary>
    /// Queries how much space this menu entry requires.
    /// </summary>
    public virtual int GetHeight() {
      return Font.LineSpacing;
    }


    /// <summary>
    /// Queries how wide the entry is, used for centering on the screen.
    /// </summary>
    public virtual int GetWidth() {
      return (int)Font.MeasureString(Text).X;
    }
  }
}
