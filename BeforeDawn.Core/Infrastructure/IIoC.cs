using System;
using Autofac;

namespace BeforeDawn.Core.Infrastructure
{
    internal interface IIoC
    {
        void RegisterContainer(IContainer container);
        T Resolve<T>();
        T Resolve<T>(T type);
        T Resolve<T>(string name);
    }
}