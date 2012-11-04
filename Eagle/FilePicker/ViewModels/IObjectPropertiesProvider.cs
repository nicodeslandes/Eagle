using System.ComponentModel;

namespace Eagle.FilePicker.ViewModels
{
    public interface IObjectPropertiesProvider
    {
        PropertyCollection GetProperties(INotifyPropertyChanged obj);
    }
}
