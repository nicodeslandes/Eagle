using System;
using NUnit.Framework;
using Eagle.FilePicker.ViewModels;
using Moq;

namespace Eagle.Test.FilePicker
{
    [TestFixture]
    public class FilePickerViewModelTests
    {
        private Mock<IObjectPropertiesProvider> _objectPropertyProvider;
        [SetUp]
        void Setup()
        {
            _objectPropertyProvider = new Mock<IObjectPropertiesProvider>();
        }

        [Test]
        public void TestViewModelInitialization()
        {
            var vm = CreateViewModel();
            Assert.IsNotNull(vm.Items);
            Assert.AreEqual(4, vm.Items.Count);
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
            return new FilePickerViewModel(_objectPropertyProvider.Object);
        }
    }
}
