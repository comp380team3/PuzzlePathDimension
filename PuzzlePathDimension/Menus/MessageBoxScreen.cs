//-----------------------------------------------------------------------------
// MessageBoxScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  /// <summary>
  /// A popup message box screen, used to display "are you sure?"
  /// confirmation messages.
  /// </summary>
  class MessageBoxScreen : GameScreen {
    string message;
    Texture2D gradientTexture;
    SpriteFont font;
    MessageBoxTemplate messageBoxTemplate; //= new DetailsTemplate();

    //MenuButton confirmButton;
    //MenuButton cancelButton;


    public event EventHandler<PlayerIndexEventArgs> Accepted;
    public event EventHandler<PlayerIndexEventArgs> Cancelled;


    /// <summary>
    /// Constructor automatically includes the standard "A=ok, B=cancel"
    /// usage text prompt.
    /// </summary>
    public MessageBoxScreen(string message)
      : this("", true) {

        messageBoxTemplate = new MessageBoxTemplate(message);
    }

    /// <summary>
    /// Constructor lets the caller specify whether to include the standard
    /// "A=ok, B=cancel" usage text prompt.
    /// </summary>
    public MessageBoxScreen(string message, bool includeUsageText) {
      /*const string usageText = "\nA button, Space, Enter = ok" +
                               "\nB button, Esc = cancel";
      if (includeUsageText)
        this.message = message + usageText;
      else*/
        this.message = "";

      messageBoxTemplate = new MessageBoxTemplate(message);

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
      gradientTexture = shared.Load<Texture2D>("Texture/gradient");
      font = shared.Load<SpriteFont>("Font/menufont");

      messageBoxTemplate.Title = new TextLine(message, font, new Color(192, 192, 192));
      
      MenuButton confirmButton = new MenuButton("Confirm", font);
      confirmButton.Selected += ConfirmMenuEntrySelected;
      messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Right] = confirmButton;
      messageBoxTemplate.SelectedItem = MessageBoxTemplate.Selection.Right;

      MenuButton cancelButton = new MenuButton("Cancel", font);
      cancelButton.Selected += CancelMenuEntrySelected;
      messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Left] = cancelButton;
    }


    /// <summary>
    /// Responds to user input, accepting or cancelling the message box.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {
      if (vtroller.CheckForRecentRelease(VirtualButtons.Left)) {
        /*if (Accepted != null)
          Accepted(this, new PlayerIndexEventArgs(PlayerIndex.One));

        ExitScreen();*/

        messageBoxTemplate.SelectPrev();

      } 
      
      if (vtroller.CheckForRecentRelease(VirtualButtons.Right)) {
        /*if (Cancelled != null)
          Cancelled(this, new PlayerIndexEventArgs(PlayerIndex.One));

        ExitScreen();*/

        messageBoxTemplate.SelectNext();

      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        messageBoxTemplate.Confirm();
      } /*else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        OnCancel(null, new PlayerIndexEventArgs(PlayerIndex.One));
      }*/

    }

    void ConfirmMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      if (Accepted != null)
        Accepted(this, new PlayerIndexEventArgs(PlayerIndex.One));

      ExitScreen();
    }

    void CancelMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      if (Cancelled != null)
        Cancelled(this, new PlayerIndexEventArgs(PlayerIndex.One));

      ExitScreen();
    }

    /// <summary>
    /// Draws the message box.
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);
     /* // Darken down any other screens that were drawn beneath the popup.
      spriteBatch.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

      // Center the message text in the viewport.
      Viewport viewport = spriteBatch.GraphicsDevice.Viewport;
      Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
      Vector2 textSize = font.MeasureString(message);
      Vector2 textPosition = (viewportSize - textSize) / 2;

      // The background includes a border somewhat larger than the text itself.
      const int hPad = 32;
      const int vPad = 16;

      Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                    (int)textPosition.Y - vPad,
                                                    (int)textSize.X + hPad * 2,
                                                    (int)textSize.Y + vPad * 2);
      */
      // Fade the popup alpha during transitions.
      Color color = Color.White * TransitionAlpha;

      messageBoxTemplate.Draw(spriteBatch, gameTime, font, TransitionAlpha, color, gradientTexture);

     /* spriteBatch.Begin();

      // Draw the background rectangle.
      spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

      // Draw the message box text.
      spriteBatch.DrawString(font, message, textPosition, color);

      spriteBatch.End();*/
    }
  }
}
