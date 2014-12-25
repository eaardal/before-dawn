using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework.Graphics;

namespace BeforeDawn.Core.Game
{
    class Valuable : Collectable, IValuable
    {
        public Valuable(IContentManagerAdapter contentManager, ILevelState levelState, IMessageBus messageBus) 
            : base(contentManager, levelState, messageBus)
        {
        }

        protected override void Collect()
        {
            if (LevelState.Collectables.Where(c => c is IValuable).All(c => c.IsCollected))
            {
                var endTile = LevelState.GetEndTile();
                endTile.Collision = TileCollision.Passable;
            }   

            base.Collect();
        }

        public override void Initialize(TileMatch match)
        {
            var texture = ContentManager.Load<Texture2D>("Items\\Item_Valuable");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(match.X, match.Y, texture.Bounds));
        }
    }
}
