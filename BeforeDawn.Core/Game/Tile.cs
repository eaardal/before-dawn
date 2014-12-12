using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public bool IsStartTile { get { return TileType == "S"; } }
        public bool IsEndTile { get { return TileType == "E"; } }

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

        private void LoadTile()
        {
            if (TileType == "0")
            {
                LoadDefaultTile();
            }
            else if (TileType == "S")
            {
                LoadDefaultTile();
            }
            else if (TileType == "E")
            {
                LoadExitTile();
            }
        }

        private void LoadExitTile()
        {
            var texture = LoadTexture("Tile_Exit");
            SetDefaultValues(texture, CalculateLocationForTilLayout(texture));
            Collision = TileCollision.Passable;
        }

        private void LoadDefaultTile()
        {
            var texture = LoadTexture("Tile_Default");
            SetDefaultValues(texture, CalculateLocationForTilLayout(texture));
            Collision = TileCollision.Passable;
        }

        private Texture2D LoadTexture(string assetName)
        {
            return _contentManager.Load<Texture2D>("Tiles/" + assetName);
        }

        public override void Update(GameTime gameTime)
        {

        }

        private Vector2 CalculateLocationForTilLayout(Texture2D texture)
        {
            var x = (TileLayoutX - 1) * texture.Width;
            var y = (TileLayoutY - 1) * texture.Height;    
            return new Vector2(x, y);
        }
    }
}
