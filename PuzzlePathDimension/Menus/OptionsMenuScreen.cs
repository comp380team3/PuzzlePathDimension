//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;

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

    MenuEntry soundMenuEntry = new MenuEntry(string.Empty);
    MenuEntry controllerConfigurationMenuEntry = new MenuEntry(string.Empty);
    MenuEntry back = new MenuEntry("back");

    /// <summary>
    /// Constructor.
    /// </summary>
    public OptionsMenuScreen()
        : base("Options") {
      SetMenuEntryText();

      soundMenuEntry.Selected += SoundMenuEntrySelected;
      controllerConfigurationMenuEntry.Selected += ControllerConfigurationMenuEntrySelected;
      back.Selected += OnCancel;

      MenuEntries.Add(soundMenuEntry);
      MenuEntries.Add(controllerConfigurationMenuEntry);
      MenuEntries.Add(back);
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
