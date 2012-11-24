using Eagle.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;

namespace Eagle.FilePicker.ViewModels
{
    [Export]
    public class RecentItemsFolderViewModel : FolderViewModel
    {
        private readonly IFileManager _fileManager;

        [ImportingConstructor]
        public RecentItemsFolderViewModel(IFileManager fileManager, IFileManagerEventSource fileManagerEventSource)
            : base("Recent")
        {
            _fileManager = fileManager;
            fileManagerEventSource.OpenFileEventStream
                .ObserveOnDispatcher()
                .Subscribe(this.OnFileOpened);
        }

        private void OnFileOpened(string path)
        {
            var newItem = new FileLocationViewModel(path, _fileManager);
            this.ChildItems.Remove(newItem);
            this.ChildItems.Insert(0, newItem);
        }
    }
}
