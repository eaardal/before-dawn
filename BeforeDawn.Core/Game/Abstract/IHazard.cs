namespace BeforeDawn.Core.Game.Abstract
{
    internal interface IHazard
    {
        string Hazard { get; }
        bool IsActive { get; }
    }
}