using System;
using BeforeDawn.Core.Game.Abstract;

namespace BeforeDawn.Core.Game.Messages
{
    class ItemCollected
    {
        public ICollectable Item { get; private set; }

        public ItemCollected(ICollectable item)
        {
            if (item == null) throw new ArgumentNullException("item");
            Item = item;
        }
    }
}
