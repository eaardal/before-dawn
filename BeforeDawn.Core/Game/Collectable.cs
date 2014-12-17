﻿using System;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Game.Messages;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    class Collectable : Sprite, ICollectable
    {
        private readonly IContentManagerAdapter _contentManager;
        private readonly ILevelState _levelState;

        public bool IsCollected { get; private set; }

        public Collectable(IContentManagerAdapter contentManager, ILevelState levelState)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            if (levelState == null) throw new ArgumentNullException("levelState");

            _contentManager = contentManager;
            _levelState = levelState;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            var player = _levelState.Player;

            if (player.Boundaries.Intersects(Boundaries))
            {
                Collect();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                base.Draw(gameTime, spriteBatch);
            }
        }

        private void Collect()
        {
            IsCollected = true;

            Message.Publish(new ItemCollected(this));
        }

        public void Initialize(TileMatch match)
        {
            var texture = _contentManager.Load<Texture2D>("Items\\Item_Collectable");
            SetDefaultValues(texture, TilePlacement.CalculateLocationForTileLayout(match.X, match.Y, texture));
        }
    }
}