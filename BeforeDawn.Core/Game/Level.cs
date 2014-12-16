using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Exceptions;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
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
        private int _levelIndex;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIoC _ioc;
        private TimeSpan _timeRemaining;
        private readonly List<Texture2D> _textureLayers;
        private readonly List<ITile> _tiles;
        private readonly IStreamReaderAdapter _streamReader;
        private readonly ITimeSpanAdapter _timeSpan;
        private Player _player;

        public IContentManagerAdapter Content { get; private set; }

        public Level(IContentManagerAdapter contentManager, IServiceProvider serviceProvider, IIoC ioc, IStreamReaderAdapter streamReader, ITimeSpanAdapter timeSpan)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");
            if (ioc == null) throw new ArgumentNullException("ioc");
            if (streamReader == null) throw new ArgumentNullException("streamReader");
            if (timeSpan == null) throw new ArgumentNullException("timeSpan");

            contentManager.Create(serviceProvider, "Content");
            Content = contentManager;
            _serviceProvider = serviceProvider;
            _ioc = ioc;
            _streamReader = streamReader;
            _timeSpan = timeSpan;

            _textureLayers = new List<Texture2D>();
            _tiles = new List<ITile>();
        }
        
        public void Initialize(IStreamAdapter levelLayoutStream, int levelIndex)
        {
            _levelIndex = levelIndex;
            _timeRemaining = _timeSpan.FromMinutes(2.0);

            LoadTiles(levelLayoutStream);

            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            _player = _ioc.Resolve<Player>();

            var startTile = _tiles.Single(tile => tile.IsStartTile);

            //_player.Initialize(startTile.Location);
            _player.Initialize(new Vector2(200f, 200f));
        }

        private void LoadTiles(IStreamAdapter fileStream)
        {
            var lines = ReadLines(fileStream);

            var matches = ExtractTiles(lines.ToList());

            _tiles.AddRange(matches.Select(match =>
            {
                var tile = _ioc.Resolve<ITile>();
                tile.Initialize(match);
                return tile;
            }));

            EnsureHasStartPosition();
            EnsureHasOnlyOneStartPosition();

            EnsureHasEndPosition();
            EnsureHasOnlyOneEndPosition();
        }

        private void EnsureHasOnlyOneStartPosition()
        {
            var nrOfTiles = _tiles.Count(tile => tile.IsStartTile);
            if (nrOfTiles > 1)
            {
                throw new RequiredGameElementMissingException("Level " + _levelIndex + " has too many start positions (" + nrOfTiles + "). Can only have one.");
            }
        }

        private void EnsureHasOnlyOneEndPosition()
        {
            var nrOfTiles = _tiles.Count(tile => tile.IsEndTile);
            if (nrOfTiles > 1)
            {
                throw new RequiredGameElementMissingException("Level " + _levelIndex + " has too many end positions (" + nrOfTiles + "). Can only have one.");
            }
        }

        private void EnsureHasStartPosition()
        {
            var hasTile = _tiles.Any(tile => tile.IsStartTile);
            if (!hasTile)
            {
                throw new RequiredGameElementMissingException("Missing start position tile");
            }
        }

        private void EnsureHasEndPosition()
        {
            var hasTile = _tiles.Any(tile => tile.IsEndTile);
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

                var matches = new Regex("([a-zA-Z0-9])+").Matches(lines[i]);

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
            int width;
            var lines = new List<string>();

            using (var reader = _streamReader.ReadStream(fileStream))
            {
                var line = reader.ReadLine();

                width = line.Length;

                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            return lines;
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
            _tiles.ForEach(tile => tile.Draw(gameTime, spriteBatch));

            _player.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _player.Update(gameTime, keyboardState);
        }
    }
}
