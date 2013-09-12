using System;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.Management.Messages
{
    public partial class MessageView : UserControl
    {
        public MessageView()
        {
            InitializeComponent();
        }

        public void Show(TransportMessage transportMessage, object message)
        {
            Clear();

            if (transportMessage == null)
            {
                return;
            }

            Populate(message, transportMessage, typeof (TransportMessage), 0);

            ValueColumn.Width = -1;
        }

        public void Clear()
        {
            PropertyValueList.Items.Clear();
        }

        private void Populate(object message, object o, Type type, int indent)
        {
            if (o == null)
            {
                return;
            }

            if (type.IsAssignableTo(typeof (IEnumerable)))
            {
                var index = 0;

                foreach (var item in (IEnumerable) o)
                {
                    var enumerated =
                        PropertyValueList.Items.Add(string.Format("{0}{1}[{2}]", GetTypeIndent(indent),
                                                                  item.GetType().FullName, index));

                    if (item.GetType() == typeof (string))
                    {
                        enumerated.SubItems.Add(Convert.ToString(item).Replace("\r", "").Replace("\n", ""));
                    }
                    else
                    {
                        Populate(message, item, item.GetType(), indent + 1);
                    }

                    index++;
                }

                if (index == 0)
                {
                    var item = PropertyValueList.Items.Add(string.Format("{0}{1}", GetValueIndent(indent), "{no items}"));

                    item.Font = new Font(item.Font, FontStyle.Italic);
                }
            }
            else
            {
                var done = new ArrayList();

                foreach (var mi in type.GetMembers())
                {
                    if (type == typeof (TransportMessage) &&
                        mi.Name.Equals("Message", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (message == null)
                        {
                            PropertyValueList.Items.Add(string.Format("{0}{1}", GetTypeIndent(indent), mi.Name)).
                                SubItems.Add("null");
                        }
                        else
                        {
                            PropertyValueList.Items.Add(string.Format("{0}{1}", GetTypeIndent(indent), mi.Name)).
                                SubItems.Add(message.GetType().FullName);

                            Populate(message, message, message.GetType(), indent + 1);
                        }

                        continue;
                    }

                    if (mi.MemberType == MemberTypes.Constructor
                        ||
                        ReferenceEquals(mi.DeclaringType, typeof (object))
                        ||
                        done.Contains(mi.Name)
                        ||
                        mi.MemberType != MemberTypes.Property)
                    {
                        continue;
                    }

                    done.Add(mi.Name);

                    var pi = (PropertyInfo) mi;
                    var value = pi.GetValue(o, null);
                    var valueType = value != null
                                        ? value.GetType()
                                        : pi.PropertyType;

                    if (pi.PropertyType.IsValueType
                        ||
                        pi.PropertyType == typeof (string)
                        ||
                        valueType == typeof (string))
                    {
                        PropertyValueList
                            .Items.Add(string.Format("{0}{1}", GetValueIndent(indent), pi.Name))
                            .SubItems.Add(Convert.ToString(pi.GetValue(o, null)));
                    }
                    else
                    {
                        var item = PropertyValueList.Items.Add(string.Format("{0}{1}", GetTypeIndent(indent), pi.Name));

                        item.Font = new Font(item.Font, FontStyle.Bold);

                        item.SubItems.Add(string.Format("{{{0}}}", valueType.FullName));

                        Populate(message, value, valueType, indent + 1);
                    }
                }
            }
        }

        private static string GetValueIndent(int indent)
        {
            return string.Concat(new string(' ', indent*3), " ");
        }

        private static string GetTypeIndent(int indent)
        {
            return string.Concat(new string(' ', indent*3), "+");
        }

        private void PropertyValueList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropertyValueList.SelectedItems.Count == 0)
            {
                return;
            }

            var item = PropertyValueList.SelectedItems[0];

            Log.Information(string.Format("{0} : {1}", item.Text,
                                          item.SubItems.Count > 1 ? item.SubItems[1].Text : string.Empty));
        }
    }
}