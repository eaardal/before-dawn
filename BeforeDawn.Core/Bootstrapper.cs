using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using BeforeDawn.Core.Game;
using BeforeDawn.Core.Game.Abstract;
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
                .Except<ILevelState>()
                .Except<ICamera2D>()
                .Except<IMessageBus>()
                .AsSelf()
                .Named<ITile>(t => t.FullName)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(a => a.Name.EndsWith("Adapter"))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<IoC>().As<IIoC>().SingleInstance();

            builder.RegisterInstance(game)
                .AsSelf()
                .AsImplementedInterfaces()
                .As<Microsoft.Xna.Framework.Game>()
                .SingleInstance();

            builder.Register<IServiceProvider>(p => game.Services);

            builder.RegisterType<MessageBus>().As<IMessageBus>().SingleInstance();

            builder.RegisterType<LevelState>().As<ILevelState>().SingleInstance();

            builder.Register<ICamera2D>(c => new Camera2D(game))
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
            
            
            var container = builder.Build();

            var ioc = container.Resolve<IIoC>();
            ioc.RegisterContainer(container);

            return ioc;
        }
    }
}
