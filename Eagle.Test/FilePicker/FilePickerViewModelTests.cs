using NUnit.Framework;
using Eagle.FilePicker.ViewModels;
using Moq;
using Eagle.ViewModels;

namespace Eagle.Test.FilePicker
{
    [TestFixture]
    public class FilePickerViewModelTests
    {
        private Mock<IObjectPropertiesProvider> _objectPropertyProvider;
        private Mock<IFileManager> _fileManager;

        [SetUp]
        public void Setup()
        {
            _objectPropertyProvider = new Mock<IObjectPropertiesProvider>();
            _fileManager = new Mock<IFileManager>();
        }

        [Test]
        public void TestViewModelInitialization()
        {
            var vm = CreateViewModel();
            Assert.IsNotNull(vm.Items);
            Assert.AreEqual(1, vm.Items.Count);
        }

        [Test]
        public void FilePickerUsesPropertyProviderWhenChangingSelectedItem()
        {
            var vm = CreateViewModel();
            var item = new Mock<IFilePickerItem>();
            vm.SelectedItem = item.Object;
            _objectPropertyProvider.Verify(o => o.GetProperties(item.Object), Times.Once());
        }

        private FilePickerViewModel CreateViewModel()
        {
            return new FilePickerViewModel(_objectPropertyProvider.Object, _fileManager.Object, null, null);
        }
    }
}
