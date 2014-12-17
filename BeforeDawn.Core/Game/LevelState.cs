using System.Collections.Generic;
using System.Linq;
using BeforeDawn.Core.Game.Abstract;

namespace BeforeDawn.Core.Game
{
    class LevelState : ILevelState
    {
        public List<ITile> Tiles { get; private set; }

        public List<ICollectable> Collectables { get; set; }

        public Player Player { get; set; }

        public LevelState()
        {
            ResetAllState();
        }

        public void ResetAllState()
        {
            Tiles = new List<ITile>();
            Collectables = new List<ICollectable>();
            Player = null;
        }

        public ITile GetStartTile()
        {
            return Tiles.Single(tile => tile.IsStartTile);
        }

        public ITile GetEndTile()
        {
            return Tiles.Single(tile => tile.IsEndTile);
        }
    }
}
