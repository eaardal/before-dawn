using BeforeDawn.Core.Game.Helpers;

namespace BeforeDawn.Core.Game.Abstract
{
    internal interface ICollectable : ISprite
    {
        void Initialize(TileMatch match);
        bool IsCollected { get; }
    }
}