using Caliburn.Micro;
using Caliburn.Micro.Logging.NLog;
using Eagle.FilePicker.ViewModels;
using Eagle.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Eagle
{
    public class MefBootstrapper : Bootstrapper<IShell>
    {
        private CompositionContainer container;
        
        static MefBootstrapper()
        {
            LogManager.GetLog = type => new NLogLogger(type);
        }

        protected override void Configure()
        {
            container = new CompositionContainer(
                new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)))
                );

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            //batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Count() > 0)
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        protected override void StartDesignTime()
        {
            base.StartDesignTime();

            base.Configure();

            AssemblySource.Instance.Clear();
            AssemblySource.Instance.AddRange(new[] { typeof(App).Assembly });

            var originalLocateTypeForModelType = ViewLocator.LocateTypeForModelType;
            Func<Type, bool> isDesignTimeType = type => type.Assembly.IsDynamic;
            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {
                var type = originalLocateTypeForModelType(modelType, displayLocation, context);
                if (type == null && isDesignTimeType(modelType))
                {
                    if (modelType.Name == "RecentItemsFolderViewModel")
                    {
                        type = typeof(RecentItemsFolderViewModel);
                    }
                }
                return type;
            };

            IoC.GetInstance = base.GetInstance;
            IoC.GetAllInstances = base.GetAllInstances;
        }
    }
}
