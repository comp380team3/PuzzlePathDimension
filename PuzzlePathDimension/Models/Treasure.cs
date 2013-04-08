using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// The Treasure class contains a treasure's data.
  /// </summary>
  public class Treasure {
    /// <summary>
    /// The hard-coded height of a treasure, in pixels.
    /// </summary>
    private const int _height = 30;
    /// <summary>
    /// The hard-coded width of a treasure, in pixels.
    /// </summary>
    private const int _width = 30;

    /// <summary>
    /// The texture that the treasure will be drawn with.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// The Body that represents the treasure in the physics simulation.
    /// </summary>
    private Body _body;

    /// <summary>
    /// Whether the treasure has been collected.
    /// </summary>
    private bool _collected;

    /// <summary>
    /// The upper-left corner of the treasure, in pixels.
    /// </summary>
    private Vector2 _origin;

    /// <summary>
    /// Gets the upper-left corner of the treasure, in pixels.
    /// </summary>
    public Vector2 Origin {
      get { return _origin; }
      set {
        _origin = value;
        // Moving the treasure also moves its center, so figure out the position of
        // the new center.
        _center = CalculateCenter();

        // Reposition the Body, but only if it has been initialized.
        if (_body != null) {
          _body.Position = UnitConverter.ToMeters(_center);
        }
      }
    }

    /// <summary>
    /// The position of the center of the treasure, in pixels.
    /// </summary>
    private Vector2 _center;

    /// <summary>
    /// Gets the position of the center of the treasure, in pixels.
    /// </summary>
    public Vector2 Center {
      get { return _center; }
    }

    /// <summary>
    /// Gets the height of the treasure object.
    /// </summary>
    public int Height {
      get { return _height; }
    }

    /// <summary>
    /// Gets the width of the treasure object.
    /// </summary>
    public int Width {
      get { return _width; }
    }

    /// <summary>
    /// Gets whether the treasure has been collected.
    /// </summary>
    public bool Collected {
      get { return _collected; }
    }

    /// <summary>
    /// This delegate is called when the treasure is collected.
    /// </summary>
    public delegate void TreasureCollected();
    /// <summary>
    /// Occurs when the treasure is collected.
    /// </summary>
    public event TreasureCollected OnTreasureCollect;

    /// <summary>
    /// Constructs a Treasure object.
    /// </summary>
    /// <param name="texture">The texture to draw the treasure with.</param>
    /// <param name="position">The upper-left corner of the treasure, in pixels.</param>
    public Treasure(Texture2D texture, Vector2 position) {
      // Set the texture.
      _texture = texture;
      // Check to make sure that the visual representation of the texture is actually the right
      // size, and print a warning to the console if that isn't the case.
      if (_texture != null && _texture.Width * _texture.Height != _width * _height) {
        Console.WriteLine("Warning: the treasure's texture does not have the expected dimensions.");
        Console.WriteLine("Expected: " + _width + ", " + _height);
        Console.WriteLine("...but the texture is: " + _texture.Width + ", " + _texture.Height);
      }
      // Figure out the origin and center of the treasure.
      _origin = position;
      _center = CalculateCenter();
      // The treasure should be there at first.
      _collected = false;
      // Leave the Body object uninitialized until a World object comes by to initialize it.
      _body = null;
    }

    /// <summary>
    /// Calculates the center of the treasure.
    /// </summary>
    /// <returns>The position of the center of the treasure.</returns>
    private Vector2 CalculateCenter() {
      Vector2 center = new Vector2();
      center.X = _origin.X + (_width / 2.0f);
      center.Y = _origin.Y + (_height / 2.0f);
      return center;
    }

    /// <summary>
    /// Initializes the treasure's Body object.
    /// </summary>
    /// <param name="world">The World object that the treasure will be a part of.</param>
    public void InitBody(World world) {
      if (_body != null) {
        throw new InvalidOperationException("There is already a Body object for this treasure.");
      }
      // Obtain the radius of the treasure, in meters.
      float radius = UnitConverter.ToMeters(_width / 2);

      // Create the Body object.
      _body = BodyFactory.CreateCircle(world, radius, 1);
      // Set its position to be the center of the treasure, in meters, which is what the
      // physics engine expects.
      _body.Position = UnitConverter.ToMeters(_center);
      // The goal should never be subjected to the World's physical forces.
      _body.BodyType = BodyType.Static;
      // The ball should not actually bounce off the treasure.
      _body.FixtureList[0].IsSensor = true;
      // Associate the Body object with the treasure.
      _body.UserData = "treasure";
      // Listen for collision events.
      _body.OnCollision += new OnCollisionEventHandler(HandleCollision);
    }

    /// <summary>
    /// Called when a collision with the treasure occurs.
    /// </summary>
    /// <param name="fixtureA">The first fixture that has collided.</param>
    /// <param name="fixtureB">The second fixture that has collided.</param>
    /// <param name="contact">The Contact object that contains information about the collision.</param>
    /// <returns>Whether the collision should still happen.</returns>
    private bool HandleCollision(Fixture fixtureA, Fixture fixtureB, Contact contact) {
      // Check if one of the Fixtures belongs to a ball.
      bool causedByBall = (string)fixtureA.Body.UserData == "ball" || (string)fixtureB.Body.UserData == "ball";

      // Only mark the treasure as collected if a ball collided with it for the first time.
      // OnCollision doesn't get called again while the two objects are still touching, though.
      if (contact.IsTouching() && causedByBall && !_collected) {
        Collect();
      }
      // A treasure isn't an object that should be bounced off of, so don't actually
      // cause the collision to happen in the physics simulation.
      return false;
    }

    /// <summary>
    /// Marks a treasure as being collected.
    /// </summary>
    private void Collect() {
      _collected = true;

      // Call any methods that are listening to this event.
      if (OnTreasureCollect != null) {
        OnTreasureCollect();
      }
    }

    /// <summary>
    /// Resets the treasure's state.
    /// </summary>
    public void Reset() {
      _collected = false;
    }

    /// <summary>
    /// Draws the treasure to the screen.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch object to use when drawing the treasure.</param>
    public void Draw(SpriteBatch spriteBatch) {
      // Only draw the treasure if it hasn't been collected.
      if (!_collected) {
        // Draw the treasure, using its center as the origin to draw from.
        Vector2 origin = new Vector2((_width / 2.0f), (_height / 2.0f));
        spriteBatch.Draw(_texture, _center, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
      }
    }
  }
}
