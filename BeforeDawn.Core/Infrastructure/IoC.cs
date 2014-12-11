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
    }
}
