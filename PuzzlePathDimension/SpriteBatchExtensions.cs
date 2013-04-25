using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension {
  public static class SpriteBatchExtensions {
    /// <summary>
    /// Helper draws a translucent black fullscreen sprite, used for fading
    /// screens in and out, and for darkening the background behind popups.
    /// </summary>
    public static void FadeBackBufferToBlack(this SpriteBatch spriteBatch, float alpha) {
      GraphicsDevice device = spriteBatch.GraphicsDevice;
      Viewport viewport = device.Viewport;

      Texture2D blankTexture = new Texture2D(device, 1, 1);
      blankTexture.SetData<Color>(new Color[] { Color.White });

      spriteBatch.Begin();

      spriteBatch.Draw(blankTexture,
                       new Rectangle(0, 0, viewport.Width, viewport.Height),
                       Color.Black * alpha);

      spriteBatch.End();
    }
  }
}