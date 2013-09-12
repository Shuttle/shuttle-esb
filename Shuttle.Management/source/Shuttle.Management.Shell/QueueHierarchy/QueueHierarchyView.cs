using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
    public enum ShowQueueButtonPosition
    {
        NotShown = 0,
        Left = 1,
        Right = 2
    }

    public partial class QueueHierarchyView : UserControl, IQueueHierarchyView
    {
        private readonly Form queueForm = new Form();
        private readonly TreeView queueTree = new TreeView();

        public QueueHierarchyView()
        {
            InitializeComponent();

            queueTree.Dock = DockStyle.Fill;
            queueTree.Location = new Point(0, 20);
            queueTree.Name = "QueueTree";
            queueTree.Size = new Size(390, 250);
            queueTree.TabIndex = 0;
            queueTree.LostFocus += queueTree_LostFocus;
            queueTree.NodeMouseClick += queueTree_NodeMouseClick;
            queueTree.KeyDown += queueTree_KeyDown;

            queueForm.Controls.Add(queueTree);
            queueForm.FormBorderStyle = FormBorderStyle.None;
            queueForm.StartPosition = FormStartPosition.Manual;
            queueForm.ShowInTaskbar = false;
            queueForm.BackColor = SystemColors.Control;

            ShowQueuesButton.Click += ShowQueuesButtonClick;
        }

        private void ShowQueuesButtonClick(object sender, EventArgs e)
        {
            ShowQueueForm();
        }

        private ShowQueueButtonPosition showQueueButtonPosition;

        public ShowQueueButtonPosition ShowQueueButtonPosition
        {
            get { return showQueueButtonPosition; }
            set
            {
                showQueueButtonPosition = value;

                switch (showQueueButtonPosition)
                {
                    case ShowQueueButtonPosition.Left:
                        {
                            ShowQueuesButton.Visible = true;
                            ShowQueuesButton.Dock = DockStyle.Left;

                            break;
                        }
                    case ShowQueueButtonPosition.Right:
                        {
                            ShowQueuesButton.Visible = true;
                            ShowQueuesButton.Dock = DockStyle.Right;

                            break;
                        }
                    default:
                        {
                            ShowQueuesButton.Visible = false;
                            break;
                        }
                }
            }
        }

        public event EventHandler<QueueSelectedEventArgs> QueueSelected = delegate { };

        public void AddQueue(Uri uri)
        {
            Guard.AgainstNull(uri, "uri");

            var scheme = queueTree.Nodes.ContainsKey(uri.Scheme)
                             ? queueTree.Nodes[uri.Scheme]
                             : queueTree.Nodes.Add(uri.Scheme, uri.Scheme);

            var host = scheme.Nodes.ContainsKey(uri.Host)
                           ? scheme.Nodes[uri.Host]
                           : scheme.Nodes.Add(uri.Host, uri.Host);

            var localPath = uri.LocalPath.Replace("/", string.Empty);

            var key = Key(uri);

            if (!host.Nodes.ContainsKey(key))
            {
                host.Nodes.Add(key, localPath).Tag = uri;
            }

            queueTree.Sort();
            queueTree.ExpandAll();
        }

        private static string Key(Uri uri)
        {
            return uri.ToString().ToLower();
        }

        public void AddQueue(string uri)
        {
            AddQueue(new Uri(uri));
        }

        public void Clear()
        {
            queueTree.Nodes.Clear();
        }

        public bool ContainsQueue(Uri uri)
        {
            Guard.AgainstNull(uri, "uri");

            return queueTree.Nodes.Find(uri.ToString(), true).Length > 0;
        }

        public bool ContainsQueue(string uri)
        {
            return ContainsQueue(new Uri(uri));
        }

        public bool RemoveQueue(Uri uri)
        {
            Guard.AgainstNull(uri, "uri");

            var node = FindQueueNode(uri);

            if (node != null)
            {
                queueTree.Nodes.Remove(node);

                NormalizeTree();
            }

            return node != null;
        }

        private void NormalizeTree()
        {
            var node = FindChildlessNode(queueTree.Nodes);

            while (node != null)
            {
                queueTree.Nodes.Remove(node);

                node = FindChildlessNode(queueTree.Nodes);
            }
        }

        private static TreeNode FindChildlessNode(TreeNodeCollection nodes)
        {
            return nodes.Cast<TreeNode>().FirstOrDefault(node => node.Nodes.Count == 0);
        }

        public override string Text
        {
            get { return SelectedQueueUri.Text; }
            set { SelectedQueueUri.Text = value; }
        }

        public bool RemoveQueue(string uri)
        {
            return RemoveQueue(new Uri(uri));
        }

        private void queueTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeSelected(e.Node);
        }

        private void NodeSelected(TreeNode node)
        {
            if (node == null || node.Tag == null)
            {
                return;
            }

            SelectedQueueUri.Text = node.Tag.ToString();

            HideQueueForm();

            QueueSelected.Invoke(this, new QueueSelectedEventArgs((Uri)node.Tag));
        }

        private void queueTree_KeyDown(object sender, KeyEventArgs e)
        {
            e.OnF4(HideQueueForm);
            e.OnEscape(HideQueueForm);
            e.OnEnterPressed(() => NodeSelected(queueTree.SelectedNode));
        }

        private void queueTree_LostFocus(object sender, EventArgs e)
        {
            HideQueueForm();
        }

        private void SelectedQueueUri_Click(object sender, EventArgs e)
        {
            HideQueueForm();
        }

        private void ShowQueueForm()
        {
            if (queueForm.Visible)
            {
                return;
            }

            var rectangle = RectangleToScreen(ClientRectangle);

            queueForm.Location = rectangle.Y + SelectedQueueUri.Height + queueForm.Height >
                                 Screen.FromHandle(queueForm.Handle).Bounds.Height
                                     ? new Point(rectangle.X, rectangle.Y - queueForm.Height)
                                     : new Point(rectangle.X, rectangle.Y + SelectedQueueUri.Height);

            queueForm.Width = rectangle.Width;
            queueForm.Show();
            queueForm.BringToFront();
        }

        private void HideQueueForm()
        {
            queueForm.Visible = false;
        }

        private void ToggleQueueForm()
        {
            if (queueForm.Visible)
            {
                HideQueueForm();
            }
            else
            {
                ShowQueueForm();
            }
        }

        private void SelectedQueueUri_KeyUp(object sender, KeyEventArgs e)
        {
            e.OnEnterPressed(() =>
                                 {
                                     Uri uri;

                                     if (Uri.TryCreate(SelectedQueueUri.Text, UriKind.Absolute, out uri))
                                     {
                                         QueueSelected.Invoke(this, new QueueSelectedEventArgs(uri));
                                     }
                                 });
            e.OnKeyDown(ShowQueueForm);
            e.OnF4(ToggleQueueForm);
        }

        private TreeNode FindQueueNode(Uri uri)
        {
            var nodes = queueTree.Nodes.Find(Key(uri), true);

            return nodes.Length > 0
                       ? nodes[0]
                       : null;
        }
    }
}