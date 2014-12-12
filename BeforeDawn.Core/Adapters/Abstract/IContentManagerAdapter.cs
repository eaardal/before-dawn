using System;

namespace BeforeDawn.Core.Adapters.Abstract
{
    internal interface IContentManagerAdapter
    {
        void Create(IServiceProvider serviceProvider, string rootPath);
        T Load<T>(string assetName);
    }
}