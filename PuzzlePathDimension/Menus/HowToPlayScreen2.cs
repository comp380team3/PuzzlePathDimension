using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuzzlePathDimension {
  class HowToPlayScreen2 : GameScreen {

    DetailsTemplate detailsTemplate = new DetailsTemplate();

    /// <summary>
    /// Next button entry on the screen.
    /// </summary>
    MenuButton nextMenuEntry;

    /// <summary>
    /// Back button entry on the screen.
    /// </summary>
    MenuButton backMenuEntry;

    /// <summary>
    /// Exit button entry on the screen.
    /// </summary>
    MenuButton exitMenuEntry;

    /// <summary>
    /// Font to write to Draw on the Screen.
    /// </summary>
    SpriteFont Font;

    /// <summary>
    /// Names of objects in the game.
    /// </summary>
    string[] gameObjects = new string[] {
      "Ball: ",
      "Platform: ",
      "Breakable Platform: ",
      "Goal: ",
      "DeathTrap: ",
      "Treasure: ",
    };

    /// <summary>
    /// Names of the Textures that correspond the objects of the game.
    /// </summary>
    string[] gameContent = new string[] {
      "Texture/ball",
      "Texture/platform",
      "Texture/platform_breakable",
      "Texture/goal",
      "Texture/deathtrap",
      "Texture/treasure",
    };

    /// <summary>
    /// List of game object Textures.
    /// </summary>
    List<Texture2D> objectContent = new List<Texture2D>();



    /// <summary>
    /// Contructor
    /// </summary>
    public HowToPlayScreen2() {
      base.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      base.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    /// <summary>
    /// Load the content that will be used to create the help screen.
    /// </summary>
    /// <param name="shared"></param>
    public override void LoadContent(ContentManager shared) {
      base.LoadContent(shared);
      Font = shared.Load<SpriteFont>("Font/menufont");

      foreach (string name in gameContent) {
        objectContent.Add(shared.Load<Texture2D>(name));
      }

      detailsTemplate.Title = new TextLine("Game Objects", Font, new Color(192, 192, 192));

      IList<IMenuLine> lines = detailsTemplate.Lines;
      lines.Clear();

      for (int i = 0; i < gameObjects.Length; ++i) {
        string text = gameObjects[i];
        Texture2D image = objectContent[i];

        TextLine caption = new TextLine(text, Font, Color.Black, 0.75f);
        caption.Align = TextAlignment.LEFT;
        lines.Add(new ImageMenuLine(image, caption));
      }

      nextMenuEntry = new MenuButton("Next", Font);
      nextMenuEntry.Selected += NextMenuEntrySelected;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Right] = nextMenuEntry;
      detailsTemplate.SelectedItem = DetailsTemplate.Selection.Right;

      backMenuEntry = new MenuButton("Back", Font);
      backMenuEntry.Selected += BackMenuEntrySelected;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Left] = backMenuEntry;

      exitMenuEntry = new MenuButton("Exit", Font);
      exitMenuEntry.Selected += OnCancel;
      detailsTemplate.Buttons[DetailsTemplate.Selection.Middle] = exitMenuEntry;

      Controller.ButtonReleased += OnButtonReleased;
    }

    private void OnButtonReleased(VirtualButtons button) {
      switch (button) {
      case VirtualButtons.Left:
        detailsTemplate.SelectPrev();
        break;
      case VirtualButtons.Right:
        detailsTemplate.SelectNext();
        break;
      case VirtualButtons.Confirm:
        detailsTemplate.Confirm();
        break;
      case VirtualButtons.Back:
        OnCancel();
        break;
      }
    }

    /// <summary>
    /// Update the Screen.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="otherScreenHasFocus"></param>
    /// <param name="coveredByOtherScreen"></param>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      detailsTemplate.TransitionPosition = TransitionPosition;
      detailsTemplate.Update(gameTime);
    }

    /// <summary>
    /// Draw the descriptions of the game objects and the Textures that correspond to those objects.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      base.Draw(gameTime, spriteBatch);

      detailsTemplate.Draw(spriteBatch, gameTime);
    }


    /// <summary>
    /// Event handler for when the Next menu entry is selected.
    /// </summary>
    void NextMenuEntrySelected() {
      ExitScreen();
      ScreenList.AddScreen(new HowToPlayScreen3());
    }

    /// <summary>
    /// Event handler for when the Back menu entry is selected.
    /// </summary>
    void BackMenuEntrySelected() {
      ExitScreen();
      ScreenList.AddScreen(new HowToPlayScreen1());
    }

    /// <summary>
    /// Event handler for when the Exit menu entry is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnCancel() {
      ExitScreen();
    }
  }
}
