namespace BeforeDawn.Core.Game.Abstract
{
    internal interface IDoorKey : ICollectable, IUsableInventoryItem
    {
        string Door { get; }
    }
}