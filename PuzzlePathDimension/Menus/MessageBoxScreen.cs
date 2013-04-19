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
    MessageBoxTemplate messageBoxTemplate;

    /// <summary>
    /// The left button's text of the Message Box.
    /// </summary>
    public string LeftButtonText { get; set; }

    /// <summary>
    /// The middle button's text of the Message Box.
    /// </summary>
    public string MiddleButtonText { get; set; }

    /// <summary>
    /// The right button's text of the Message Box.
    /// </summary>
    public string RightButtonText { get; set; }

    public event EventHandler<PlayerIndexEventArgs> Accepted;
    public event EventHandler<PlayerIndexEventArgs> Cancelled;


    /// <summary>
    /// Constructor automatically includes the standard "A=ok, B=cancel"
    /// usage text prompt.
    /// </summary>
    public MessageBoxScreen(string message) {

        this.message = message; // Added. - Jorenz

        messageBoxTemplate = new MessageBoxTemplate(message);
        LeftButtonText = "Cancel";
        MiddleButtonText = null;
        RightButtonText = "Confirm";

        base.IsPopup = true;
        base.TransitionOnTime = TimeSpan.FromSeconds(0.2);
        base.TransitionOffTime = TimeSpan.FromSeconds(0.2);
    }

    /// <summary>
    /// Constructor lets the caller specify whether to include the standard
    /// "A=ok, B=cancel" usage text prompt.
    /// </summary>
    public MessageBoxScreen(string message, string leftButtonText, string middleButtonText, string rightButtonText) {

      this.message = message;

      messageBoxTemplate = new MessageBoxTemplate(message);
      LeftButtonText = leftButtonText;
      MiddleButtonText = middleButtonText;
      RightButtonText = rightButtonText;

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

      if (LeftButtonText != null) {
        MenuButton leftButton = new MenuButton(LeftButtonText, font);
        leftButton.Selected += CancelMenuEntrySelected;
        messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Left] = leftButton;
      }

      if (MiddleButtonText != null) {
        MenuButton centerButton = new MenuButton(MiddleButtonText, font);
        centerButton.Selected += CancelMenuEntrySelected;
        messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Middle] = centerButton;
      }

      if (RightButtonText != null) {
        MenuButton rightButton = new MenuButton(RightButtonText, font);
        rightButton.Selected += ConfirmMenuEntrySelected;
        messageBoxTemplate.Buttons[MessageBoxTemplate.Selection.Right] = rightButton;
        messageBoxTemplate.SelectedItem = MessageBoxTemplate.Selection.Right;
      }
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
