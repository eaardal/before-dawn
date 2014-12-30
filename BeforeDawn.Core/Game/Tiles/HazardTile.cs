using System;
using System.Collections.Generic;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    class HazardTile : Tile, IHazard
    {
        private const int DamageTickDelay = 200;
        private int _aggregatedGameTime;

        public string Hazard { get; private set; }
        public bool IsActive { get; private set; }
        
        public HazardTile(IContentManagerAdapter contentManager, ILevelState levelState) : base(contentManager, levelState)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (LevelState.Player.Boundaries.Intersects(Boundaries))
            {
                _aggregatedGameTime += gameTime.ElapsedGameTime.Milliseconds;

                if (_aggregatedGameTime > DamageTickDelay && IsActive)
                {
                    LevelState.Player.Kill();
                    _aggregatedGameTime = 0;
                }
            }
        }

        public override List<string> TileTypes
        {
            get { return new List<string>{ TileKinds.HazardWater, TileKinds.HazardFire }; }
        }

        protected override void LoadTile()
        {
            var texture = LoadTexture(GetTextureForHazard());
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture.Bounds));
            Collision = TileCollision.Passable;
        }

        private string GetTextureForHazard()
        {
            if (Hazard == TileKinds.HazardFire)
            {
                return "Tile_Fire";
            }
            return "Tile_Water";
        }

        public override void Initialize(TileMatch tile)
        {
            Hazard = tile.TileType;
            IsActive = true;
            base.Initialize(tile);
        }

        public void DisableHazard()
        {
            IsActive = false;
        }
    }
}
