using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Game.Helpers;

namespace BeforeDawn.Core.Game
{
    internal interface ITile
    {
        void Initialize(TileMatch match);
        char TileType { get; }
        int X { get; }
        int Y { get; }
        bool IsStartTile { get; }
        bool IsEndTile { get; }
    }

    class Tile : ITile
    {
        public char TileType { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public void Initialize(TileMatch tile)
        {
            TileType = tile.TileType;
            X = tile.X;
            Y = tile.Y;
        }

        public bool IsStartTile { get { return TileType == 'S'; } }
        public bool IsEndTile { get { return TileType == 'E'; }}
    }
}
