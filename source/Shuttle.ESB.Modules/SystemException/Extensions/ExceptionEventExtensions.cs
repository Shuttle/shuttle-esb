using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Modules.Events;

namespace Shuttle.ESB.Modules.Extensions
{
	public static class ExceptionEventExtensions
	{
		public static HandlerExceptionEvent CoreHandlerExceptionEvent(Exception ex, string hostName, string[] ipAddresses)
		{
			Guard.AgainstNull(ex, "ex");

			return new HandlerExceptionEvent
					{
						Source = ex.Source,
						Message=ex.CompactMessages(),
						StackTrace = ex.ToString(),
						ExceptionTypeFullName = ex.GetType().FullName,
						TargetSite = ex.TargetSite.ToString(),
						TargetSiteName = ex.TargetSite.Name,
						TargetSiteMemberType = ex.TargetSite.MemberType.ToString(),
						TargetSiteDeclaringTypeFullName = ex.TargetSite.DeclaringType.ToString(),
						TargetSiteModuleFullyQualifiedName = ex.TargetSite.Module.FullyQualifiedName,
						TargetSiteModuleAssemblyFullName = ex.TargetSite.Module.Assembly.FullName,
						DateThrown = DateTime.Now,
						MachineName = Environment.MachineName,
						HostName = hostName,
						IPAddresses = ipAddresses
					};
		}

		public static PipelineExceptionEvent CorePipelineExceptionEvent(Exception ex, string hostName, string[] ipAddresses)
		{
			Guard.AgainstNull(ex, "ex");

			return new PipelineExceptionEvent
					{
						Source = ex.Source,
						Message=ex.CompactMessages(),
						StackTrace = ex.ToString(),
						ExceptionTypeFullName = ex.GetType().FullName,
						TargetSite = ex.TargetSite.ToString(),
						TargetSiteName = ex.TargetSite.Name,
						TargetSiteMemberType = ex.TargetSite.MemberType.ToString(),
						TargetSiteDeclaringTypeFullName = ex.TargetSite.DeclaringType.ToString(),
						TargetSiteModuleFullyQualifiedName = ex.TargetSite.Module.FullyQualifiedName,
						TargetSiteModuleAssemblyFullName = ex.TargetSite.Module.Assembly.FullName,
						DateThrown = DateTime.Now,
						MachineName = Environment.MachineName,
						HostName = hostName,
						IPAddresses = ipAddresses
					};
		}
	}
}