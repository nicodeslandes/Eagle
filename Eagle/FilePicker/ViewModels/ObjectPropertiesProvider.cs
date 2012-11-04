using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Eagle.FilePicker.ViewModels
{
    [Export(typeof(IObjectPropertiesProvider))]
    public class ObjectPropertiesProvider : IObjectPropertiesProvider
    {
        public PropertyCollection GetProperties(INotifyPropertyChanged obj)
        {
            return null;
        }
    }
}
