using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Goal class represents the goal of a level.
  /// </summary>
  class Goal {
    /// <summary>
    /// The texture that the goal will be drawn with.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// The texture's color data.
    /// </summary>
    private Color[] _colorData;

    /// <summary>
    /// The position of the goal.
    /// </summary>
    private Vector2 _position;

    /// <summary>
    /// Whether the goal is active.
    /// </summary>
    private bool _active;

    /// <summary>
    /// Gets the position of the goal.
    /// </summary>
    public Vector2 Position {
      get { return _position; }
    }

    /// <summary>
    /// Gets the height of the goal.
    /// </summary>
    public int Height {
      get { return _texture.Height; }
    }

    /// <summary>
    /// Gets the width of the goal.
    /// </summary>
    public int Width {
      get { return _texture.Width; }
    }

    /// <summary>
    /// Gets whether the goal is active.
    /// </summary>
    public bool Active {
      get { return _active; }
    }

    /// <summary>
    /// Gets the texture's color data.
    /// </summary>
    /// <returns>The texture's color data as an array.</returns>
    public Color[] GetColorData() {
      // See http://msdn.microsoft.com/en-us/library/0fss9skc.aspx for why
      // this is not a property.
      return (Color[])_colorData.Clone();
    }

    /// <summary>
    /// Initializes a goal.
    /// </summary>
    /// <param name="texture">The texture that the goal will be drawn with.</param>
    /// <param name="position">The position of the goal.</param>
    public void Initialize(Texture2D texture, Vector2 position) {
      _texture = texture;

      // Get the texture's color data, which is used for per-pixel collision
      _colorData = new Color[_texture.Width * _texture.Height];
      _texture.GetData<Color>(_colorData);

      _position = position;
      _active = true;
    }

    /// <summary>
    /// Updates the state of the goal.
    /// </summary>
    public void Update() {
    }

    /// <summary>
    /// Draws the goal to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the goal.</param>
    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
  }
}
