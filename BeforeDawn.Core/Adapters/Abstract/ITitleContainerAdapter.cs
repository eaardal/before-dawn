namespace BeforeDawn.Core.Adapters.Abstract
{
    internal interface ITitleContainerAdapter
    {
        IStreamAdapter OpenStream(string name);
    }
}