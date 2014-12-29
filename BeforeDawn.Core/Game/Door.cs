using System.Globalization;
using System.Linq;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    class Door : Collectable, IDoor, IRequireInventoryItem<IDoorKey>
    {
        public string Key { get; private set; }

        public Door(IContentManagerAdapter contentManager, ILevelState levelState, IMessageBus messageBus)
            : base(contentManager, levelState, messageBus)
        {
        }


        public bool HasRequiredItem
        {
            get { return Item != null; }
        }

        public IDoorKey Item
        {
            get
            {
                return LevelState.Collectables
                    .Where(item => item.IsCollected)
                    .Where(item => item is IDoorKey)
                    .Cast<IDoorKey>()
                    .FirstOrDefault(key => key.Identifier == Key);
            }
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (Boundaries.Contains(LevelState.Player.Boundaries))
            {
                if (LevelState.Player.Inventory.Any(item => item.Identifier == Key))
                {
                    if (HasRequiredItem)
                    {
                        Item.Use(this);
                    }
                }
            }
        }

        public override void Initialize(TileMatch match)
        {
            var texture = ContentManager.Load<Texture2D>("Door");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(match.X, match.Y, texture.Bounds));

            SetColor(match);
            SetKey(match);
        
            var tile = LevelState.Tiles.TileAt(match.X, match.Y);
            if (tile != null)
            {
                tile.Collision = TileCollision.Interactive;
            }

            base.Initialize(match);
        }

        private void SetColor(TileMatch match)
        {
            if (match.TileType == TileKinds.DoorRed)
            {
                Color = Color.Red;
            }
            else if (match.TileType == TileKinds.DoorBlue)
            {
                Color = Color.Blue;
            }
            else if (match.TileType == TileKinds.DoorGreen)
            {
                Color = Color.Green;
            }
            else if (match.TileType == TileKinds.DoorYellow)
            {
                Color = Color.Yellow;
            }
        }

        private void SetKey(TileMatch match)
        {
            if (match.TileType.Length == 2)
            {
                var chars = match.TileType.ToCharArray();
                var number = chars[1].ToString(CultureInfo.InvariantCulture);
                Key = TileKinds.Keys.SingleOrDefault(k => k.EndsWith(number));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawWithAllSettings(spriteBatch);
        }
    }
}
