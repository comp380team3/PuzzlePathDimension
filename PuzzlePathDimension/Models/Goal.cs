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
    /// The hard-coded height of the goal, in pixels.
    /// </summary>
    private const int _height = 60;
    /// <summary>
    /// The hard-coded width of the goal, in pixels.
    /// </summary>
    private const int _width = 60;

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
      get { return _height; }
    }

    /// <summary>
    /// Gets the width of the goal.
    /// </summary>
    public int Width {
      get { return _width; }
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

      // Check to make sure that the visual representation of the goal is actually the right
      // size, and print a warning to the console if that isn't the case.
      if (_texture != null && _texture.Width * _texture.Height != _width * _height) {
        Console.WriteLine("Warning: the goal's texture does not have the expected dimensions.");
        Console.WriteLine("Expected: " + _width + ", " + _height);
        Console.WriteLine("...but the texture is: " + _texture.Width + ", " + _texture.Height);
      }

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
