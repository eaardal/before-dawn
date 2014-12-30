using System.Collections.Generic;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Game.Abstract;
using BeforeDawn.Core.Game.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Tiles
{
    class StartTile : DefaultTile
    {
        public StartTile(IContentManagerAdapter contentManager, ILevelState levelState) : base(contentManager, levelState)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            
        }

        public override List<string> TileTypes
        {
            get { return new List<string> { TileKinds.Start }; }
        }
    }
}
