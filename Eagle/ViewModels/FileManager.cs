using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.ComponentModel.Composition;

namespace Eagle.ViewModels
{
    [Export(typeof(IFileManager))]
    [Export(typeof(IFileManagerEventSource))]
    public class FileManager : IFileManager, IFileManagerEventSource, IDisposable
    {
        readonly Subject<string> _openFileEventStream = new Subject<string>();

        public void OpenFile(string fileName)
        {
            _openFileEventStream.OnNext(fileName);
        }

        public IObservable<string> OpenFileEventStream
        {
            get { return _openFileEventStream.AsObservable(); }
        }

        public void Dispose()
        {
            _openFileEventStream.Dispose();
        }
    }
}
