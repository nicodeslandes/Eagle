namespace Eagle.ViewModels
{
    public interface IStateCaptureContext
    {
        void SaveState(string key, object state);
    }
}
