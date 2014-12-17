using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeforeDawn.Core.Game.Helpers
{
    class TilePlacement
    {
        public static Vector2 CalculateLocationForTileLayout(int tileLayoutX, int tileLayoutY, Texture2D texture)
        {
            var x = (tileLayoutX - 1) * texture.Width;
            var y = (tileLayoutY - 1) * texture.Height;
            return new Vector2(x, y);
        }
    }
}
