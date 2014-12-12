using System;
using BeforeDawn.Core.Adapters.Abstract;
using BeforeDawn.Core.Infrastructure;
using Microsoft.Xna.Framework;

namespace BeforeDawn.Core.Adapters
{
    class TitleContainerAdapter : ITitleContainerAdapter
    {
        private readonly IIoC _ioc;

        public TitleContainerAdapter(IIoC ioc)
        {
            if (ioc == null) throw new ArgumentNullException("ioc");
            _ioc = ioc;
        }

        public IStreamAdapter OpenStream(string name)
        {
            var stream = TitleContainer.OpenStream(name);
            
            var streamAdapter =_ioc.Resolve<IStreamAdapter>();

            return streamAdapter.WithStream(stream);
        }
    }
}
