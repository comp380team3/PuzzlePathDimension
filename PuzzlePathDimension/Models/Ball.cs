using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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


    private Vector2 _center;

    /// <summary>
    /// Gets the texture that the ball will be drawn with.
    /// </summary>
    public Texture2D Texture {
      get { return _texture; }
    }

    /************************
     * Brian's Physics stuff*
     * *********************/

    private Body _body;

    /// <summary>
    /// Gets or sets the center of the ball.
    /// </summary>
    public Vector2 Center {
      get { return UnitConverter.ToPixels(_body.Position); }
      set { _body.Position = UnitConverter.ToMeters(value); }
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

    public Ball(Texture2D texture) {
      this._texture = texture;
      _body = null;
    }

    public void InitBody(World world) {
      if (_body != null) {
        // TODO: throw an exception
        Console.WriteLine("There is already a Body object for this ball.");
      }

      _body = BodyFactory.CreateCircle(world, UnitConverter.ToMeters(_width / 2), 1);
      _body.BodyType = BodyType.Static;
      _center = new Vector2((_width / 2.0f), (_height / 2.0f));
      _body.Restitution = .8f;
      _body.Inertia = 0f;
      _body.Friction = 0f;
    }

    public void Launch(float velX, float velY) {
      _body.BodyType = BodyType.Dynamic;
      _body.LinearVelocity = new Vector2(velX, velY);
    }

    public void Stop() {
      _body.Enabled = false;
      _body.BodyType = BodyType.Static;
      _body.Enabled = true;
    }

    /*****************************
     * Brian's Physics stuff ends*
     * **************************/

    /// <summary>
    /// Draws the ball to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the ball.</param>
    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Draw(_texture, Center, null, Color.White, 0f, _center, 1f, SpriteEffects.None, 0f);
    }
  }
}
