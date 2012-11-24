using System;

namespace Eagle.ViewModels
{
    public interface IFileManagerEventSource
    {
        IObservable<string> OpenFileEventStream { get; }
    }
}
