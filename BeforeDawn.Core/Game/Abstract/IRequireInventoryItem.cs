namespace BeforeDawn.Core.Game.Abstract
{
    interface IRequireInventoryItem
    {
        bool HasRequiredItem { get; }
    }

    internal interface IRequireInventoryItem<T> : IRequireInventoryItem
    {
        T Item { get; }
    }
}