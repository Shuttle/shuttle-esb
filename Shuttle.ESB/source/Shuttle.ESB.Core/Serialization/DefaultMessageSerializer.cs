using System;
using System.Collections;
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
	public class DefaultMessageSerializer :
		IKnownTypes,
		IMessageSerializer,
		IReplay<IKnownTypes>
	{
		private const string SOAP_INCLUDE = "SoapInclude";
		private const string TYPE_ERROR = "--- could not retrieve type name from exception ---";
		private const string XML_INCLUDE = "XmlInclude";

		private static readonly Regex expression = new Regex(@"The\stype\s(?<Type>[\w\\.]*)");

		private static readonly object padlock = new object();

		private readonly List<Type> ensuredTypes = new List<Type>();
		private readonly List<Type> knownTypes = new List<Type>();
		private readonly XmlWriterSettings xmlSettings;
		private Dictionary<Type, XmlSerializer> serializers = new Dictionary<Type, XmlSerializer>();
		private Dictionary<Type, XmlSerializerNamespaces> serializerNamespaces = new Dictionary<Type, XmlSerializerNamespaces>();

		public DefaultMessageSerializer()
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

		public void AddKnownType(Type type)
		{
			Guard.AgainstNull(type, "type");

			if (type.IsInterface)
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

				ResetSerializers();
			}

			EnsureTypes(type);
		}

		public bool HasKnownType(Type type)
		{
			return knownTypes.Contains(type);
		}

		public void AddKnownTypes(IEnumerable<Type> types)
		{
			Guard.AgainstNull(types, "type");

			lock (padlock)
			{
				foreach (var type in types)
				{
					if (type.IsInterface)
					{
						continue;
					}

					if (HasKnownType(type))
					{
						return;
					}

					knownTypes.Add(type);
				}

				ResetSerializers();
			}

			foreach (var type in types)
			{
				EnsureTypes(type);
			}
		}

		public Stream Serialize(object message)
		{
			Guard.AgainstNull(message, "message");

			var messageType = message.GetType();

			EnsureTypes(messageType);

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

			using (
				var reader = XmlDictionaryReader.CreateTextReader(stream.Copy(), Encoding.UTF8,
																  new XmlDictionaryReaderQuotas
																	  {
																		  MaxArrayLength = Int32.MaxValue,
																		  MaxStringContentLength = int.MaxValue,
																		  MaxNameTableCharCount = int.MaxValue
																	  }, null))
			{
				try
				{
					return GetSerializer(type).Deserialize(reader);
				}
				catch (Exception ex)
				{
					throw new MessageDeserializationException(ex);
				}
			}
		}

		public void Replay(IKnownTypes destination)
		{
			foreach (var type in knownTypes)
			{
				destination.AddKnownType(type);
			}
		}

		private void EnsureTypes(Type type)
		{
			if (ensuredTypes.Contains(type))
			{
				return;
			}

			if (!HasKnownType(type))
			{
				AddKnownType(type);
			}

			foreach (var nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (!HasKnownType(nested))
				{
					AddKnownType(nested);
				}
			}

			ensuredTypes.Add(type);
		}

		private void ResetSerializers()
		{
			serializers = new Dictionary<Type, XmlSerializer>();
			serializerNamespaces = new Dictionary<Type, XmlSerializerNamespaces>();
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
				if (!serializers.ContainsKey(type))
				{
					serializers.Add(type, new XmlSerializer(type, knownTypes.ToArray()));
				}

				return serializers[type];
			}
		}

		private XmlSerializerNamespaces GetSerializerNamespaces(Type type)
		{
			lock (padlock)
			{
				if (!serializerNamespaces.ContainsKey(type))
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

					serializerNamespaces.Add(type, namespaces);
				}

				return serializerNamespaces[type];
			}
		}

		public IEnumerator<Type> GetEnumerator()
		{
			return knownTypes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}