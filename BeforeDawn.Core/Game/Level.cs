﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BeforeDawn.Core.Exceptions;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Adapters.Abstract;
using BeforeDawn.Core.Game.Helpers;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        public Level(IContentManagerAdapter contentManager, IServiceProvider serviceProvider, IIoC ioc)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");
            if (ioc == null) throw new ArgumentNullException("ioc");

            contentManager.Create(serviceProvider, "Content");
            Content = contentManager;
            _serviceProvider = serviceProvider;
            _ioc = ioc;

            _textureLayers = new List<Texture2D>();
            _tiles = new List<ITile>();
        }

        public IContentManagerAdapter Content { get; private set; }

        public void Initialize(Stream levelLayoutStream, int levelIndex)
        {
            _levelIndex = levelIndex;
            _timeRemaining = TimeSpan.FromMinutes(2.0);

            LoadTiles(levelLayoutStream);
        }
        
        private void LoadTiles(Stream fileStream)
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
            EnsureHasEndPosition();
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
                        TileType = match.Value.ToCharArray()[0],
                        X = xCounter,
                        Y = yCounter
                    });
                }

                xCounter = 0;
            }

            return tiles;
        }

        private IEnumerable<string> ReadLines(Stream fileStream)
        {
            int width;
            var lines = new List<string>();

            using (var reader = new StreamReader(fileStream))
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
    }
}