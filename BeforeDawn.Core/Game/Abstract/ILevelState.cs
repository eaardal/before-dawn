﻿using System.Collections.Generic;

namespace BeforeDawn.Core.Game.Abstract
{
    internal interface ILevelState
    {
        List<ITile> Tiles { get; }
        Player Player { get; set; }
        List<ICollectable> Collectables { get; }
        void ResetAllState();
        ITile GetStartTile();
        ITile GetEndTile();
    }
}