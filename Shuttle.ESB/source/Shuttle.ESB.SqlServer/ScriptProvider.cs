using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.SqlServer
{
    public class ScriptProvider : IScriptProvider
    {
        private static readonly object padlock = new object();

        public string ScriptBatchSeparator { get; private set; }
        public string ScriptFolder { get; private set; }

        private readonly Dictionary<Script, string> scripts = new Dictionary<Script, string>();

        public ScriptProvider()
        {
            ScriptFolder = ConfigurationItem<string>.ReadSetting("ScriptFolder", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts")).GetValue();
            ScriptBatchSeparator = ConfigurationItem<string>.ReadSetting("ScriptBatchSeparator", "GO").GetValue();
        }

        public string GetScript(Script script, params string[] parameters)
        {
            if (!scripts.ContainsKey(script))
            {
                AddScript(script);
            }

            return parameters != null
                       ? string.Format(scripts[script], parameters)
                       : scripts[script];
        }

        private void AddScript(Script script)
        {
            lock (padlock)
            {
                if (scripts.ContainsKey(script))
                {
                    return;
                }

                var files = new string[0];

                if (Directory.Exists(ScriptFolder))
                {
                    files = Directory.GetFiles(ScriptFolder, script.FileName, SearchOption.AllDirectories);
                }

                if (files.Length == 0)
                {
                    AddEmbeddedScript(script);

                    return;
                }

                if (files.Length > 1)
                {
                    throw new ScriptException(string.Format(SqlResources.ScriptCountException, ScriptFolder,
                                                            script.FileName, files.Length));
                }

                scripts.Add(script, File.ReadAllText(files[0]));
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
                    scripts.Add(script, reader.ReadToEnd());
                }
            }
        }
    }
}