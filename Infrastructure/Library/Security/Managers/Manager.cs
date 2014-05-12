using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Managers
{
    public class Manager : IManager
    {
        private readonly IContainer _container;

        public Manager(IContainer container)
        {
            _container = container;
            Populate();
        }

        private void Populate()
        {
            _container.Configure(x =>
                x.Scan(s =>
                {
                    s.AssembliesFromApplicationBaseDirectory();
                    s.AddAllTypesOf<IDescriptor>();
                }));
        }

        public AuthorizationResult Authorize<TAction>(object instance) where TAction : IAction
        {
            var result = new AuthorizationResult();

            var descriptors = _container.GetAllInstances<IDescriptor>();

            if (!descriptors.Any())
                return result;

            var applicableSecurityDescriptors = descriptors.Where(sd => sd.CanAuthorize<TAction>(instance));

            if (!applicableSecurityDescriptors.Any())
                return result;

            foreach (var securityDescriptor in applicableSecurityDescriptors)
                result.ProcessAuthorizeDescriptorResult(securityDescriptor.Authorize(instance));

            return result;
        }
    }
}