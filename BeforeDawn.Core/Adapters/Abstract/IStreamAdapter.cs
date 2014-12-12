using System;
using System.IO;

namespace BeforeDawn.Core.Adapters.Abstract
{
    internal interface IStreamAdapter : IDisposable
    {
        IStreamAdapter WithStream(Stream stream);
        Stream Stream { get; }
    }
}