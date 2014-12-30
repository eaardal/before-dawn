using System.Collections.Generic;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    class DefaultTile : Tile
    {
        public DefaultTile(IContentManagerAdapter contentManager, ILevelState levelState)
            : base(contentManager, levelState)
        {

        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {

        }

        public override List<string> TileTypes
        {
            get
            {
                return new List<string>
                {
                    TileKinds.Default,
                    TileKinds.Valuable,
                    TileKinds.DoorRed,
                    TileKinds.DoorBlue,
                    TileKinds.DoorGreen,
                    TileKinds.DoorYellow,
                    TileKinds.KeyRed,
                    TileKinds.KeyBlue,
                    TileKinds.KeyGreen,
                    TileKinds.KeyYellow,
                    TileKinds.HazardProtectionFire,
                    TileKinds.HazardProtectionWater
                };
            }
        }

        protected override void LoadTile()
        {
            var texture = LoadTexture("Tile_Default");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture.Bounds));
            Collision = TileCollision.Passable;
        }
    }
}
