using System.Collections.Generic;
using System.Linq;
using BeforeDawn.Core.Game.Abstract;

namespace BeforeDawn.Core.Game
{
    class LevelState : ILevelState
    {
        public List<ITile> Tiles { get; private set; }
        public List<ICollectable> Collectables { get; set; }
        public List<IDoor> Doors { get; private set; }
        public List<DoorKey> DoorKeys { get; private set; }

        public Player Player { get; set; }

        public LevelState()
        {
            ResetAllState();
        }

        public void ResetAllState()
        {
            Tiles = new List<ITile>();
            Collectables = new List<ICollectable>();
            Doors = new List<IDoor>();
            DoorKeys = new List<DoorKey>();
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
