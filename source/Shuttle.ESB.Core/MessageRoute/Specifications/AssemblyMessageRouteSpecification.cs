using System;
using System.IO;
using System.Reflection;

namespace Shuttle.ESB.Core
{
    public class AssemblyMessageRouteSpecification : TypeListMessageRouteSpecification
    {
        public AssemblyMessageRouteSpecification(Assembly assembly)
        {
            AddAssemblyTypes(assembly);
        }

        public AssemblyMessageRouteSpecification(string assembly)
        {
            Assembly scanAssembly = null;

            try
            {
                switch (Path.GetExtension(assembly))
                {
                    case ".dll":
                    case ".exe":
                    {
                        scanAssembly = Path.GetDirectoryName(assembly) == AppDomain.CurrentDomain.BaseDirectory
                                       ? Assembly.Load(Path.GetFileNameWithoutExtension(assembly))
                                       : Assembly.LoadFile(assembly);
                        break;
                    }

                    default:
                    {
                        scanAssembly = Assembly.Load(assembly);

                        break;
                    }
                }
            }
            catch
            {
            }

            if (scanAssembly == null)
            {
                throw new MessageRouteSpecificationException(string.Format(ESBResources.AssemblyNotFound, assembly, "AssemblyMessageRouteSpecification"));
            }

            AddAssemblyTypes(scanAssembly);
        }

        private void AddAssemblyTypes(Assembly assembly)
        {
			foreach (var type in assembly.GetTypes())
	        {
		        _messageTypes.Add(type.FullName);
	        }
        }
    }
}