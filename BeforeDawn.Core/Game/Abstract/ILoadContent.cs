using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeforeDawn.Core.Adapters.Abstract;

namespace BeforeDawn.Core.Game.Abstract
{
    interface ILoadContent
    {
        void LoadContent(ISpriteBatchAdapter spriteBatch);
    }
}
