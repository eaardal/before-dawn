using System;
using System.Linq;
using Autofac;

namespace BeforeDawn.Core.Infrastructure
{
    class IoC : IIoC
    {
        private IContainer _container;

        public void RegisterContainer(IContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(T type)
        {
            return (T)_container.Resolve(typeof (T));
        }

        public T Resolve<T>(string name)
        {
            return _container.ResolveNamed<T>(name);
        }
    }
}
