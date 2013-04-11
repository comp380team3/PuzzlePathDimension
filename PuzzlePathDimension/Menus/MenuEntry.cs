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
  class MenuEntry : IMenuEntry {
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
    public MenuEntry(string text) {
      Text = text;
    }


    /// <summary>
    /// Updates the menu entry.
    /// </summary>
    public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime) {
      // When the menu selection changes, entries gradually fade between
      // their selected and deselected appearance, rather than instantly
      // popping to the new state.
      float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

      if (isSelected)
        selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
      else
        selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
    }


    /// <summary>
    /// Draws the menu entry. This can be overridden to customize the appearance.
    /// </summary>
    public virtual void Draw(MenuScreen screen, SpriteBatch spriteBatch,
                             bool isSelected, GameTime gameTime) {
      // Draw the selected entry in yellow, otherwise white.
      Color color = isSelected ? Color.Yellow : Color.White;
      // Modify the alpha to fade text out during transitions.
      color *= screen.TransitionAlpha;

      // Pulsate the size of the selected menu entry.
      double time = gameTime.TotalGameTime.TotalSeconds;
      float pulsate = (float)Math.Sin(time * 6) + 1;
      float scale = 1 + pulsate * 0.05f * selectionFade;

      // Draw text, centered on the middle of each line.
      SpriteFont font = screen.TitleFont;
      Vector2 origin = new Vector2(0, font.LineSpacing / 2);

      spriteBatch.DrawString(font, Text, Position, color, 0,
                             origin, scale, SpriteEffects.None, 0);
    }


    /// <summary>
    /// Queries how much space this menu entry requires.
    /// </summary>
    public virtual int GetHeight(MenuScreen screen) {
      return screen.TitleFont.LineSpacing;
    }


    /// <summary>
    /// Queries how wide the entry is, used for centering on the screen.
    /// </summary>
    public virtual int GetWidth(MenuScreen screen) {
      return (int)screen.TitleFont.MeasureString(Text).X;
    }
  }
}
