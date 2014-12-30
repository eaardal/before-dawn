using System.Collections.Generic;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    class BlockTile : Tile
    {
        public BlockTile(IContentManagerAdapter contentManager, ILevelState levelState)
            : base(contentManager, levelState)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {

        }

        public override List<string> TileTypes
        {
            get { return new List<string>{ TileKinds.Block }; }
        }

        protected override void LoadTile()
        {
            var texture = LoadTexture("Tile_Block");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture.Bounds));
            Collision = TileCollision.Impassable;
        }
    }
}
