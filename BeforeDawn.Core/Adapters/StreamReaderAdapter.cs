using System.IO;
using BeforeDawn.Core.Adapters.Abstract;

namespace BeforeDawn.Core.Adapters
{
    class StreamReaderAdapter : IStreamReaderAdapter
    {
        private StreamReader _reader;

        public IStreamReaderAdapter ReadStream(IStreamAdapter streamAdapter)
        {
            _reader = new StreamReader(streamAdapter.Stream);
            return this;
        }

        public string ReadLine()
        {
            return _reader.ReadLine();
        }

        public void Dispose()
        {
            
        }
    }
}
