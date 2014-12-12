using System;
using System.Reflection;
using Autofac;
using BeforeDawn.Core.Game;
using BeforeDawn.Core.Infrastructure;

namespace BeforeDawn.Core
{
    class Bootstrapper
    {
        public static IIoC Wire(Microsoft.Xna.Framework.Game game)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Except<IIoC>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(a => a.Name.EndsWith("Adapter"))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register<IIoC>(i => new IoC()).SingleInstance();

            builder.Register<IServiceProvider>(p => game.Services);
            
            var container = builder.Build();

            var ioc = container.Resolve<IIoC>();
            ioc.RegisterContainer(container);

            return ioc;
        }
    }
}
