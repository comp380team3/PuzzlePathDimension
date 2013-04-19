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

    /// <summary>
    /// The left button's text of the Message Box.
    /// </summary>
    public string LeftButtonText {
      // check for null? - Jorenz
      get; //{ return messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Left].Text;  }
      set; //{ messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Left].Text = value; }
    }

    /// <summary>
    /// The middle button's text of the Message Box.
    /// </summary>
    public string MiddleButtonText {
      // check for null? - Jorenz
      get; //{ return messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Middle].Text; }
      set; //{ messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Middle].Text = value; }
    }

    /// <summary>
    /// The right button's text of the Message Box.
    /// </summary>
    public string RightButtonText {
      // check for null? - Jorenz
      get; //{ return messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Right].Text; }
      set; //{ messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Right].Text = value; }
    }

    public event EventHandler<PlayerIndexEventArgs> Accepted;
    public event EventHandler<PlayerIndexEventArgs> Cancelled;


    /// <summary>
    /// Constructor automatically includes the standard "A=ok, B=cancel"
    /// usage text prompt.
    /// </summary>
    public MessageBoxScreen(string message)
      : this("", true) {

        this.message = message; // Added. - Jorenz

        messageBoxTemplate = new MessageBoxTemplate(message);
        LeftButtonText = "Cancel";
        MiddleButtonText = "";
        RightButtonText = "Confirm";
    }

    /// <summary>
    /// Constructor lets the caller specify whether to include the standard
    /// "A=ok, B=cancel" usage text prompt.
    /// </summary>
    public MessageBoxScreen(string message, bool includeUsageText) {

      this.message = message;

      messageBoxTemplate = new MessageBoxTemplate(message);
      LeftButtonText = "Cancel";
      MiddleButtonText = "";
      RightButtonText = "Confirm";

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

        MenuButton confirmButton = new MenuButton(RightButtonText, font);
        confirmButton.Selected += ConfirmMenuEntrySelected;
        messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Right] = confirmButton;
        messageBoxTemplate.SelectedItem = MessageBoxTemplate.Selection.Right;

      MenuButton cancelButton = new MenuButton(LeftButtonText, font);
      cancelButton.Selected += CancelMenuEntrySelected;
      messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Left] = cancelButton;
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      messageBoxTemplate.TransitionPosition = TransitionPosition;
      messageBoxTemplate.Update(gameTime);
    }

    /// <summary>
    /// Responds to user input, accepting or cancelling the message box.
    /// </summary>
    public override void HandleInput(VirtualController vtroller) {
      if (vtroller.CheckForRecentRelease(VirtualButtons.Left)) {
        
        messageBoxTemplate.SelectPrev();

      } 
      
      if (vtroller.CheckForRecentRelease(VirtualButtons.Right)) {

        messageBoxTemplate.SelectNext();

      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        messageBoxTemplate.Confirm();
      } 
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

      // Fade the popup alpha during transitions.
      Color color = Color.White * TransitionAlpha;

      messageBoxTemplate.Draw(spriteBatch, gameTime, font, TransitionAlpha, color, gradientTexture);
    }
  }
}
