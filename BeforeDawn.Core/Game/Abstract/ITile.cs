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
    }
}