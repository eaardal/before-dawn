using System.IO;
using BeforeDawn.Core.Adapters.Abstract;

namespace BeforeDawn.Core.Adapters
{
    class StreamAdapter : IStreamAdapter
    {
        private Stream _stream;

        public Stream Stream { get { return _stream; }}

        public void Dispose()
        {
            
        }

        public IStreamAdapter WithStream(Stream stream)
        {
            _stream = stream;
            return this;
        }
    }
}
