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
        private int _aggregatedGameTime;
        private readonly ILevelState _levelState;
        public Direction Direction { get; private set; }
        public int ConveyorSpeed { get { return 100; } }

        public ConveyorBeltTile(IContentManagerAdapter contentManager, ILevelState levelState) : base(contentManager)
        {
            if (levelState == null) throw new ArgumentNullException("levelState");
            _levelState = levelState;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (Boundaries.Intersects(_levelState.Player.Boundaries))
            {
                _aggregatedGameTime += gameTime.ElapsedGameTime.Milliseconds;

                if (_aggregatedGameTime < ConveyorSpeed)
                {
                    return;
                }

                var currentTile =
                    _levelState.Tiles
                        .Where(tile => tile.IsConveyorBeltTile)
                        .Where(tile => tile.Boundaries.Intersects(_levelState.Player.Boundaries))
                        .Cast<ConveyorBeltTile>()
                        .FirstOrDefault();

                if (currentTile == null)
                {
                    return;
                }

                if (currentTile.Direction == Direction.Down)
                {
                    _levelState.Player.GoToTile(currentTile.TileLayoutX, currentTile.TileLayoutY + 1);
                }
                else if (currentTile.Direction == Direction.Right)
                {
                    _levelState.Player.GoToTile(currentTile.TileLayoutX + 1, currentTile.TileLayoutY);
                }

                _aggregatedGameTime = 0;
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
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture.Bounds));

            if (Direction == Direction.Up)
            {
                UseCenterAsOrigin = true;
                Boundaries = new Rectangle((int)CenterLocation.X, (int)CenterLocation.Y, Texture.Width, Texture.Height);
                Rotation = MathHelper.Pi * 1.5f;
            }
            else if (Direction == Direction.Down)
            {
                UseCenterAsOrigin = true;
                //Boundaries = new Rectangle((int)CenterLocation.X, (int)CenterLocation.Y, Texture.Width, Texture.Height);
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
            //Color = Color.Red;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawWithAllSettings(spriteBatch);
        }
    }
}
