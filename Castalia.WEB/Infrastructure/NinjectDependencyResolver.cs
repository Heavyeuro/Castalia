using Castalia.Domain.Interfaces;
using Castalia.Domain.Repositories;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Castalia.WEB.Infrastructure
{
   
     public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //binding interface to class and defines connection string
            kernel.Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument("connectionString",
                ConfigurationManager.ConnectionStrings["Castalia"].ConnectionString);

        }
    }
}