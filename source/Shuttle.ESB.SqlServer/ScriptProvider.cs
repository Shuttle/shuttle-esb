using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.SqlServer
{
	public class ScriptProvider : IScriptProvider
	{
		private readonly ISqlServerConfiguration _configuration;
		private static readonly object _padlock = new object();

		private readonly Dictionary<Script, string> _scripts = new Dictionary<Script, string>();

		public ScriptProvider(ISqlServerConfiguration configuration)
		{
			Guard.AgainstNull(configuration, "configuration");

			_configuration = configuration;
		}

		public string GetScript(Script script, params string[] parameters)
		{
			if (!_scripts.ContainsKey(script))
			{
				AddScript(script);
			}

			return parameters != null
					   ? string.Format(_scripts[script], parameters)
					   : _scripts[script];
		}

		private void AddScript(Script script)
		{
			lock (_padlock)
			{
				if (_scripts.ContainsKey(script))
				{
					return;
				}

				var files = new string[0];

				if (Directory.Exists(_configuration.ScriptFolder))
				{
					files = Directory.GetFiles(_configuration.ScriptFolder, script.FileName, SearchOption.AllDirectories);
				}

				if (files.Length == 0)
				{
					AddEmbeddedScript(script);

					return;
				}

				if (files.Length > 1)
				{
					throw new ScriptException(string.Format(SqlResources.ScriptCountException, _configuration.ScriptFolder, script.FileName, files.Length));
				}

				_scripts.Add(script, File.ReadAllText(files[0]));
			}
		}

		private void AddEmbeddedScript(Script script)
		{
			using (var stream =
				Assembly.GetCallingAssembly().GetManifestResourceStream(
					string.Format("Shuttle.ESB.SqlServer.Scripts.{0}", script.FileName)))
			{
				if (stream == null)
				{
					throw new ScriptException(string.Format(SqlResources.EmbeddedScriptMissingException, script.FileName));
				}

				using (var reader = new StreamReader(stream))
				{
					_scripts.Add(script, reader.ReadToEnd());
				}
			}
		}

		public static IScriptProvider Default()
		{
			return new ScriptProvider(new SqlServerConfiguration());
		}
	}
}