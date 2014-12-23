using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    class ConveyorBeltTile : Tile
    {
        private readonly ILevelState _levelState;
        public Direction Direction { get; private set; }
        public int ConveyorSpeed { get { return 30; } }

        public ConveyorBeltTile(IContentManagerAdapter contentManager, ILevelState levelState) : base(contentManager)
        {
            if (levelState == null) throw new ArgumentNullException("levelState");
            _levelState = levelState;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            var isOnConveyorBeltTile =
                _levelState.Tiles.Where(tile => tile.IsConveyorBeltTile)
                    .Any(tile => tile.Boundaries.Intersects(Boundaries));

            var t = _levelState.Tiles.Where(tile => tile.Boundaries.Intersects(Boundaries));

            if (isOnConveyorBeltTile)
            {
                var currentTile = _levelState.Tiles.Where(tile => tile.IsConveyorBeltTile).Where(tile => tile.Boundaries.Intersects(Boundaries));

                foreach (var tile in currentTile.Cast<ConveyorBeltTile>())
                {
                    if (tile.Direction == Direction.Up)
                    {
                        //TryMoveToLocation(new Vector2(Location.X, Location.Y - tile.ConveyorSpeed));
                    }
                }
            }
        }

        public override List<string> TileTypes
        {
            get { return new List<string>{ TileKinds.ConveyorUp, TileKinds.ConveyorDown, TileKinds.ConveyorLeft, TileKinds.ConveyorRight }; }
        }

        public override void Initialize(TileMatch tile)
        {
            if (tile.TileType == TileKinds.ConveyorUp)
            {
                Direction = Direction.Up;
            }
            else if (tile.TileType == TileKinds.ConveyorDown)
            {
                Direction = Direction.Down;
            }
            else if (tile.TileType == TileKinds.ConveyorLeft)
            {
                Direction = Direction.Left;
            }
            else if (tile.TileType == TileKinds.ConveyorRight)
            {
                Direction = Direction.Right;
            }

            base.Initialize(tile);
        }

        protected override void LoadTile()
        {
            var texture = LoadTexture("Tile_Conveyor");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture));

            if (Direction == Direction.Up)
            {
                UseCenterAsOrigin = true;
                Boundaries = new Rectangle((int)CenterLocation.X, (int)CenterLocation.Y, Texture.Width, Texture.Height);
                Rotation = MathHelper.Pi * 1.5f;
            }
            else if (Direction == Direction.Down)
            {
                UseCenterAsOrigin = true;
                Boundaries = new Rectangle((int)CenterLocation.X, (int)CenterLocation.Y, Texture.Width, Texture.Height);
                Rotation = MathHelper.Pi * 0.5f;
            }
            else if (Direction == Direction.Left)
            {
                UseCenterAsOrigin = true;
                Boundaries = new Rectangle((int)CenterLocation.X, (int)CenterLocation.Y, Texture.Width, Texture.Height);
                Rotation = MathHelper.Pi * 1f;
            }
            else if (Direction == Direction.Right)
            {
                // Default
            }

            Collision = TileCollision.Passable;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawWithAllSettings(spriteBatch);
        }
    }
}
