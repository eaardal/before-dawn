using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Adapters.Abstract;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeforeDawn.Core.Game
{
    class GameEngine : ILoadContent
    {
        private readonly IIoC _ioc;
        private ILevel _currentLevel;
        private int _levelIndex;
        private int _numberOfLevels;

        public GameEngine(IIoC ioc)
        {
            if (ioc == null) throw new ArgumentNullException("ioc");
            _ioc = ioc;
        }

        public void LoadContent(ISpriteBatchAdapter spriteBatch)
        {
            _numberOfLevels = 1;

            LoadNextLevel();
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
                using (var stream = TitleContainer.OpenStream("Content/Levels/level" + _levelIndex + ".txt"))
                {
                    _currentLevel = _ioc.Resolve<Level>();
                    _currentLevel.Initialize(stream, _levelIndex);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ReloadCurrentLevel()
        {
            --_levelIndex;
            LoadNextLevel();
        }

    }
}
