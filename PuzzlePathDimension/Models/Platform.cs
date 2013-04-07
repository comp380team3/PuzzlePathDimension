using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Platform class describes a platform.
  /// </summary>
  class Platform {
    /// <summary>
    /// The texture that the platform uses.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// Whether the platform is visible.
    /// </summary>
    private bool _visible;

    /// <summary>
    /// Whether the platform is breakable.
    /// </summary>
    private bool _breakable;

    /// <summary>
    /// The upper-left corner of the platform, in pixels.
    /// </summary>
    private Vector2 _origin;

    /// <summary>
    /// Gets the upper-left corner of the platform, in pixels.
    /// </summary>
    public Vector2 Origin {
      get { return _origin; }
    }

    /************************
     * Brian's Physics stuff*
     * *********************/

    /// <summary>
    /// The Body that represents this platform in the physics simulation.
    /// </summary>
    private Body _body;

    /// <summary>
    /// Gets or sets the center of the platform, in pixels.
    /// </summary>
    public Vector2 Center {
      get { return UnitConverter.ToPixels(_body.Position); }
      set { _body.Position = UnitConverter.ToMeters(value); }
    }

    /// <summary>
    /// The size of the platform, in pixels.
    /// </summary>
    private Vector2 _pixelSize;

    /// <summary>
    /// The size of the platform, in meters.
    /// </summary>
    private Vector2 _meterSize;

    /// <summary>
    /// Gets or sets the size of the platform, in pixels.
    /// </summary>
    public Vector2 Size {
      get { return UnitConverter.ToPixels(_meterSize); }
      set { _meterSize = UnitConverter.ToMeters(value); }
    }

    /// <summary>
    /// Gets whether the platform is visible.
    /// </summary>
    public bool Visible {
      get { return _visible; }
      set { _visible = value; }
    }

    /// <summary>
    /// Gets whether the platform is breakable.
    /// </summary>
    public bool Breakable {
      get { return _breakable; }
    }

    public Platform(Texture2D texture, Vector2 size, Vector2 position, bool breakable) {
      _origin = position;
      _pixelSize = size;
      _meterSize = UnitConverter.ToMeters(size);

      _texture = texture;
      _visible = true;

      _breakable = breakable;

      _body = null;
    }

    public void InitBody(World world) {
      if (_body != null) {
        // TODO: throw an exception
        Console.WriteLine("There is already a Body object for this platform.");
      }

      _body = BodyFactory.CreateRectangle(world, _meterSize.X, _meterSize.Y, 1);
      _body.BodyType = BodyType.Static;
      _body.Friction = 0f;
      _body.Restitution = .8f;

      Vector2 center = new Vector2();
      center.X = _origin.X + (_pixelSize.X / 2.0f);
      center.Y = _origin.Y + (_pixelSize.Y / 2.0f);

      _body.Position = UnitConverter.ToMeters(center);
    }

    /*****************************
     * Brian's Physics stuff ends*
     * **************************/

    /// <summary>
    /// Draws the platform to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      if (_visible) {
        // Scale the texture to the appropriate size.
        Vector2 scale = new Vector2(Size.X / (float)_texture.Width, Size.Y / (float)_texture.Height);
        // Draw it!
        // TODO: replace hard-coded origin
        spriteBatch.Draw(_texture, Center, null, Color.White, 0f, new Vector2(10, 10), scale, SpriteEffects.None, 0f);
      }
    }

    /// <summary>
    /// Checks if the origin of the platform is within the level boundaries.
    /// </summary>
    /// <param name="v">The origin.</param>
    /// <returns>Whether the origin of the platform is inside the level.</returns>
    private bool InBounds(Vector2 v) {
      // Subtract 1 to account for the fact that the origin is at (0,0).
      return v.X >= 0 && v.X <= Simulation.FieldWidth - 1 && v.Y >= 0 && v.Y <= Simulation.FieldHeight - 1;
    }
  }
}
