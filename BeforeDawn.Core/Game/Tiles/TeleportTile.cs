using System;
using System.Collections.Generic;
using System.Linq;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    class TeleportTile : Tile
    {
        private readonly ILevelState _levelState;

        public TeleportTile(IContentManagerAdapter contentManager, ILevelState levelState) : base(contentManager)
        {
            if (levelState == null) throw new ArgumentNullException("levelState");
            _levelState = levelState;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (Boundaries.Intersects(_levelState.Player.Boundaries))
            {
                if (_levelState.Player.Direction == Direction.Right)
                {
                    var nextTeleportTile =
                        _levelState.Tiles.GetLayoutRow(TileLayoutY).Where(tile => tile.IsTeleportTile)
                            .FirstOrDefault(tile => tile.TileLayoutX > TileLayoutX);

                    _levelState.Player.GoToTile(nextTeleportTile != null
                        ? nextTeleportTile.GetRightNeighbourOrDefault(_levelState.Tiles)
                        : this.GetRightNeighbourOrDefault(_levelState.Tiles));
                }
                else if (_levelState.Player.Direction == Direction.Left)
                {
                    var nextTeleportTile =
                        _levelState.Tiles.GetLayoutRow(TileLayoutY).Where(tile => tile.IsTeleportTile)
                            .FirstOrDefault(tile => tile.TileLayoutX < TileLayoutX);

                    _levelState.Player.GoToTile(nextTeleportTile != null
                        ? nextTeleportTile.GetLeftNeighbourOrDefault(_levelState.Tiles)
                        : this.GetLeftNeighbourOrDefault(_levelState.Tiles));
                }
                else if (_levelState.Player.Direction == Direction.Up)
                {
                    var nextTeleportTile =
                        _levelState.Tiles.GetLayoutColumn(TileLayoutX).Where(tile => tile.IsTeleportTile)
                            .FirstOrDefault(tile => tile.TileLayoutY < TileLayoutY);

                    _levelState.Player.GoToTile(nextTeleportTile != null
                        ? nextTeleportTile.GetAboveNeighbourOrDefault(_levelState.Tiles)
                        : this.GetAboveNeighbourOrDefault(_levelState.Tiles));
                }
                else if (_levelState.Player.Direction == Direction.Down)
                {
                    var nextTeleportTile =
                        _levelState.Tiles.GetLayoutColumn(TileLayoutX).Where(tile => tile.IsTeleportTile)
                            .FirstOrDefault(tile => tile.TileLayoutY > TileLayoutY);

                    _levelState.Player.GoToTile(nextTeleportTile != null
                        ? nextTeleportTile.GetBelowNeighbourOrDefault(_levelState.Tiles)
                        : this.GetBelowNeighbourOrDefault(_levelState.Tiles));
                }
            }
        }

        public override List<string> TileTypes
        {
            get { return new List<string>{ TileKinds.Teleport }; }
        }

        protected override void LoadTile()
        {
            var texture = LoadTexture("Tile_Teleport");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture.Bounds));
            Collision = TileCollision.Passable;
        }
    }
}
