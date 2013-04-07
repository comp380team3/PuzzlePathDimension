using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace PuzzlePathDimension {
  public class Ball {
    /// <summary>
    /// The hard-coded height of the ball, in pixels.
    /// </summary>
    private const int _height = 25;

    /// <summary>
    /// The hard-coded width of the ball, in pixels.
    /// </summary>
    private const int _width = 25;

    /// <summary>
    /// The texture that the ball will be drawn with.
    /// </summary>
    private Texture2D _texture;


    private Vector2 _origin;

    /// <summary>
    /// Gets the texture that the ball will be drawn with.
    /// </summary>
    public Texture2D Texture {
      get { return _texture; }
    }

    /************************
     * Brian's Physics stuff*
     * *********************/

    public Body body;
    public Vector2 Center {
      get { return UnitConverter.ToPixels(body.Position); }
      set { body.Position = UnitConverter.ToMeters(value); }
    }
    private Vector2 _size;
    public Vector2 Size {
      get { return UnitConverter.ToPixels(_size); }
      set { _size = UnitConverter.ToMeters(value); }
    }

    /// <summary>
    /// Gets the height of the ball.
    /// </summary>
    public int Height {
      get { return _height; }
    }

    /// <summary>
    /// Gets the width of the ball.
    /// </summary>
    public int Width {
      get { return _width; }
    }

    public Ball(World world, Texture2D texture) {
      body = BodyFactory.CreateCircle(world, UnitConverter.ToMeters(_width / 2), 1);
      body.BodyType = BodyType.Static;
      _origin = new Vector2((_width / 2.0f), (_height / 2.0f));
      body.Restitution = .8f;
      body.Inertia = 0f;
      body.Friction = 0f;
      this.Size = _size;
      this._texture = texture;
    }

    public void Launch(float velX, float velY) {
      body.BodyType = BodyType.Dynamic;
      body.LinearVelocity = new Vector2(velX, velY);
    }

    public void Stop() {
      // Possible fix for the assertion failure? - Jorenz
      body.Enabled = false;
      body.BodyType = BodyType.Static;
      body.Enabled = true;
    }

    /*****************************
     * Brian's Physics stuff ends*
     * **************************/

    /// <summary>
    /// Draws the ball to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(_texture, Center, null, Color.White, 0f, _origin, 1f, SpriteEffects.None, 0f);
    }
  }
}
