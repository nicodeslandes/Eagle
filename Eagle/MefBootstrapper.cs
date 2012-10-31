﻿using Caliburn.Micro;
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
    using System.IO;
    using System.Reflection;
    using Eagle.FilePicker.Views;

    public class MefBootstrapper : Bootstrapper<IShell>
    {
#if LOG_DESIGN_TIME
        private const string LogFileName = @"D:\Nicolas\Desktop\logs\file.log";
#endif
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
#if LOG_DESIGN_TIME
            File.AppendAllText(LogFileName, "Starting design-time bootstrapper\n");
            try
            {
#endif
                base.StartDesignTime();

                base.Configure();

                AssemblySource.Instance.Clear();
                AssemblySource.Instance.AddRange(new[] { typeof(App).Assembly });

                var originalLocateTypeForModelType = ViewLocator.LocateTypeForModelType;
                Func<Type, bool> isDesignTimeType = type => type.Assembly.IsDynamic;
                ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
                    {
#if LOG_DESIGN_TIME
                        File.AppendAllText(
                            LogFileName, "Looking for " + modelType + '\n');
#endif
                        var type = originalLocateTypeForModelType(modelType, displayLocation, context);
                        if (type == null && isDesignTimeType(modelType))
                        {
#if LOG_DESIGN_TIME
                            File.AppendAllText(
                                LogFileName, "Looking for " + modelType + '\n');
#endif
                            if (modelType.Name == "RecentItemsFolderViewModel")
                            {
                                type = typeof(RecentItemsFolderView);
                            }

                            if (modelType.Name == "FileLocationViewModel")
                            {
                                type = typeof(FileLocationView);
                            }

                            if (modelType.Name == "FilePickerViewModel")
                            {
                                type = typeof(FilePickerView);
                            }
                        }
                        return type;
                    };

                IoC.GetInstance = base.GetInstance;
                IoC.GetAllInstances = base.GetAllInstances;
#if LOG_DESIGN_TIME
                File.AppendAllText(
                    LogFileName, "Done intialising design-time bootstrapper\n");
            }
            catch (ReflectionTypeLoadException rex)
            {
                File.AppendAllText(
                    LogFileName, "Error: intialising design-time bootstrapper:\n" + rex + '\n');
                File.AppendAllText(
                    LogFileName, rex.LoaderExceptions[0].ToString() + '\n');
            }
            catch (Exception ex)
            {
                File.AppendAllText(
                    LogFileName, "Error: intialising design-time bootstrapper:\n" + ex + '\n');
            }
#endif
        }
    }
}
