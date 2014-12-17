using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;

namespace BeforeDawn.Core.Game.Abstract
{
    internal interface ITile : ISprite
    {
        void Initialize(TileMatch match);
        string TileType { get; }
        bool IsStartTile { get; }
        bool IsEndTile { get; }
        int TileLayoutX { get; set; }
        int TileLayoutY { get; set; }
        bool IsBlockTile { get; }
        bool IsDefaultTile { get; }
        bool HasCollectable { get; }
        TileCollision Collision { get; set; }
    }
}