using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeforeDawn.Core.Game.Tiles
{
    class IceTile
    {
        // Goes forward until hits a guiding obstacle that changes direction. Can't intercept forced movement
        // in any way. If hits a wall or obstacle that is not the "guiding obstacle", reverses direction.
    }
}
