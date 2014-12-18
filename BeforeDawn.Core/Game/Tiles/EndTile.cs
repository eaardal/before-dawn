﻿using System.Collections.Generic;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    class EndTile : Tile
    {
        public EndTile(IContentManagerAdapter contentManager)
            : base(contentManager)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {

        }

        public override List<string> TileTypes
        {
            get { return new List<string>{ TileKinds.End }; }
        }

        protected override void LoadTile()
        {
            var texture = LoadTexture("Tile_Exit");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture));
            Collision = TileCollision.Impassable;
        }
    }
}
