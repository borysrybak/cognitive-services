﻿using Caliburn.Micro;
using RealTimeCrowdInsights.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RealTimeCrowdInsights
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;
        
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.PerRequest<ShellViewModel>();

            IoC.GetInstance = GetInstance;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
