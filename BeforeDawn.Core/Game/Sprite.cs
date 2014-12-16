using BeforeDawn.Core.Game.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    public abstract class Sprite : ISprite
    {
        private Rectangle? _sourceRectangle;
        public Rectangle Boundaries { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public Vector2 Location { get; protected set; }
        public Vector2 Center { get { return new Vector2(Boundaries.Width / 2.0f, Boundaries.Height / 2.0f); } }
        public Vector2 CenterLocation { get { return new Vector2(Location.X + Center.X, Location.Y + Center.Y); } }
        public Vector2 Origin { get; protected set; }
        public float Rotation { get; protected set; }
        public Color Color { get; protected set; }
        public Rectangle? DrawRectangle { get; protected set; }

        /// <summary>
        /// SourceRectangle's X/Y value is based in the current Location.X/Y.
        /// If the current Location is 100x,100y and SourceRectangle is 0x,50y, it will draw a rectangle at 100x,150y
        /// </summary>
        public Rectangle? SourceRectangle
        {
            get { return _sourceRectangle; }
            protected set
            {
                if (value.HasValue)
                {
                    _sourceRectangle = value;
                    Boundaries = new Rectangle(value.Value.X, value.Value.Y, value.Value.Width, value.Value.Height);
                }
            }
        }

        public Vector2? Scale { get; protected set; }
        public SpriteEffects SpriteEffect { get; protected set; }
        public float Depth { get; protected set; }
        public bool UseCenterAsOrigin { get; protected set; }

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
            Location = location;
            Rotation = 0.0f;
            Color = Color.White;
            DrawRectangle = null;
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
            spriteBatch.Draw(Texture, Location, Color.White);
        }

        public virtual void DrawWithAllSettings(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, null, Boundaries, SourceRectangle, UseCenterAsOrigin ? Center : Origin, Rotation, Scale, Color, SpriteEffect, Depth);
        }

        public abstract void Update(GameTime gameTime, KeyboardState keyboardState);
    }
}
