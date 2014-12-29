namespace BeforeDawn.Core.Game.Abstract
{
    interface IUsableInventoryItem : IInventoryItem
    {
        void Use(params object[] parameters);
    }
}