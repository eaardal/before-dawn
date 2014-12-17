using System;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,
    }

    class Tile : Sprite, ITile
    {
        private readonly IContentManagerAdapter _contentManager;

        public string TileType { get; private set; }
        public int TileLayoutX { get; set; }
        public int TileLayoutY { get; set; }
        public TileCollision Collision { get; set; }
        public bool IsDefaultTile { get { return TileType == TileTypes.Default; }}
        public bool IsStartTile { get { return TileType == TileTypes.Start; } }
        public bool IsEndTile { get { return TileType == TileTypes.End; } }
        public bool IsBlockTile { get { return TileType == TileTypes.Block; }}
        public bool HasCollectable { get { return TileType == TileTypes.Collectable; } }

        public Tile(IContentManagerAdapter contentManager)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            _contentManager = contentManager;
        }

        public void Initialize(TileMatch tile)
        {
            TileType = tile.TileType;
            TileLayoutX = tile.X;
            TileLayoutY = tile.Y;
            LoadTile();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {

        }

        private void LoadTile()
        {
            if (TileType == TileTypes.Default)
            {
                LoadDefaultTile();
            }
            else if (TileType == TileTypes.Start)
            {
                LoadDefaultTile();
            }
            else if (TileType == TileTypes.End)
            {
                LoadExitTile();
            }
            else if (TileType == TileTypes.Block)
            {
                LoadBlockTile();
            }
            else if (TileType == TileTypes.Collectable)
            {
                LoadDefaultTile();
            }
        }

        private void LoadBlockTile()
        {
            var texture = LoadTexture("Tile_Block");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture));
            Collision = TileCollision.Impassable;
        }

        private void LoadExitTile()
        {
            var texture = LoadTexture("Tile_Exit");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture));
            Collision = TileCollision.Impassable;
        }

        private void LoadDefaultTile()
        {
            var texture = LoadTexture("Tile_Default");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, texture));
            Collision = TileCollision.Passable;
        }

        private Texture2D LoadTexture(string assetName)
        {
            return _contentManager.Load<Texture2D>("Tiles/" + assetName);
        }
        
        
    }
}
