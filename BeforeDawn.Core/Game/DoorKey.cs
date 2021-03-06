﻿using System;
using System.Globalization;
using System.Linq;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeforeDawn.Core.Game
{
    class DoorKey : Collectable, IDoorKey
    {
        public string Door { get; private set; }
        public string Identifier { get; private set; }
   
        public DoorKey(IContentManagerAdapter contentManager, IMessageBus messageBus, ILevelState levelState) : base(contentManager, levelState, messageBus)
        {
        }

        public void Use(params object[] parameters)
        {
            var doorForKey = parameters[0] as IDoor;

            if (doorForKey != null)
            {
                LevelState.Collectables.Remove(doorForKey);

                var tileForDoor = LevelState.Tiles.TileAt(doorForKey.TileLayoutX, doorForKey.TileLayoutY);

                if (tileForDoor != null)
                {
                    tileForDoor.Collision = TileCollision.Passable;
                }
            }
        }

        protected override void Collect()
        {
            LevelState.Player.Inventory.Add(this);

            base.Collect();
        }

        public override void Initialize(TileMatch match)
        {
            var texture = ContentManager.Load<Texture2D>("DoorKey");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(match.X, match.Y, texture.Bounds));

            SetDoor(match);
            SetColor(match);
            
            Identifier = match.TileType;

            base.Initialize(match);
        }

        private void SetColor(TileMatch match)
        {
            if (match.TileType == TileKinds.KeyRed)
            {
                Color = Color.Red;
            }
            else if (match.TileType == TileKinds.KeyBlue)
            {
                Color = Color.Blue;
            }
            else if (match.TileType == TileKinds.KeyGreen)
            {
                Color = Color.Green;
            }
            else if (match.TileType == TileKinds.KeyYellow)
            {
                Color = Color.Yellow;
            }
        }

        private void SetDoor(TileMatch match)
        {
            if (match.TileType.Length == 2)
            {
                var chars = match.TileType.ToCharArray();
                var number = chars[1].ToString(CultureInfo.InvariantCulture);
                Door = TileKinds.Doors.SingleOrDefault(k => k.EndsWith(number));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsCollected)
                DrawWithAllSettings(spriteBatch);
        }
    }
}
