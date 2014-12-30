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
        public TeleportTile(IContentManagerAdapter contentManager, ILevelState levelState) : base(contentManager, levelState)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (Boundaries.Intersects(LevelState.Player.Boundaries))
            {
                if (LevelState.Player.Direction == Direction.Right)
                {
                    var nextTeleportTile =
                        LevelState.Tiles.GetLayoutRow(TileLayoutY).Where(tile => tile.IsTeleportTile)
                            .FirstOrDefault(tile => tile.TileLayoutX > TileLayoutX);

                    LevelState.Player.GoToTile(nextTeleportTile != null
                        ? nextTeleportTile.GetRightNeighbourOrDefault(LevelState.Tiles)
                        : this.GetRightNeighbourOrDefault(LevelState.Tiles));
                }
                else if (LevelState.Player.Direction == Direction.Left)
                {
                    var nextTeleportTile =
                        LevelState.Tiles.GetLayoutRow(TileLayoutY).Where(tile => tile.IsTeleportTile)
                            .FirstOrDefault(tile => tile.TileLayoutX < TileLayoutX);

                    LevelState.Player.GoToTile(nextTeleportTile != null
                        ? nextTeleportTile.GetLeftNeighbourOrDefault(LevelState.Tiles)
                        : this.GetLeftNeighbourOrDefault(LevelState.Tiles));
                }
                else if (LevelState.Player.Direction == Direction.Up)
                {
                    var nextTeleportTile =
                        LevelState.Tiles.GetLayoutColumn(TileLayoutX).Where(tile => tile.IsTeleportTile)
                            .FirstOrDefault(tile => tile.TileLayoutY < TileLayoutY);

                    LevelState.Player.GoToTile(nextTeleportTile != null
                        ? nextTeleportTile.GetAboveNeighbourOrDefault(LevelState.Tiles)
                        : this.GetAboveNeighbourOrDefault(LevelState.Tiles));
                }
                else if (LevelState.Player.Direction == Direction.Down)
                {
                    var nextTeleportTile =
                        LevelState.Tiles.GetLayoutColumn(TileLayoutX).Where(tile => tile.IsTeleportTile)
                            .FirstOrDefault(tile => tile.TileLayoutY > TileLayoutY);

                    LevelState.Player.GoToTile(nextTeleportTile != null
                        ? nextTeleportTile.GetBelowNeighbourOrDefault(LevelState.Tiles)
                        : this.GetBelowNeighbourOrDefault(LevelState.Tiles));
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
