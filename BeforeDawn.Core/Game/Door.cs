using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Game.Tiles;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    class Door : Collectable, IDoor
    {
        public string Key { get; private set; }

        public Door(IContentManagerAdapter contentManager, ILevelState levelState, IMessageBus messageBus) 
            : base(contentManager, levelState, messageBus)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            
        }

        public override void Initialize(TileMatch match)
        {
            var texture = ContentManager.Load<Texture2D>("Tiles\\Tile_Exit_Closed");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(match.X, match.Y, texture));

            if (match.TileType.Length == 2)
            {
                var chars = match.TileType.ToCharArray();
                var number = chars[1].ToString(CultureInfo.InvariantCulture);
                Key = TileKinds.Keys.SingleOrDefault(k => k.EndsWith(number));
            }

            base.Initialize(match);
        }
    }
}
