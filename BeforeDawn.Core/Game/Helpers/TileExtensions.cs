using System.Collections.Generic;
using System.Linq;
using BeforeDawn.Core.Game.Abstract;

namespace BeforeDawn.Core.Game.Helpers
{
    static class TileExtensions
    {
        public static IEnumerable<IEnumerable<ITile>> AsLayout(this IEnumerable<ITile> tiles)
        {
            return tiles.GroupBy(tile => tile.TileLayoutY).Select(tile => tile.ToList()).ToList();
        }

        public static IEnumerable<ITile> GetLayoutRow(this IEnumerable<ITile> tiles, int row)
        {
            var layout = tiles.AsLayout();
            return layout.ElementAt((row - 1));
        }

        public static IEnumerable<ITile> GetLayoutRow(this IEnumerable<IEnumerable<ITile>> tiles, int row)
        {
            return tiles.ElementAt((row - 1));
        }

        public static IEnumerable<ITile> GetLayoutColumn(this IEnumerable<ITile> tiles, int row)
        {
            var column = new List<ITile>();
            var layout = tiles.AsLayout().ToList();

            var tile = layout.Select(layoutRow => layoutRow.ElementAt((row - 1))).First();

            column.Add(tile);

            return layout.ElementAt(row);
        }

        public static IEnumerable<ITile> GetLayoutColumn(this IEnumerable<IEnumerable<ITile>> tiles, int rowNr)
        {
            return tiles.Select(row => row.ElementAt((rowNr - 1))).ToList();
        }
    }
}
