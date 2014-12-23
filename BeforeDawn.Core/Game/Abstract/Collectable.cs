using System;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Game.Messages;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Abstract
{
    abstract class Collectable : Sprite, ICollectable
    {
        protected readonly IContentManagerAdapter ContentManager;
        protected readonly ILevelState LevelState;
        protected readonly IMessageBus MessageBus;

        public virtual int TileLayoutX { get; protected set; }
        public virtual int TileLayoutY { get; protected set; }
        public virtual bool IsCollected { get; protected set; }
        public virtual string CollectableKind { get; protected set; }

        protected Collectable(IContentManagerAdapter contentManager, ILevelState levelState, IMessageBus messageBus)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            if (levelState == null) throw new ArgumentNullException("levelState");
            if (messageBus == null) throw new ArgumentNullException("messageBus");

            ContentManager = contentManager;
            LevelState = levelState;
            MessageBus = messageBus;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (LevelState.Player.Boundaries.Intersects(Boundaries))
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

        protected virtual void Collect()
        {
            IsCollected = true;

            MessageBus.Publish(new ItemCollected(this));
        }

        public virtual void Initialize(TileMatch match)
        {
            TileLayoutX = match.X;
            TileLayoutY = match.Y;
            CollectableKind = match.TileType;
        }   
    }
}
