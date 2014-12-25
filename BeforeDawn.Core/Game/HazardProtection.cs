using System.Globalization;
using System.Linq;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeforeDawn.Core.Game
{
    class HazardProtection : Collectable, IHazardProtection
    {
        public string Hazard { get; private set; }

        public HazardProtection(IContentManagerAdapter contentManager, ILevelState levelState, IMessageBus messageBus) 
            : base(contentManager, levelState, messageBus)
        {
        }

        protected override void Collect()
        {
            var hazardTiles = LevelState.Tiles
                .Where(t => t is IHazard)
                .Cast<IHazard>()
                .Where(h => h.Hazard == Hazard);

            foreach (var tile in hazardTiles)
            {
                tile.DisableHazard();
            }

            base.Collect();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsCollected)
                DrawWithAllSettings(spriteBatch);
        }

        public override void Initialize(TileMatch match)
        {
            var texture = ContentManager.Load<Texture2D>("Items\\Item_HazardProtection");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(match.X, match.Y, texture.Bounds));

            SetHazard(match);
            SetColor(match);

            base.Initialize(match);
        }

        private void SetColor(TileMatch match)
        {
            if (Hazard == TileKinds.HazardFire)
            {
                Color = Color.Red;
            }
            else if (Hazard == TileKinds.HazardWater)
            {
                Color = Color.Blue;
            }
        }

        private void SetHazard(TileMatch match)
        {
            if (match.TileType.Length == 2)
            {
                var chars = match.TileType.ToCharArray();
                var number = chars[1].ToString(CultureInfo.InvariantCulture);
                Hazard = TileKinds.Hazards.SingleOrDefault(k => k.EndsWith(number));
            }
        }
    }

    internal interface IHazardProtection : ICollectable
    {
    }
}
