using System.Windows;
using Caliburn.Micro.DesignTimeSupport.Design;
using Microsoft.Windows.Design.Metadata;

[assembly: ProvideMetadata(typeof(MetadataProvider))]

namespace Caliburn.Micro.DesignTimeSupport.Design
{
    internal class MetadataProvider : IProvideAttributeTable
    {
        public AttributeTable AttributeTable
        {
            get
            {                
                var builder = new AttributeTableBuilder();

                builder.AddCustomAttributes(typeof(DesignTime), "Enable",
                    new AttachedPropertyBrowsableForTypeAttribute(Helper.DependencyObjectType));
                return builder.CreateTable();
            }
        }
    }
}
