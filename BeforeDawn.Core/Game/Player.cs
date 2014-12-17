using System;
using System.Diagnostics;
using System.Linq;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    class Player : Sprite
    {
        private readonly IContentManagerAdapter _contentManager;
        private readonly ILevelState _levelState;
        private const int Height = 50;
        private const int Width = 50;
        private readonly Rectangle _facingNorthTextureOffset;
        private readonly Rectangle _facingSouthTextureOffset;
        private readonly Rectangle _facingWestTextureOffset;
        private readonly Rectangle _facingEastTextureOffset;
        private const float VelocityX = 50;
        private const float VelocityY = 50;
        private const int MovementSpeed = 250;
        private int _aggregatedGameTime;

        public Player(IContentManagerAdapter contentManager, ILevelState levelState)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            if (levelState == null) throw new ArgumentNullException("levelState");

            _contentManager = contentManager;
            _levelState = levelState;

            _facingNorthTextureOffset = new Rectangle(0, 0, Width, Height);
            _facingWestTextureOffset = new Rectangle(50, 0, Width, Height);
            _facingEastTextureOffset = new Rectangle(100, 0, Width, Height);
            _facingSouthTextureOffset = new Rectangle(150, 0, Width, Height);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _aggregatedGameTime += gameTime.ElapsedGameTime.Milliseconds;
            
            if (_aggregatedGameTime > MovementSpeed)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    FaceUp();
                    TryMoveUp();
                }
                else if (keyboardState.IsKeyDown(Keys.A))
                {
                    FaceLeft();
                    TryMoveLeft();
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    FaceDown();
                    TryMoveDown();
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    FaceRight();
                    TryMoveRight();
                }

                _aggregatedGameTime = 0;
            }
        }

        private void TryMoveRight()
        {
            TryMoveToLocation(new Vector2(Location.X + VelocityX, Location.Y));
        }

        private void TryMoveDown()
        {
            TryMoveToLocation(new Vector2(Location.X, Location.Y + VelocityY));
        }

        private void TryMoveLeft()
        {
            TryMoveToLocation(new Vector2(Location.X - VelocityX, Location.Y));
        }

        private void TryMoveUp()
        {
            TryMoveToLocation(new Vector2(Location.X, Location.Y - VelocityY));
        }

        private void TryMoveToLocation(Vector2 location)
        {
            if (CanMoveTo(location))
            {
                Location = location;
            }
        }

        private bool CanMoveTo(Vector2 location)
        {
            return CanMoveTo(new Rectangle((int) location.X, (int) location.Y, Boundaries.Width, Boundaries.Height));
        }

        private bool CanMoveTo(Rectangle location)
        {
            return !_levelState.Tiles.Where(tile => tile.IsBlockTile || tile.Collision == TileCollision.Impassable).Any(tile => tile.Boundaries.Intersects(location));
        }

        private void FaceRight()
        {
            SourceRectangle = new Rectangle(_facingEastTextureOffset.X, _facingEastTextureOffset.Y,
                _facingEastTextureOffset.Width, _facingEastTextureOffset.Height);
        }

        private void FaceDown()
        {
            SourceRectangle = new Rectangle(_facingSouthTextureOffset.X, _facingSouthTextureOffset.Y,
                _facingSouthTextureOffset.Width, _facingSouthTextureOffset.Height);
        }

        private void FaceLeft()
        {
            SourceRectangle = new Rectangle(_facingWestTextureOffset.X, _facingWestTextureOffset.Y,
                _facingWestTextureOffset.Width, _facingWestTextureOffset.Height);
        }

        private void FaceUp()
        {
            SourceRectangle = new Rectangle(_facingNorthTextureOffset.X, _facingNorthTextureOffset.Y,
                _facingNorthTextureOffset.Width, _facingNorthTextureOffset.Height);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location, SourceRectangle, Color.White);
        }

        public Player Initialize(Vector2 location)
        {
            var texture = _contentManager.Load<Texture2D>("Player\\Player");
            SetDefaultValues(texture, location, new Rectangle((int)location.X, (int)location.Y, Width, Height));
            FaceRight();
            return this;
        }

        public override string ToString()
        {
            return String.Format("Texture.Bounds.X: {0}, Texture.Bounds.Y:{1}, Location.X:{2}, Location.Y:{3}",
                Texture.Bounds.X, Texture.Bounds.Y, Location.X, Location.Y);
        }
    }
}
