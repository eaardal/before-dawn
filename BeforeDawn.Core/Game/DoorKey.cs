using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework.Graphics;

namespace BeforeDawn.Core.Game
{
    class DoorKey : Collectable, IDoorKey
    {
        public string DoorKind { get; private set; }

        public DoorKey(IContentManagerAdapter contentManager, IMessageBus messageBus, ILevelState levelState) : base(contentManager, levelState, messageBus)
        {
        }

        public override void Initialize(TileMatch match)
        {
            DoorKind = match.TileType;
            var texture = ContentManager.Load<Texture2D>("Items\\Item_Collectable");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(match.X, match.Y, texture));
            
            base.Initialize(match);
        }

    }

    internal interface IDoorKey : ICollectable
    {
        string DoorKind { get; }
    }
}
