using System.Configuration;

namespace Shuttle.ESB.Core
{
	public static class ShuttleConfigurationSection
	{
		public static T Open<T>(string name, string file) where T : class
		{
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file));

			var group = configuration.GetSectionGroup("shuttle");

			var section = group == null ? configuration.GetSection(name) as T : group.Sections[name] as T;

			if (section == null)
			{
				throw new ConfigurationErrorsException(string.Format(ESBResources.OpenSectionException, name, file));
			}

			return section;
		}
	}
}