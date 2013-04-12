//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    MenuButton soundMenuEntry;
    MenuButton controllerConfigurationMenuEntry;
    MenuButton back;

    /// <summary>
    /// Constructor.
    /// </summary>
    public OptionsMenuScreen()
        : base("Options") {
    }

    public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager shared) {
      base.LoadContent(shared);
      SpriteFont font = shared.Load<SpriteFont>("menufont");

      soundMenuEntry = new MenuButton(string.Empty, font);
      soundMenuEntry.Selected += SoundMenuEntrySelected;
      MenuEntries.Add(soundMenuEntry);

      controllerConfigurationMenuEntry = new MenuButton(string.Empty, font);
      controllerConfigurationMenuEntry.Selected += ControllerConfigurationMenuEntrySelected;
      MenuEntries.Add(controllerConfigurationMenuEntry);

      back = new MenuButton("back", font);
      back.Selected += OnCancel;
      MenuEntries.Add(back);

      SetMenuEntryText();
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
