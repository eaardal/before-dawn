using System;
using System.Diagnostics;
using BeforeDawn.Core.Game.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeforeDawn.Core.Game
{
    /// <summary>
    /// Thanks to Khalid Abuhakmeh @ http://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite
    /// </summary>
    public class Camera2D : GameComponent, ICamera2D
    {
        private readonly Microsoft.Xna.Framework.Game _game;
        private Vector2 _position;
        private bool _hasRunOnce;
        private Rectangle _passiveArea;
        private Rectangle _topBufferArea;
        private Rectangle _bottomBufferArea;
        private Rectangle _leftBufferArea;
        private Rectangle _rightBufferArea;
        protected float ViewportHeight;
        protected float ViewportWidth;

        public Camera2D(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            if (game == null) throw new ArgumentNullException("game");
            _game = game;
        }

        #region Properties

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public Matrix Transform { get; set; }
        public IFocusable Focus { get; set; }
        public float MoveSpeed { get; set; }
        public bool EnableCameraDragEffect { get; set; }
        public bool AlwaysCenterFocus { get; set; }

        #endregion

        /// <summary>
        /// Called when the GameComponent needs to be initialized. 
        /// </summary>
        public override void Initialize()
        {
            ViewportWidth = _game.GraphicsDevice.Viewport.Width;
            ViewportHeight = _game.GraphicsDevice.Viewport.Height;

            ScreenCenter = new Vector2(ViewportWidth / 2, ViewportHeight / 2);
            Scale = 1;
            MoveSpeed = 1f;

            const int bufferPercentage = 10;

            var xBuffer = (int)ViewportWidth * bufferPercentage / 100;
            var yBuffer = (int)ViewportHeight * bufferPercentage / 100;

            var minXBuffer = xBuffer;
            var minYBuffer = yBuffer;

            var maxXBuffer = ViewportWidth - xBuffer;
            var maxYBuffer = ViewportHeight - yBuffer;

            var passiveAreaWidth = (int)ViewportWidth - (xBuffer * 2);
            var passiveAreaHeight = (int)ViewportHeight - (yBuffer * 2);

            _passiveArea = new Rectangle(xBuffer, yBuffer, passiveAreaWidth, passiveAreaHeight);

            _topBufferArea = new Rectangle(0, 0, (int)ViewportWidth, yBuffer);
            _bottomBufferArea = new Rectangle(0, _passiveArea.Bottom, (int)ViewportWidth, yBuffer);
            _leftBufferArea = new Rectangle(yBuffer, _passiveArea.X, xBuffer, passiveAreaHeight);
            _rightBufferArea = new Rectangle(_passiveArea.Right, yBuffer, xBuffer, _passiveArea.Height);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Focus == null)
            {
                return;
            }
            
            // Create the Transform used by any
            // spritebatch process
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(Scale, Scale, Scale));

            Origin = ScreenCenter / Scale;

            // Move the Camera to the position that it needs to go
            var delta = EnableCameraDragEffect ? (float)gameTime.ElapsedGameTime.TotalSeconds : 1f;

            PrintBufferAreaStatus();

            if (AlwaysCenterFocus || !_hasRunOnce || (_leftBufferArea.Contains(Focus.Position) || _rightBufferArea.Contains(Focus.Position)))
            {
                _position.X += (Focus.Position.X - Position.X) * MoveSpeed * delta;    
            }

            if (AlwaysCenterFocus || !_hasRunOnce || (_topBufferArea.Contains(Focus.Position) || _bottomBufferArea.Contains(Focus.Position)))
            {
                _position.Y += (Focus.Position.Y - Position.Y) * MoveSpeed * delta;
            }
            
            if (!_hasRunOnce)
                _hasRunOnce = true;

            base.Update(gameTime);
        }

        private void PrintBufferAreaStatus()
        {
            if (_rightBufferArea.Contains(Focus.Position))
            {
                Debug.WriteLine("Right buffer area contains focus position: True");
            }

            if (_leftBufferArea.Contains(Focus.Position)) 
            {
                Debug.WriteLine("Left buffer area contains focus position: True");
            }

            if (_topBufferArea.Contains(Focus.Position))
            {
                Debug.WriteLine("Top buffer area contains focus position: True");
            }

            if (_bottomBufferArea.Contains(Focus.Position))
            {
                Debug.WriteLine("Bottom buffer area contains focus position: True");
            }
        }

        private bool ShouldMove()
        {
            if (Focus == null)
                return false;

            Debug.WriteLine("Is in passive area? {0}", _passiveArea.Contains(Focus.Position));

            return _hasRunOnce && _passiveArea.Contains(Focus.Position);
        }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects
        /// directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns>
        ///     <c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInView(Vector2 position, Texture2D texture)
        {
            // If the object is not within the horizontal bounds of the screen

            if ((position.X + texture.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X))
                return false;

            // If the object is not within the vertical bounds of the screen
            if ((position.Y + texture.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y))
                return false;

            // In View
            return true;
        }
    }
}