using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    enum SandTileState
    {
        Sand, Normal
    }

    /// <summary>
    /// Is made into a normal tile by walking over it.
    /// Can not move a "movable block tile" over it until it has become "normal".
    /// </summary>
    class SandTile : Tile
    {
        private Texture2D _normalTexture;
        private SandTileState _state;
        private Texture2D _sandTexture;

        public SandTile(IContentManagerAdapter contentManager, ILevelState levelState)
            : base(contentManager, levelState)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (Boundaries.Contains(LevelState.Player.Boundaries))
            {
                if (_state == SandTileState.Sand)
                {
                    _state = SandTileState.Normal;
                }
            }

            Texture = _state == SandTileState.Sand ? _sandTexture : _normalTexture;
        }

        public override List<string> TileTypes
        {
            get { return new List<string>{ TileKinds.Sand }; }
        }

        protected override void LoadTile()
        {
            _sandTexture = LoadTexture("Tile_Exit_Closed");
            _normalTexture = LoadTexture("Tile_Default");
            SetDefaultValues(_sandTexture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, _sandTexture.Bounds));
            Collision = TileCollision.Passable;
            _state = SandTileState.Sand;
        }
    }
}
