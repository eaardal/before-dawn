using BeforeDawn.Core.Game.Helpers;

namespace BeforeDawn.Core.Game.Abstract
{
    internal interface ICollectable : ISprite
    {
        void Initialize(TileMatch match);
        bool IsCollected { get; }
        string CollectableKind { get; }
        int TileLayoutX { get; }
        int TileLayoutY { get; }
    }
}