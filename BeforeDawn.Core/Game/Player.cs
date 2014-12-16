using System;
using System.Diagnostics;
using BeforeDawn.Core.Adapters.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    class Player : Sprite
    {
        private readonly IContentManagerAdapter _contentManager;
        private const int Height = 50;
        private const int Width = 50;
        private readonly Rectangle _facingNorthTextureOffset;
        private readonly Rectangle _facingSouthTextureOffset;
        private readonly Rectangle _facingWestTextureOffset;
        private readonly Rectangle _facingEastTextureOffset;
        private const float VelocityX = 10;
        private const float VelocityY = 10;

        public Player(IContentManagerAdapter contentManager)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            _contentManager = contentManager;

            _facingNorthTextureOffset = new Rectangle(0, 0, Width, Height);
            _facingWestTextureOffset = new Rectangle(50, 0, Width, Height);
            _facingEastTextureOffset = new Rectangle(100, 0, Width, Height);
            _facingSouthTextureOffset = new Rectangle(150, 0, Width, Height);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                SourceRectangle = new Rectangle(_facingNorthTextureOffset.X, _facingNorthTextureOffset.Y,
                    _facingNorthTextureOffset.Width, _facingNorthTextureOffset.Height);

                Location = new Vector2(Location.X, Location.Y - VelocityY);
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                SourceRectangle = new Rectangle(_facingWestTextureOffset.X, _facingWestTextureOffset.Y,
                    _facingWestTextureOffset.Width, _facingWestTextureOffset.Height);

                Location = new Vector2(Location.X - VelocityX, Location.Y);
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                SourceRectangle = new Rectangle(_facingSouthTextureOffset.X, _facingSouthTextureOffset.Y,
                    _facingSouthTextureOffset.Width, _facingSouthTextureOffset.Height);

                Location = new Vector2(Location.X, Location.Y + VelocityY);
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                SourceRectangle = new Rectangle(_facingEastTextureOffset.X, _facingEastTextureOffset.Y,
                    _facingEastTextureOffset.Width, _facingEastTextureOffset.Height);

                Location = new Vector2(Location.X + VelocityX, Location.Y);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location, SourceRectangle, Color.White);
        }

        public Player Initialize(Vector2 location)
        {
            var texture = _contentManager.Load<Texture2D>("Player\\Player");
            SetDefaultValues(texture, location);
            return this;
        }

        public override string ToString()
        {
            return String.Format("Texture.Bounds.X: {0}, Texture.Bounds.Y:{1}, Location.X:{2}, Location.Y:{3}",
                Texture.Bounds.X, Texture.Bounds.Y, Location.X, Location.Y);
        }
    }
}
