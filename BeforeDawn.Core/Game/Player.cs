using System;
using System.Diagnostics;
using System.Linq;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Game.Messages;
using BeforeDawn.Core.Game.Tiles;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    class Player : Sprite, IFocusable
    {
        private readonly IContentManagerAdapter _contentManager;
        private readonly ILevelState _levelState;
        private readonly IMessageBus _messageBus;
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
        private bool _bypassMovementSpeedLimit = false;

        public int Health { get; private set; }
        public Direction Direction { get; private set; }

        public Player(IContentManagerAdapter contentManager, ILevelState levelState, IMessageBus messageBus)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            if (levelState == null) throw new ArgumentNullException("levelState");
            if (messageBus == null) throw new ArgumentNullException("messageBus");

            _contentManager = contentManager;
            _levelState = levelState;
            _messageBus = messageBus;

            _facingNorthTextureOffset = new Rectangle(0, 0, Width, Height);
            _facingWestTextureOffset = new Rectangle(50, 0, Width, Height);
            _facingEastTextureOffset = new Rectangle(100, 0, Width, Height);
            _facingSouthTextureOffset = new Rectangle(150, 0, Width, Height);

            Health = 100;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _aggregatedGameTime += gameTime.ElapsedGameTime.Milliseconds;
            
            if (keyboardState.GetPressedKeys().Count() == 0)
            {
                _bypassMovementSpeedLimit = true;
            }
            
            if (_bypassMovementSpeedLimit || _aggregatedGameTime > MovementSpeed)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    FaceUp();
                    TryMoveUp();
                    RestrictMovementSpeed();
                }
                else if (keyboardState.IsKeyDown(Keys.A))
                {
                    FaceLeft();
                    TryMoveLeft();
                    RestrictMovementSpeed();
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    FaceDown();
                    TryMoveDown();
                    RestrictMovementSpeed();
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    FaceRight();
                    TryMoveRight();
                    RestrictMovementSpeed();
                }

                _aggregatedGameTime = 0;
            }
        }

        private void RestrictMovementSpeed()
        {
            _bypassMovementSpeedLimit = false;
        }

        private void TryMoveRight()
        {
            TryMoveToLocation(new Vector2(Position.X + VelocityX, Position.Y));
        }

        private void TryMoveDown()
        {
            TryMoveToLocation(new Vector2(Position.X, Position.Y + VelocityY));
        }

        private void TryMoveLeft()
        {
            TryMoveToLocation(new Vector2(Position.X - VelocityX, Position.Y));
        }

        private void TryMoveUp()
        {
            TryMoveToLocation(new Vector2(Position.X, Position.Y - VelocityY));
        }

        private void TryMoveToLocation(Vector2 location)
        {
            if (CanMoveTo(location))
            {
                Position = location;
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

            Direction = Direction.Right;
        }

        private void FaceDown()
        {
            SourceRectangle = new Rectangle(_facingSouthTextureOffset.X, _facingSouthTextureOffset.Y,
                _facingSouthTextureOffset.Width, _facingSouthTextureOffset.Height);

            Direction = Direction.Down;
        }

        private void FaceLeft()
        {
            SourceRectangle = new Rectangle(_facingWestTextureOffset.X, _facingWestTextureOffset.Y,
                _facingWestTextureOffset.Width, _facingWestTextureOffset.Height);

            Direction = Direction.Left;
        }

        private void FaceUp()
        {
            SourceRectangle = new Rectangle(_facingNorthTextureOffset.X, _facingNorthTextureOffset.Y,
                _facingNorthTextureOffset.Width, _facingNorthTextureOffset.Height);

            Direction = Direction.Up;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White);
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
            return String.Format("Texture.Bounds.X: {0}, Texture.Bounds.Y:{1}, Position.X:{2}, Position.Y:{3}",
                Texture.Bounds.X, Texture.Bounds.Y, Position.X, Position.Y);
        }

        public void GoToLocation(int x, int y)
        {
            TryMoveToLocation(new Vector2(x, y));
        }

        public void GoToTile(int tileLayoutX, int tileLayoutY)
        {
            var vector = TilePlacement.CalculateLocationForTileLayout(tileLayoutX, tileLayoutY, Boundaries);
            TryMoveToLocation(vector);
        }

        public void GoToTile(ITile tile)
        {
            TryMoveToLocation(new Vector2(tile.Position.X, tile.Position.Y));
        }

        public void Kill()
        {
            Health = 0;

            _messageBus.Publish(new PlayerDied());
        }

    }
}
