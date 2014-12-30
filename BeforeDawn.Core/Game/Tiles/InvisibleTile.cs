using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    enum InvisibleTileState
    {
        AlwaysInvisible, VisibleWhenTouched
    }

    /// <summary>
    /// Is either:
    /// - always invisible
    /// - becomes visible once touched by player
    /// </summary>
    class InvisibleTile : Tile
    {
        private InvisibleTileState _state;
        
        public InvisibleTile(IContentManagerAdapter contentManager, ILevelState levelState) : base(contentManager, levelState)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (_state == InvisibleTileState.VisibleWhenTouched)
            {
                if (IsPlayerOnTile())
                {
                    
                }
            }
        }

        public override List<string> TileTypes
        {
            get { return new List<string>{ TileKinds.InvisibleAlways, TileKinds.InvisibleTouched };}
        }

        public override void Initialize(TileMatch tile)
        {
            if (tile.TileType == TileKinds.InvisibleAlways)
            {
                _state = InvisibleTileState.AlwaysInvisible;
            }
            else if (tile.TileType == TileKinds.InvisibleTouched)
            {
                _state = InvisibleTileState.VisibleWhenTouched;
            }
            
            base.Initialize(tile);
        }

        protected override void LoadTile()
        {
            var texture = LoadTexture("Tile_Default");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture.Bounds));
            Collision = TileCollision.Passable;
        }
    }
}
