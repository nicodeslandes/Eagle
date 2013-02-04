using System;
using System.Threading.Tasks;

namespace Eagle.ViewModels
{
    public interface IStateService
    {
        Task InitializeAsync();
        void MarkAsDirty();
        IObservable<IStateCaptureContext> SavingEvent { get; }
        bool TryGetState<T>(string key, out T state) where T : class;
    }
}
