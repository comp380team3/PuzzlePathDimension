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
    string message;
    Texture2D gradientTexture;
    List<Rectangle> positions;
    Texture2D platformTexture;

    MouseState previousMouseState;
    MouseState currentMouseState;
    Platform selected;
    public Platform Selected {
      get{ return selected; }
    }
    SpriteFont font;

    public event EventHandler<PlayerIndexEventArgs> Accepted;
    public event EventHandler<PlayerIndexEventArgs> Cancelled;


    /// <summary>
    /// Constructor automatically includes the standard "A=ok, B=cancel"
    /// usage text prompt.
    /// </summary>
    public ToolboxScreen(string message)
      : this(message, true) { }

    /// <summary>
    /// Constructor lets the caller specify whether to include the standard
    /// "A=ok, B=cancel" usage text prompt.
    /// </summary>
    public ToolboxScreen(string message, bool includeUsageText) {
      //if (includeUsageText)
        this.message = "Choose a platform to add(right click to cancel)";
      //else
      //  this.message = message;
        positions = new List<Rectangle>();
        positions.Add(new Rectangle(100, 100, 100, 20));
        positions.Add(new Rectangle(210, 100, 20, 100));
        previousMouseState = currentMouseState = Mouse.GetState();

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
      gradientTexture = shared.Load<Texture2D>("gradient");
      font = shared.Load<SpriteFont>("menufont");
      platformTexture = shared.Load<Texture2D>("platform");
    }


    /// <summary>
    /// Responds to user input, accepting or cancelling the message box.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {
      previousMouseState = currentMouseState;
      currentMouseState = Mouse.GetState();
      if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed) {
        foreach (Rectangle rect in positions) {
          if (currentMouseState.X > rect.X && currentMouseState.X < rect.X + rect.Width) {
            if (currentMouseState.Y > rect.Y && currentMouseState.Y < rect.Y + rect.Height) {
              selected = new Platform(platformTexture, new Vector2(rect.X, rect.Y), new Vector2(rect.Width, rect.Height), false);
              Console.WriteLine(selected.Origin);
              ExitScreen();
            }
          }
        }
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        if (Accepted != null)
          Accepted(this, new PlayerIndexEventArgs(PlayerIndex.One));

        ExitScreen();
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        if (Cancelled != null)
          Cancelled(this, new PlayerIndexEventArgs(PlayerIndex.One));

        ExitScreen();
      }
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
      Vector2 textSize = font.MeasureString(message);
      Vector2 textPosition = new Vector2(((viewportSize - textSize) / 2).X, 40);

      Rectangle backgroundRectangle = new Rectangle(20, 20, Simulation.FieldWidth - 40,
                                                    Simulation.FieldHeight - 40);

      // Fade the popup alpha during transitions.
      Color color = Color.White * TransitionAlpha;

      

      spriteBatch.Begin();

     

      // Draw the background rectangle.
      spriteBatch.Draw(gradientTexture, backgroundRectangle, color);
      
      foreach (Rectangle rect in positions) {
        Vector2 scale = new Vector2(rect.Width / (float)platformTexture.Width, rect.Height / (float)platformTexture.Height);
        spriteBatch.Draw(platformTexture, new Vector2(rect.X, rect.Y), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
      }
      // Draw the message box text.
      spriteBatch.DrawString(font, message, textPosition, color);

      spriteBatch.End();
    }
  }
}
