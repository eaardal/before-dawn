using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Game.Messages;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    class EndBlockTile : Tile
    {
        private bool _isBlocking;
        private Texture2D _collectedTexture;

        public EndBlockTile(IContentManagerAdapter contentManager, IMessageBus messageBus, ILevelState levelState) 
            : base(contentManager, levelState)
        {
            if (messageBus == null) throw new ArgumentNullException("messageBus");
            
            _isBlocking = true;

            messageBus.Subscribe<ItemCollected>(OnItemCollected);
        }

        private void OnItemCollected(ItemCollected msg)
        {
            if (msg.Item is IValuable)
            {
                _isBlocking =
                    LevelState.Collectables
                        .Where(item => item is IValuable)
                        .Cast<IValuable>()
                        .All(val => !val.IsCollected);
            }
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (!_isBlocking)
            {
                Collision = TileCollision.Passable;
            }

            if (Boundaries.Contains(LevelState.Player.Boundaries))
            {
                SetDefaultValues(_collectedTexture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, _collectedTexture.Bounds));
            }
        }

        public override List<string> TileTypes
        {
            get { return new List<string>{ TileKinds.EndBlock };}
        }

        protected override void LoadTile()
        {
            var activeTexture = LoadTexture("Tile_Exit_Closed");
            _collectedTexture = LoadTexture("Tile_Default");
            SetDefaultValues(activeTexture, TilePlacement.CalculateLocationForTileLayout(TileLayoutX, TileLayoutY, activeTexture.Bounds));
            Collision = TileCollision.Impassable;
        }
    }
}
