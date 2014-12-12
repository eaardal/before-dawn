using System;
using BeforeDawn.Core.Adapters.Abstract;
using Microsoft.Xna.Framework.Content;

namespace BeforeDawn.Core.Adapters
{
    class ContentManagerAdapter : IContentManagerAdapter
    {
        private ContentManager _contentManager;

        public void Create(IServiceProvider serviceProvider, string rootPath)
        {
            _contentManager = new ContentManager(serviceProvider, rootPath);
        }

        public T Load<T>(string assetName)
        {
            if (_contentManager == null)
            {
                throw new NullReferenceException("Tried to use ContentManager before it was created");
            }

            return _contentManager.Load<T>(assetName);
        }
    }
}