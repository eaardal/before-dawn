using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeforeDawn.Core.Game.Helpers
{
    class TilePlacement
    {
        public static Vector2 CalculateLocationForTileLayout(int tileLayoutX, int tileLayoutY, Rectangle size)
        {
            var x = (tileLayoutX - 1) * size.Width;
            var y = (tileLayoutY - 1) * size.Height;
            return new Vector2(x, y);
        }
    }
}
