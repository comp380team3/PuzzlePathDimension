//-----------------------------------------------------------------------------
// ToolboxScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace PuzzlePathDimension {
  /// <summary>
  /// A popup message box screen, used to display "are you sure?"
  /// confirmation messages.
  /// </summary>
  class ToolboxScreen : GameScreen {

    /// <summary>
    /// Message displayed at the top of Toolbox
    /// </summary>
    private string _message;

    /// <summary>
    /// Backgorund texture
    /// </summary>
    private Texture2D _gradientTexture;

    /// <summary>
    /// List of the position of platform  positions.
    /// </summary>
    private List<Rectangle> _platforms;

    /// <summary>
    /// List of the position of breakable platforms.
    /// </summary>
    private List<Rectangle> _breakablePlatforms;

    /// <summary>
    /// The dictionary that maps platform sizes to the appropriate texture.
    /// This dictionary is for regular platforms.
    /// </summary>
    Dictionary<Vector2, Texture2D> platformTextures;

    /// <summary>
    /// The dictionary that maps platform sizes to the appropriate texture.
    /// This dictionary is for breakable platforms.
    /// </summary>
    Dictionary<Vector2, Texture2D> breakablePlatformTextures;

    /// <summary>
    /// Boolean to determine if more platforms can be selected
    /// </summary>
    private Boolean _cantAdd;



    private SpriteFont _font;

    public event Action Accepted;

    private DeathTrap deathTrap;
    private Treasure treasure;

    ///// <summary>
    ///// Constructor automatically includes the standard "A=ok, B=cancel"
    ///// usage text prompt.
    ///// </summary>
    //public ToolboxScreen(TopLevelModel topLevel, string message)
    //  : this(topLevel, message, true) { }

    EditableLevel editableLevel;

    /// <summary>
    /// Constructor that includes a message to be displayed and Boolean to determine if more platforms can be added
    /// </summary>
    public ToolboxScreen(TopLevelModel topLevel, EditableLevel level, string message, bool limitReached)
      : base(topLevel) {
      this._message = message;
      _cantAdd = limitReached;
      //initializa the position of regular platforms
      _platforms = new List<Rectangle>();
      editableLevel = level;


      if (level.TypesAllowed.Contains("R")) {
        if (level.TypesAllowed.Contains("H")) {
          _platforms.Add(new Rectangle(100, 130, 100, 25));
          _platforms.Add(new Rectangle(300, 130, 150, 25));
          _platforms.Add(new Rectangle(500, 130, 200, 25));
        }
        if (level.TypesAllowed.Contains("V")) {
          _platforms.Add(new Rectangle(100, 210, 25, 100));
          _platforms.Add(new Rectangle(300, 210, 25, 150));
          _platforms.Add(new Rectangle(500, 210, 25, 200));
        }
      }
      //Initializes the position of breakable platforms.
      _breakablePlatforms = new List<Rectangle>();
      if (level.TypesAllowed.Contains("B")) {
        if (level.TypesAllowed.Contains("H")) {
          _breakablePlatforms.Add(new Rectangle(100, 160, 100, 25));
          _breakablePlatforms.Add(new Rectangle(300, 160, 150, 25));
          _breakablePlatforms.Add(new Rectangle(500, 160, 200, 25));
        }
        if (level.TypesAllowed.Contains("V")) {
          _breakablePlatforms.Add(new Rectangle(175, 210, 25, 100));
          _breakablePlatforms.Add(new Rectangle(425, 210, 25, 150));
          _breakablePlatforms.Add(new Rectangle(675, 210, 25, 200));
        }
      }



      base.IsPopup = true;
      base.TransitionOnTime = TimeSpan.FromSeconds(0.2);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.2);
    }

    /// <summary>
    /// Loads graphics content for this screen. This uses the shared ContentManager
    /// provided by the Game class, so the content will remain loaded forever.
    /// Whenever a subsequent MessageBoxScreen tries to load this same content,
    /// it will just get back another reference to the already loaded data.
    /// </summary>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);

      _gradientTexture = shared.Load<Texture2D>("Texture/gradient");
      _font = shared.Load<SpriteFont>("Font/menufont");
      // Create the dictionaries and cache all the textures needed to draw each
      // platform in the toolbox.
      platformTextures = new Dictionary<Vector2, Texture2D>();
      breakablePlatformTextures = new Dictionary<Vector2, Texture2D>();

      foreach (Vector2 size in Platform.NormalPlatNames.Keys) {
        platformTextures.Add(size, shared.Load<Texture2D>(Platform.NormalPlatNames[size]));
      }
      foreach (Vector2 size in Platform.BreakablePlatNames.Keys) {
        breakablePlatformTextures.Add(size, shared.Load<Texture2D>(Platform.BreakablePlatNames[size]));
      }
      if (editableLevel.Custom) {
        deathTrap = new DeathTrap(shared.Load<Texture2D>("Texture/deathtrap"), new Vector2(200, 400));
        treasure = new Treasure(shared.Load<Texture2D>("Texture/treasure"), new Vector2(300, 400));
      }
    }


    /// <summary>
    /// Responds to user input.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {
      if (Controller.IsButtonPressed(VirtualButtons.Select) && !_cantAdd) {
        Point pointer = Controller.Point;

        foreach (Rectangle rect in _platforms) {
          if (pointer.X > rect.X && pointer.X < rect.X + rect.Width) {
            if (pointer.Y > rect.Y && pointer.Y < rect.Y + rect.Height) {
              Texture2D textureToUse = platformTextures[new Vector2(rect.Width, rect.Height)];

              editableLevel.MoveablePlatforms.Add(new Platform(textureToUse, new Vector2(rect.X, rect.Y), new Vector2(rect.Width, rect.Height), false));
              ExitScreen();
            }
          }
        }

        foreach (Rectangle rect in _breakablePlatforms) {
          if (pointer.X > rect.X && pointer.X < rect.X + rect.Width) {
            if (pointer.Y > rect.Y && pointer.Y < rect.Y + rect.Height) {
              Texture2D textureToUse = breakablePlatformTextures[new Vector2(rect.Width, rect.Height)];

              editableLevel.MoveablePlatforms.Add(new Platform(textureToUse, new Vector2(rect.X, rect.Y), new Vector2(rect.Width, rect.Height), true));
              ExitScreen();
            }
          }
        }
        if (editableLevel.Custom) {
          if (Intersects(deathTrap, pointer)) {
            editableLevel.DeathTraps.Add(deathTrap);
            ExitScreen();
          }

          if (Intersects(treasure, pointer)) {
            editableLevel.Treasures.Add(treasure);
            ExitScreen();
          }
        }
      }

      if (_cantAdd && Controller.IsButtonPressed(VirtualButtons.Select)) {
        ExitScreen();
      }
    }

    protected override void OnButtonReleased(VirtualButtons button) {
      if (button == VirtualButtons.Delete) {
        if (Accepted != null)
          Accepted();

        ExitScreen();
      }
    }

    /// <summary>
    /// Determines if a mouse click intersects with a rectangle.
    /// </summary>
    /// <returns></returns>
    private Boolean Intersects(DeathTrap dt, Point ms) {
      if(Vector2.Distance(dt.Center, new Vector2(ms.X, ms.Y)) 
                < Vector2.Distance( dt.Center, dt.Center+new Vector2(0, dt.Width/2))){
        return true;
      }
      return false;
    }
    private Boolean Intersects(Treasure t, Point ms) {
      if (Vector2.Distance(t.Center, new Vector2(ms.X, ms.Y))
                < Vector2.Distance(t.Center, t.Center + new Vector2(0, t.Width / 2))) {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Draws the message box.
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      // Darken down any other screens that were drawn beneath the popup.
      spriteBatch.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

      // Center the message text in the viewport.
      Viewport viewport = spriteBatch.GraphicsDevice.Viewport;
      Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
      Vector2 textSize = _font.MeasureString(_message);
      Vector2 textPosition = new Vector2(((viewportSize - textSize) / 2).X, 40);

      Rectangle backgroundRectangle = new Rectangle(20, 20, Simulation.FieldWidth - 40,
                                                    Simulation.FieldHeight - 40);

      // Fade the popup alpha during transitions.
      Color color = Color.White * TransitionAlpha;



      spriteBatch.Begin();



      // Draw the background rectangle.
      spriteBatch.Draw(_gradientTexture, backgroundRectangle, color);


      // Draw the platforms in the toolbox, referring to the dictionaries to determine
      // what textures to draw.
      foreach (Rectangle rect in _platforms) {
        Texture2D textureToUse = platformTextures[new Vector2(rect.Width, rect.Height)];
        spriteBatch.Draw(textureToUse, new Vector2(rect.X, rect.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      }
      foreach (Rectangle rect in _breakablePlatforms) {
        Texture2D textureToUse = breakablePlatformTextures[new Vector2(rect.Width, rect.Height)];
        spriteBatch.Draw(textureToUse, new Vector2(rect.X, rect.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

      }

      if (editableLevel.Custom) {
        deathTrap.Draw(spriteBatch);
        treasure.Draw(spriteBatch);
      }

      // Draw the message box text.
      spriteBatch.DrawString(_font, _message, textPosition, color);

      spriteBatch.End();
    }
  }
}
