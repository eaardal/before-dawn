using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
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

        public GameEngine(IIoC ioc, ITitleContainerAdapter titleContainer)
        {
            if (ioc == null) throw new ArgumentNullException("ioc");
            if (titleContainer == null) throw new ArgumentNullException("titleContainer");

            _ioc = ioc;
            _titleContainer = titleContainer;
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
                    using (var stream = _titleContainer.OpenStream("Content/Levels/level" + levelIndex + ".txt"))
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

        private void LoadNextLevel()
        {
            _levelIndex++;

            if (_currentLevel != null)
            {
                _currentLevel.Dispose();
            }

            try
            {
                using (var stream = _titleContainer.OpenStream("Content/Levels/level" + _levelIndex + ".txt"))
                {
                    _currentLevel = _ioc.Resolve<Level>();
                    _currentLevel.Initialize(stream, _levelIndex, LevelCompleted);
                }
            }
            catch (Exception ex)
            {
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
