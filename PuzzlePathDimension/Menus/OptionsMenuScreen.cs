//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  /// <summary>
  /// The options screen is brought up over the top of the main menu
  /// screen, and gives the user a chance to configure the game
  /// in various hopefully useful ways.
  /// </summary>
  class OptionsMenuScreen : MenuScreen {
    static bool sound = true;
    static string[] controllerType = { "Keyboard/Mouse", "Xbox 360 Gamepad" };
    static int currentControllerType = 0;

    MenuTemplate menuTemplate = new MenuTemplate();

    MenuButton soundMenuEntry;
    MenuButton controllerConfigurationMenuEntry;
    MenuButton back;

    /// <summary>
    /// Constructor.
    /// </summary>
    public OptionsMenuScreen() : base("") {
    }

    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("menufont");

      menuTemplate.Title = new TextLine("Options", font, new Color(192, 192, 192));
      menuTemplate.Cancelled += OnCancel;


      IList<MenuButton> items = menuTemplate.Items;

      soundMenuEntry = new MenuButton(string.Empty, font);
      soundMenuEntry.Selected += SoundMenuEntrySelected;
      items.Add(soundMenuEntry);

      controllerConfigurationMenuEntry = new MenuButton(string.Empty, font);
      controllerConfigurationMenuEntry.Selected += ControllerConfigurationMenuEntrySelected;
      items.Add(controllerConfigurationMenuEntry);

      back = new MenuButton("back", font);
      back.Selected += OnCancel;
      items.Add(back);


      SetMenuEntryText();
    }

    public override void HandleInput(VirtualController vtroller) {
      if (vtroller.CheckForRecentRelease(VirtualButtons.Up)) {
        menuTemplate.SelectPrev();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Down)) {
        menuTemplate.SelectNext();
      }

      if (vtroller.CheckForRecentRelease(VirtualButtons.Confirm)) {
        menuTemplate.Confirm();
      } else if (vtroller.CheckForRecentRelease(VirtualButtons.Back)) {
        menuTemplate.Cancel();
      }
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      menuTemplate.TransitionPosition = TransitionPosition;
      menuTemplate.Update(true, gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      menuTemplate.Draw(spriteBatch, true, gameTime);
    }

    /// <summary>
    /// Fills in the latest values for the options screen menu text.
    /// </summary>
    void SetMenuEntryText() {
      soundMenuEntry.Text = "Sound: " + (sound ? "on" : "off");
      controllerConfigurationMenuEntry.Text = "Controller Type: " + controllerType[currentControllerType];
    }


    /// <summary>
    /// Event handler for when the Sound menu entry is selected.
    /// </summary>
    void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      sound = !sound;

      SetMenuEntryText();
    }

    /// <summary>
    /// Event handler for when the Controller Configuration menu entry is selected.
    /// </summary>
    void ControllerConfigurationMenuEntrySelected(object sender, PlayerIndexEventArgs e) {
      currentControllerType = (currentControllerType + 1) % controllerType.Length;

      SetMenuEntryText();
    }
  }
}
