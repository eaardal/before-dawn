using BeforeDawn.Core.Game.Helpers;

namespace BeforeDawn.Core.Game.Abstract
{
    internal interface ITile : IDraw
    {
        void Initialize(TileMatch match);
        string TileType { get; }
        bool IsStartTile { get; }
        bool IsEndTile { get; }
    }
}