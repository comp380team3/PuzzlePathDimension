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

    public const float unitToPixel = 100.0f;
    public const float pixelToUnit = 1 / unitToPixel;

    public Body body;
    public Vector2 Position {
      get { return body.Position * unitToPixel; }
      set { body.Position = value * pixelToUnit; }
    }
    private Vector2 size;
    public Vector2 Size {
      get { return size * unitToPixel; }
      set { size = value * pixelToUnit; }
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

    public Ball(World world, Texture2D texture, Vector2 size, float mass) {
      body = BodyFactory.CreateCircle(world, pixelToUnit * (texture.Width / 2), 1);
      body.BodyType = BodyType.Static;
      _origin = new Vector2((texture.Width / 2.0f), (texture.Height / 2.0f));
      body.Restitution = .8f;
      body.Inertia = 0f;
      body.Friction = 0f;
      this.Size = size;
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
      Vector2 scale = new Vector2(Size.X / (float)_texture.Width, Size.Y / (float)_texture.Height);
      spriteBatch.Draw(_texture, Position, null, Color.White, 0f, _origin, 1f, SpriteEffects.None, 0f);
    }
  }
}
