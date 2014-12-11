using Autofac;

namespace BeforeDawn.Core.Infrastructure
{
    internal interface IIoC
    {
        void RegisterContainer(IContainer container);
        T Resolve<T>();
    }
}