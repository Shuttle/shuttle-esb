using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DefaultSerializer : ISerializer
    {
        private const string SOAP_INCLUDE = "SoapInclude";
        private const string TYPE_ERROR = "--- could not retrieve type name from exception ---";
        private const string XML_INCLUDE = "XmlInclude";

        private static readonly Regex expression = new Regex(@"The\stype\s(?<Type>[\w\\.]*)");

        private static readonly object padlock = new object();

        private readonly List<Type> knownTypes = new List<Type>();
        private readonly XmlWriterSettings xmlSettings;
        private Dictionary<string, XmlSerializer> serializers = new Dictionary<string, XmlSerializer>();
        private Dictionary<string, XmlSerializerNamespaces> serializerNamespaces = new Dictionary<string, XmlSerializerNamespaces>();

        public DefaultSerializer()
        {
            xmlSettings = new XmlWriterSettings
                              {
                                  Encoding = Encoding.UTF8,
                                  OmitXmlDeclaration = true,
                                  Indent = true
                              };

            AddKnownType(typeof(TransportMessage));
            AddKnownType(typeof(TransportHeader));
        }

        private void AddKnownType(Type type)
        {
            Guard.AgainstNull(type, "type");

            if (HasKnownType(type))
            {
                return;
            }

            lock (padlock)
            {
                if (HasKnownType(type))
                {
                    return;
                }

                knownTypes.Add(type);

                foreach (var nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!HasKnownType(nested))
                    {
                        AddKnownType(nested);
                    }
                }

                ResetSerializers();
            }
        }

        public bool HasKnownType(Type type)
        {
            return knownTypes.Find(candidate => candidate.AssemblyQualifiedName.Equals(type.AssemblyQualifiedName, StringComparison.InvariantCultureIgnoreCase)) != null;
        }

        public Stream Serialize(object message)
        {
            Guard.AgainstNull(message, "message");

            var messageType = message.GetType();

            AddKnownType(messageType);

            var serializer = GetSerializer(messageType);

            var xml = new StringBuilder();

            using (var writer = XmlWriter.Create(xml, xmlSettings))
            {
                try
                {
                    serializer.Serialize(writer, message, GetSerializerNamespaces(messageType));
                }
                catch (InvalidOperationException ex)
                {
                    var exception = UnknownTypeException(ex);

                    if (exception != null)
                    {
                        throw new SerializerUnknownTypeExcption(GetTypeName(exception));
                    }

                    throw;
                }

                writer.Flush();
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(xml.ToString()));
        }

        public object Deserialize(Type type, Stream stream)
        {
            Guard.AgainstNull(type, "type");
            Guard.AgainstNull(stream, "stream");

            using (var copy = stream.Copy())
            using (var reader = XmlDictionaryReader.CreateTextReader(copy, Encoding.UTF8,
                                                                  new XmlDictionaryReaderQuotas
                                                                      {
                                                                          MaxArrayLength = Int32.MaxValue,
                                                                          MaxStringContentLength = int.MaxValue,
                                                                          MaxNameTableCharCount = int.MaxValue
                                                                      }, null))
            {
                return GetSerializer(type).Deserialize(reader);
            }
        }

        private void ResetSerializers()
        {
            serializers = new Dictionary<string, XmlSerializer>();
            serializerNamespaces = new Dictionary<string, XmlSerializerNamespaces>();
        }

        private static string GetTypeName(Exception exception)
        {
            var match = expression.Match(exception.Message);

            var group = match.Groups["Type"];

            return group == null
                       ? TYPE_ERROR
                       : (!string.IsNullOrEmpty(group.Value)
                              ? group.Value
                              : TYPE_ERROR);
        }

        private static Exception UnknownTypeException(Exception exception)
        {
            var ex = exception;

            while (ex != null)
            {
                if (ex.Message.Contains(XML_INCLUDE) || ex.Message.Contains(SOAP_INCLUDE))
                {
                    return ex;
                }

                ex = ex.InnerException;
            }

            return null;
        }

        private XmlSerializer GetSerializer(Type type)
        {
            lock (padlock)
            {
                var key = type.AssemblyQualifiedName;

                if (!serializers.ContainsKey(key))
                {
                    serializers.Add(key, new XmlSerializer(type, knownTypes.ToArray()));
                }

                return serializers[key];
            }
        }

        private XmlSerializerNamespaces GetSerializerNamespaces(Type type)
        {
            lock (padlock)
            {
                var key = type.AssemblyQualifiedName;

                if (!serializerNamespaces.ContainsKey(key))
                {
                    var namespacesAdded = new List<string>();
                    var namespaces = new XmlSerializerNamespaces();

                    var q = 1;

                    foreach (var knownType in knownTypes)
                    {
                        if (string.IsNullOrEmpty(knownType.Namespace) || namespacesAdded.Contains(knownType.Namespace))
                        {
                            continue;
                        }

                        namespaces.Add(string.Format("q{0}", q++), knownType.Namespace);
                        namespacesAdded.Add(knownType.Namespace);
                    }

                    serializerNamespaces.Add(key, namespaces);
                }

                return serializerNamespaces[key];
            }
        }
    }
}