using System;

namespace BeforeDawn.Core.Adapters.Abstract
{
    internal interface IStreamReaderAdapter : IDisposable
    {
        IStreamReaderAdapter ReadStream(IStreamAdapter streamAdapter);
        string ReadLine();
    }
}