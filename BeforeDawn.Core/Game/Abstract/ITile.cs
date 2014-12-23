using System.Collections.Generic;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Game.Tiles;
using Microsoft.Xna.Framework;

namespace BeforeDawn.Core.Game.Abstract
{
    internal interface ITile : ISprite
    {
        void Initialize(TileMatch match);
        List<string> TileTypes { get; }
        bool IsStartTile { get; }
        bool IsEndTile { get; }
        int TileLayoutX { get; set; }
        int TileLayoutY { get; set; }
        bool IsBlockTile { get; }
        bool IsDefaultTile { get; }
        bool HasCollectable { get; }
        bool IsConveyorBeltTile { get; }
        TileCollision Collision { get; set; }
        bool IsTeleportTile { get; }
        bool IsDoorTile { get; }
    }
}