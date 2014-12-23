using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Exceptions;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Game.Messages;
using BeforeDawn.Core.Game.Tiles;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    class Level : ILevel
    {
        private Rectangle _boundaries;
        private int _levelIndex;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIoC _ioc;
        private TimeSpan _timeRemaining;
        private readonly List<Texture2D> _textureLayers;
        private readonly IStreamReaderAdapter _streamReader;
        private readonly ITimeSpanAdapter _timeSpan;
        private readonly ILevelState _levelState;
        private readonly IEnumerable<ITile> _tileDefinitions;
        private readonly IMessageBus _messageBus;
        private Action _levelCompleted;

        public IContentManagerAdapter Content { get; private set; }

        public Level(IContentManagerAdapter contentManager, IServiceProvider serviceProvider,
            IIoC ioc, IStreamReaderAdapter streamReader, ITimeSpanAdapter timeSpan,
            ILevelState levelState, IEnumerable<ITile> tileDefinitions, IMessageBus messageBus)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");
            if (ioc == null) throw new ArgumentNullException("ioc");
            if (streamReader == null) throw new ArgumentNullException("streamReader");
            if (timeSpan == null) throw new ArgumentNullException("timeSpan");
            if (levelState == null) throw new ArgumentNullException("levelState");
            if (tileDefinitions == null) throw new ArgumentNullException("tileDefinitions");
            if (messageBus == null) throw new ArgumentNullException("messageBus");

            contentManager.Create(serviceProvider, "Content");
            Content = contentManager;
            _serviceProvider = serviceProvider;
            _ioc = ioc;
            _streamReader = streamReader;
            _timeSpan = timeSpan;
            _levelState = levelState;
            _tileDefinitions = tileDefinitions;
            _messageBus = messageBus;

            _textureLayers = new List<Texture2D>();

            _messageBus.Subscribe<ItemCollected>(OnItemCollected);
        }

        public void Initialize(IStreamAdapter levelLayoutStream, int levelIndex, Action levelCompleted)
        {
            _levelState.ResetAllState();

            _levelIndex = levelIndex;
            _timeRemaining = _timeSpan.FromMinutes(2.0);
            _levelCompleted = levelCompleted;

            LoadTiles(levelLayoutStream);

            SetLevelBoundaries();

            SpawnPlayer();
        }

        private void SetLevelBoundaries()
        {
            var firstRow = _levelState.Tiles.AsLayout().GetLayoutRow(1).ToList();
            var firstCol = _levelState.Tiles.AsLayout().GetLayoutColumn(1).ToList();

            var width = firstRow.Sum(tile => tile.Boundaries.Width);
            var height = firstCol.Sum(tile => tile.Boundaries.Height);
            _boundaries = new Rectangle(0, 0, width, height);
        }

        private void SpawnPlayer()
        {
            _levelState.Player = _ioc.Resolve<Player>();

            var startTile = _levelState.Tiles.Single(tile => tile.IsStartTile);

            _levelState.Player.Initialize(startTile.Location);
        }

        private void LoadTiles(IStreamAdapter fileStream)
        {
            var lines = ReadLines(fileStream);

            var matches = ExtractTiles(lines.ToList()).ToList();

            foreach (var match in matches)
            {
                var tileDef = _tileDefinitions.SingleOrDefault(t => t.TileTypes.Contains(match.TileType));
                if (tileDef != null)
                {
                    var tile = _ioc.Resolve<ITile>(tileDef.GetType().FullName);
                    tile.Initialize(match);

                    _levelState.Tiles.Add(tile);

                    if (TileKinds.Collectables.Contains(match.TileType))
                    {
                        ICollectable collectable = null;

                        if (match.TileType == TileKinds.Valuable)
                        {
                            collectable = _ioc.Resolve<IValuable>();
                        }
                        else if (TileKinds.Doors.Contains(match.TileType))
                        {
                            collectable = _ioc.Resolve<IDoor>();
                            tile.Collision = TileCollision.Impassable;
                        }
                        else if (TileKinds.Keys.Contains(match.TileType))
                        {
                            collectable = _ioc.Resolve<IDoorKey>();
                        }

                        if (collectable != null)
                        {
                            collectable.Initialize(match);
                            _levelState.Collectables.Add(collectable);
                        }
                    }
                }
                else
                {
                    throw new RequiredGameElementMissingException("Tile for " + match.TileType + " tile type missing");
                }
            }

            EnsureHasStartPosition();
            EnsureHasOnlyOneStartPosition();

            EnsureHasEndPosition();
            EnsureHasOnlyOneEndPosition();

            if (!_levelState.Collectables.Any(c => c is IValuable))
            {
                var endTile = _levelState.GetEndTile();
                endTile.Collision = TileCollision.Passable;
            } 
        }

        private void EnsureHasOnlyOneStartPosition()
        {
            var nrOfTiles = _levelState.Tiles.Count(tile => tile.IsStartTile);
            if (nrOfTiles > 1)
            {
                throw new RequiredGameElementMissingException("Level " + _levelIndex + " has too many start positions (" + nrOfTiles + "). Can only have one.");
            }
        }

        private void EnsureHasOnlyOneEndPosition()
        {
            var nrOfTiles = _levelState.Tiles.Count(tile => tile.IsEndTile);
            if (nrOfTiles > 1)
            {
                throw new RequiredGameElementMissingException("Level " + _levelIndex + " has too many end positions (" + nrOfTiles + "). Can only have one.");
            }
        }

        private void EnsureHasStartPosition()
        {
            var hasTile = _levelState.Tiles.Any(tile => tile.IsStartTile);
            if (!hasTile)
            {
                throw new RequiredGameElementMissingException("Missing start position tile");
            }
        }

        private void EnsureHasEndPosition()
        {
            var hasTile = _levelState.Tiles.Any(tile => tile.IsEndTile);
            if (!hasTile)
            {
                throw new RequiredGameElementMissingException("Missing end position tile");
            }
        }

        private IEnumerable<TileMatch> ExtractTiles(List<string> lines)
        {
            var xCounter = 0;
            var yCounter = 0;

            var tiles = new List<TileMatch>();

            for (var i = 0; i < lines.Count(); i++)
            {
                yCounter++;

                var matches = new Regex(@"([a-zA-Z0-9\$])+").Matches(lines[i]);

                for (var j = 0; j < matches.Count; j++)
                {
                    xCounter++;

                    var match = matches[j];

                    if (!match.Success)
                    {
                        continue;
                    }

                    tiles.Add(new TileMatch
                    {
                        TileType = match.Value,
                        X = xCounter,
                        Y = yCounter
                    });
                }

                xCounter = 0;
            }

            return tiles;
        }

        private IEnumerable<string> ReadLines(IStreamAdapter fileStream)
        {
            var lines = new List<string>();

            using (var reader = _streamReader.ReadStream(fileStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith("#") && !line.StartsWith("//"))
                    {
                        lines.Add(line);
                    }
                }
            }

            return lines;
        }

        private void OnItemCollected(ItemCollected message)
        {
            if (message.Item is IValuable)
            {
                UpdateRemainingValuableCount();   
            }
            else if (message.Item is IDoorKey)
            {
            }
        }

        private void UpdateRemainingValuableCount()
        {
            var remaining = _levelState.Collectables.Where(c => c is Valuable).Count(c => !c.IsCollected);
            Debug.WriteLine("Remaining items: " + remaining);
        }

        public void LoadNextLevel()
        {
            _levelIndex++;
        }

        public void Dispose()
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _levelState.Tiles.ForEach(tile => tile.Draw(gameTime, spriteBatch));

            _levelState.Collectables.ForEach(c => c.Draw(gameTime, spriteBatch));

            _levelState.Player.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            var endTile = _levelState.GetEndTile();
            if (_levelState.Player.Boundaries.Intersects(endTile.Boundaries))
            {
                if (_levelCompleted != null)
                {
                    _levelCompleted();
                }
            }
            else
            {
                _levelState.Tiles.ForEach(tile => tile.Update(gameTime, keyboardState));

                _levelState.Player.Update(gameTime, keyboardState);

                _levelState.Collectables.ForEach(c => c.Update(gameTime, keyboardState));
            }
        }
    }
}
