using Caliburn.Micro;
using Eagle.Common.ViewModels;
using Eagle.FilePicker.ViewModels;
using NUnit.Framework;
using System;
using System.Linq;

namespace Eagle.Test.FilePicker
{
    [TestFixture]
    public class ObjectPropertiesProviderTest
    {
        [Test]
        public void IfNoMarkedPropertiesOnAnalysedObjectTheProviderReturnsAnEmptyList()
        {
            var provider = this.CreateProvider();
            var properties = provider.GetProperties(new NoMarkedProperties());
            Assert.AreEqual(0, properties.Items.Count);
        }

        [Test]
        public void ProviderReturnsAllMarkedProperties()
        {
            var provider = this.CreateProvider();
            var properties = provider.GetProperties(new ClassWithMarkedProperties());
            CollectionAssert.AreEquivalent(new[] { "Age", "Name" }, properties.Items.Select(p => p.PropertyName));
        }

        [Test]
        public void PropertiesValueAreKeptInSyncWithSource()
        {
            var provider = this.CreateProvider();
            var source = new ClassWithMarkedProperties { Name = "Nicolas", Age = 30 };
            var properties = provider.GetProperties(source);
            PropertyValue<string> nameProperty = (PropertyValue<string>)properties.Items.Single(p => p.PropertyName == "Name");
            PropertyValue<int> ageProperty = (PropertyValue<int>)properties.Items.Single(p => p.PropertyName == "Age");
            
            CollectionAssert.AreEquivalent(new[] { "Age", "Name" }, properties.Items.Select(p => p.PropertyName));
            Assert.AreEqual("Nicolas", nameProperty.Value);
            Assert.AreEqual(30, ageProperty.Value);

            source.Name = "Sophie";
            source.Age = 20;
            Assert.AreEqual("Sophie", nameProperty.Value);
            Assert.AreEqual(20, ageProperty.Value);

            nameProperty.Value = "Regis";
            ageProperty.Value = 28;
            Assert.AreEqual("Regis", source.Name);
            Assert.AreEqual(28, source.Age);
        }

        private ObjectPropertiesProvider CreateProvider()
        {
            return new ObjectPropertiesProvider();
        }

        private class NoMarkedProperties : PropertyChangedBase
        {
            public string AString { get; set; }
        }

        private class ClassWithMarkedProperties : PropertyChangedBase
        {
            public static string NameProperty = Property.Name<ClassWithMarkedProperties>(x => x.Name);
            public static string AddressProperty = Property.Name<ClassWithMarkedProperties>(x => x.Address);
            public static string AgeProperty = Property.Name<ClassWithMarkedProperties>(x => x.Age);

            private string _address;
            private int _age;
            private string _name;

            [UIVisible]
            public string Name
            {
                get { return _name; }

                set
                {
                    if (_name != value)
                    {
                        _name = value;
                        this.NotifyOfPropertyChange(NameProperty);
                    }
                }
            }

            [UIVisible]
            public int Age
            {
                get { return _age; }

                set
                {
                    if (_age != value)
                    {
                        _age = value;
                        this.NotifyOfPropertyChange(AgeProperty);
                    }
                }
            }

            public string Address
            {
                get { return _address; }

                set
                {
                    if (_address != value)
                    {
                        _address = value;
                        this.NotifyOfPropertyChange(AddressProperty);
                    }
                }
            }
            
            
            
        }
    }
}
