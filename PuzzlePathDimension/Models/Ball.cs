﻿using System;
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

    /// <summary>
    /// Gets the texture that the ball will be drawn with.
    /// </summary>
    public Texture2D Texture {
      get { return _texture; }
    }

    /************************
     * Brian's Physics stuff*
     * *********************/

    /// <summary>
    /// The Body that represents the ball in the physics simulation.
    /// </summary>
    private Body _body;

    /// <summary>
    /// Gets or sets the center of the ball, which is its current position, in pixels.
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

    public Vector2 Velocity {
      get { return _body.LinearVelocity; }
    }

    /// <summary>
    /// Constructs a Ball object.
    /// </summary>
    /// <param name="texture">The texture that the ball will be drawn with.</param>
    public Ball(Texture2D texture) {
      // Set the texture of the ball.
      _texture = texture;

      // Check to make sure that the visual representation of the texture is actually the right
      // size, and print a warning to the console if that isn't the case.
      if (_texture != null && _texture.Width * _texture.Height != _width * _height) {
        Console.WriteLine("Warning: the treasure's texture does not have the expected dimensions.");
        Console.WriteLine("Expected: " + _width + ", " + _height);
        Console.WriteLine("...but the texture is: " + _texture.Width + ", " + _texture.Height);
      }

      // Leave the Body object uninitialized until a World object comes by to initialize it.
      _body = null;
    }

    /// <summary>
    /// Initializes the ball's Body object.
    /// </summary>
    /// <param name="world">The World object that the ball will be a part of.</param>
    public void InitBody(World world) {
      if (_body != null) {
        throw new InvalidOperationException("There is already a Body object for the ball.");
      }
      // Obtain the radius of the ball, in meters.
      float radius = UnitConverter.ToMeters(_width / 2);

      // Create the Body object.
      _body = BodyFactory.CreateCircle(world, radius, 1);
      // Until the ball is launched, allow the ball's position to be set manually by the launcher.
      // That is, don't let the World affect the ball's position.
      _body.BodyType = BodyType.Static;
      // Set other properties of the ball's Body object.
      _body.Restitution = .8f;
      _body.Inertia = 0f;
      _body.Friction = 0f;
      // Slows down the ball over time.
      _body.LinearDamping = .1f;
      // Mark the body as belonging to a ball.
      _body.UserData = "ball";
    }

    /// <summary>
    /// Launches the ball with a given velocity.
    /// </summary>
    /// <param name="velX">The horizontal component of the velocity.</param>
    /// <param name="velY">The vertical component of the velocity.</param>
    public void Launch(float velX, float velY) {
      if (_body == null) {
        throw new InvalidOperationException("Call InitBody() on the Ball object first.");
      }
      // Let the ball be subjected to the World's physical forces.
      _body.BodyType = BodyType.Dynamic;
      // Propel the ball!
      _body.LinearVelocity = new Vector2(velX, velY);
    }

    /// <summary>
    /// Stops the ball.
    /// </summary>
    public void Stop() {
      if (_body == null) {
        throw new InvalidOperationException("Call InitBody() on the Ball object first.");
      }
      // Temporarily remove the ball from being checked by the World while changing the BodyType
      // back in order to avoid errors.
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
      // Draw the ball, using the center as the origin.
      Vector2 center = new Vector2((_width / 2.0f), (_height / 2.0f));
      Vector2 drawPos = UnitConverter.ToPixels(_body.Position);
      spriteBatch.Draw(_texture, drawPos, null, Color.White, 0f, center, 1f, SpriteEffects.None, 0f);
    }
  }
}
