using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Abstract
{
    public abstract class Sprite : ISprite, IDraw, IUpdate
    {
        private Rectangle? _sourceRectangle;
        private Vector2 _position;
        private bool _useCenterAsOrigin;
        public Rectangle Boundaries { get; protected set; }
        public Texture2D Texture { get; protected set; }

        public Vector2 Position
        {
            get { return _position; }
            protected set
            {
                _position = value;
                Boundaries = new Rectangle((int)value.X, (int)value.Y, Boundaries.Width, Boundaries.Height);
                DrawRectangle = new Rectangle((int)value.X, (int)value.Y, Boundaries.Width, Boundaries.Height);
            }
        }

        public Vector2 Center { get { return new Vector2(Boundaries.Width / 2.0f, Boundaries.Height / 2.0f); } }
        public Vector2 CenterLocation { get { return new Vector2(Position.X + Center.X, Position.Y + Center.Y); } }
        public Vector2 Origin { get; protected set; }
        public float Rotation { get; protected set; }
        public Color Color { get; protected set; }
        public Rectangle? DrawRectangle { get; protected set; }

        /// <summary>
        /// SourceRectangle's X/Y value is based in the current Position.X/Y.
        /// If the current Position is 100x,100y and SourceRectangle is 0x,50y, it will draw a rectangle at 100x,150y
        /// </summary>
        public Rectangle? SourceRectangle
        {
            get { return _sourceRectangle; }
            protected set
            {
                if (value.HasValue)
                {
                    _sourceRectangle = value;
                }
            }
        }

        public Vector2? Scale { get; protected set; }
        public SpriteEffects SpriteEffect { get; protected set; }
        public float Depth { get; protected set; }

        public bool UseCenterAsOrigin
        {
            get { return _useCenterAsOrigin; }
            protected set
            {
                _useCenterAsOrigin = value;
                if (_useCenterAsOrigin)
                {
                    DrawRectangle = new Rectangle(Boundaries.X + Boundaries.Width / 2, Boundaries.Y + Boundaries.Height / 2, Boundaries.Width, Boundaries.Height);
                }
            }
        }

        protected Sprite() { }

        protected Sprite(Texture2D texture, Vector2 location)
            : this(texture, location, new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height))
        { }

        protected Sprite(Texture2D texture, Vector2 location, Rectangle boundaries)
        {
            SetDefaultValues(texture, location, boundaries);
        }

        protected void SetDefaultValues(Texture2D texture, Vector2 location)
        {
            SetDefaultValues(texture, location, new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height));
        }

        protected void SetDefaultValues(Texture2D texture, Vector2 location, Rectangle boundaries)
        {
            Boundaries = boundaries;
            Texture = texture;
            Position = location;
            Rotation = 0.0f;
            Color = Color.White;
            DrawRectangle = boundaries;
            SourceRectangle = null;
            Scale = Vector2.Zero;
            SpriteEffect = SpriteEffects.None;
            Depth = 0.0f;
            UseCenterAsOrigin = false;
            Origin = Vector2.Zero;
        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public virtual void DrawWithAllSettings(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, null, DrawRectangle, SourceRectangle, UseCenterAsOrigin ? Center : Origin, Rotation, Scale, Color, SpriteEffect, Depth);
        }

        public abstract void Update(GameTime gameTime, KeyboardState keyboardState);
    }
}
