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
      set {
        _origin = value;
        _center = CalculateCenter();

        if (_body != null) {
          _body.Position = UnitConverter.ToMeters(_center);
        }
      }
    }

    /************************
     * Brian's Physics stuff*
     * *********************/

    /// <summary>
    /// The Body that represents this platform in the physics simulation.
    /// </summary>
    private Body _body;

    /// <summary>
    /// The center of the platform, in pixels.
    /// </summary>
    private Vector2 _center;

    /// <summary>
    /// Gets the center of the platform, in pixels.
    /// </summary>
    public Vector2 Center {
      get { return _center; }
    }

    /// <summary>
    /// The size of the platform, in pixels.
    /// </summary>
    private Vector2 _size;

    /// <summary>
    /// Gets or sets the size of the platform, in pixels.
    /// </summary>
    public Vector2 Size {
      get { return _size; }
      set { 
        _size = value;

        if (_body != null) {
          // TODO: replace the Body's fixture
        }
      }
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <param name="breakable"></param>
    public Platform(Texture2D texture, Vector2 position, Vector2 size, bool breakable) {
      _origin = position;
      _size = size;
      _center = CalculateCenter();

      _texture = texture;
      _visible = true;

      _breakable = breakable;

      _body = null;
    }

    /// <summary>
    /// 
    /// </summary>
    private Vector2 CalculateCenter() {
      Vector2 center = new Vector2();
      center.X = _origin.X + (_size.X / 2.0f);
      center.Y = _origin.Y + (_size.Y / 2.0f);
      return center;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="world"></param>
    public void InitBody(World world) {
      if (_body != null) {
        // TODO: throw an exception
        Console.WriteLine("There is already a Body object for this platform.");
      }

      Vector2 meterSize = UnitConverter.ToMeters(_size);

      _body = BodyFactory.CreateRectangle(world, meterSize.X, meterSize.Y, 1);
      _body.Position = UnitConverter.ToMeters(_center);
      _body.BodyType = BodyType.Static;
      _body.Friction = 0f;
      _body.Restitution = .8f;
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
