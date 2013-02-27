using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzlePathDimension
{
    class Platform
    {
        public Texture2D PlatformTexture;

        public Vector2 Position;

        public int Height
        {
            get { return PlatformTexture.Height; }
        }

        public int Width
        {
            get { return PlatformTexture.Width; }
        }

        public bool Active;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            PlatformTexture = texture;

            Position = position;

            Active = true;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlatformTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
