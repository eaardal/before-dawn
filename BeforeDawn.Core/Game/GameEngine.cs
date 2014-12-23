using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Messages;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game
{
    class GameEngine : ILoadContent, IDraw, IUpdate
    {
        private readonly IIoC _ioc;
        private ILevel _currentLevel;
        private int _levelIndex;
        private int _numberOfLevels;
        private readonly ITitleContainerAdapter _titleContainer;
        private readonly IMessageBus _messageBus;

        public GameEngine(IIoC ioc, ITitleContainerAdapter titleContainer, IMessageBus messageBus)
        {
            if (ioc == null) throw new ArgumentNullException("ioc");
            if (titleContainer == null) throw new ArgumentNullException("titleContainer");
            if (messageBus == null) throw new ArgumentNullException("messageBus");

            _ioc = ioc;
            _titleContainer = titleContainer;
            _messageBus = messageBus;

            _messageBus.Subscribe<PlayerDied>(OnPlayerDied);
        }

        private void OnPlayerDied(PlayerDied obj)
        {
            Debug.WriteLine("Player died :(");
            ReloadCurrentLevel();
        }

        public void LoadContent(ISpriteBatchAdapter spriteBatch)
        {
            _numberOfLevels = FindNumberOfLevels();

            if (_levelIndex < _numberOfLevels)
            {
                LoadNextLevel();    
            }
            else if (_levelIndex == _numberOfLevels)
            {
                CompleteGame();
            }
        }

        private void CompleteGame()
        {
            Debug.WriteLine("Game completed!");
        }

        private int FindNumberOfLevels()
        {
            int count = 0;

            try
            {
                var levelIndex = 1;
                while (true)
                {
                    using (var stream = GetLevel(levelIndex))
                    {
                        count++;
                    }
                    levelIndex++;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }

        private IStreamAdapter GetLevel(int index)
        {
            var exeLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
            var location = exeLocation.Substring(0, exeLocation.LastIndexOf("\\", StringComparison.Ordinal));
            var levelPath = String.Format("{0}\\Content\\Levels\\level{1}.txt", location, index);
            return _ioc.Resolve<IStreamAdapter>().WithStream(new FileStream(levelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        private void LoadNextLevel()
        {
            _levelIndex++;

            if (_currentLevel != null)
            {
                _currentLevel.Dispose();
            }

            try
            {
                using (var stream = GetLevel(_levelIndex))
                {
                    _currentLevel = _ioc.Resolve<Level>();
                    _currentLevel.Initialize(stream, _levelIndex, LevelCompleted);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        private void LevelCompleted()
        {
            if (_levelIndex < _numberOfLevels)
            {
                LoadNextLevel();
            }
            else if (_levelIndex == _numberOfLevels)
            {
                CompleteGame();
            }
        }

        private void ReloadCurrentLevel()
        {
            --_levelIndex;
            LoadNextLevel();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _currentLevel.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _currentLevel.Update(gameTime, keyboardState);
        }
    }
}
