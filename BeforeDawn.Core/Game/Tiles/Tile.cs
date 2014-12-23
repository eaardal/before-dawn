using System;
using System.Collections.Generic;
using System.Linq;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    abstract class Tile : Sprite, ITile
    {
        private readonly IContentManagerAdapter _contentManager;
        
        public int TileLayoutX { get; set; }
        public int TileLayoutY { get; set; }
        public TileCollision Collision { get; set; }
        public bool IsTeleportTile { get { return TileTypes.Contains(TileKinds.Teleport); }}
        public bool IsDefaultTile { get { return TileTypes.Contains(TileKinds.Default); }}
        public bool IsStartTile { get { return TileTypes.Contains(TileKinds.Start); } }
        public bool IsEndTile { get { return TileTypes.Contains(TileKinds.End); } }
        public bool IsBlockTile { get { return TileTypes.Contains(TileKinds.Block); } }
        public bool IsConveyorBeltTile { get { return TileTypes.Any(t => t.StartsWith("C")); }}
        public bool IsDoorTile { get { return TileTypes.Any(t => t.StartsWith("D")); } }
        public bool HasCollectable { get { return TileTypes.Contains(TileKinds.Valuable); } }

        public abstract List<string> TileTypes { get; }

        protected Tile(IContentManagerAdapter contentManager)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            _contentManager = contentManager;
        }

        public virtual void Initialize(TileMatch tile)
        {
            TileLayoutX = tile.X;
            TileLayoutY = tile.Y;
            LoadTile();
        }
        
        protected Texture2D LoadTexture(string assetName)
        {
            return _contentManager.Load<Texture2D>("Tiles/" + assetName);
        }

        protected abstract void LoadTile();
    }
}
