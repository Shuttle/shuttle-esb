using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
    public class MessageManagementPresenter : ManagementModulePresenter, IMessageManagementPresenter
    {
        private readonly IMessageConfiguration messageConfiguration;
        private readonly IQueueManager queueManager;
        private readonly ISerializer serializer;
        private readonly IMessageManagementView view;

        public MessageManagementPresenter()
        {
            view = new MessageManagementView(this);

            queueManager = new QueueManager();

            messageConfiguration = new MessageConfiguration();

            serializer = messageConfiguration.GetSerializer();
        }

        public override string Text
        {
            get { return MessageResources.Text_Messages; }
        }

        public override Image Image
        {
            get { return MessageResources.Image_Messages; }
        }

        public override UserControl ViewUserControl
        {
            get { return (UserControl) view; }
        }

        public void RefreshQueue()
        {
            var sourceQueueUriValue = view.SourceQueueUriValue;
            var fetchCountValue = view.FetchCountValue;

            QueueTask("RefreshQueue",
                      () =>
                          {
                              view.ClearMessages();

                              var reader = queueManager.GetQueue(sourceQueueUriValue) as IQueueReader;

                              if (reader == null)
                              {
                                  Log.Error(MessageResources.SourceQueueUriReader);

                                  return;
                              }

                              view.PopulateMessages(
                                  reader.Read(fetchCountValue)
                                      .Select(stream => serializer.Deserialize(typeof (TransportMessage), stream)).Cast
                                      <TransportMessage>());
                          }
                );
        }

        public void Remove()
        {
            if (!view.HasSelectedMessages)
            {
                Log.Error(MessageResources.NoMessagesSelected);

                return;
            }

            if (MessageBox.Show(MessageResources.ConfirmMessageDeletion,
                                MessageResources.Confirmation,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            var sourceQueueUriValue = view.SourceQueueUriValue;

            QueueTask("Remove",
                      () =>
                          {
                              var queue = queueManager.GetQueue(sourceQueueUriValue);

                              foreach (var message in view.SelectedMessages)
                              {
                                  queue.Remove(message.MessageId);
                              }
                          });

            RefreshQueue();
        }

        public void Move()
        {
            if (!view.HasSelectedMessages)
            {
                Log.Error(MessageResources.NoMessagesSelected);

                return;
            }

            var sourceQueueUriValue = view.SourceQueueUriValue;
            var destinationQueueUriValue = view.DestinationQueueUriValue;

            QueueTask("Move",
                      () =>
                          {
                              var source = queueManager.GetQueue(sourceQueueUriValue);
                              var destination = queueManager.GetQueue(destinationQueueUriValue);

                              foreach (var message in view.SelectedMessages)
                              {
                                  using (var scope = new TransactionScope())
                                  {
                                      if (source.Remove(message.MessageId))
                                      {
                                          Log.Information(string.Format(MessageResources.RemovedMessage,
                                                                        message.MessageId, sourceQueueUriValue));

                                          destination.Enqueue(message.MessageId, serializer.Serialize(message));

                                          Log.Information(string.Format(MessageResources.EnqueuedMessage,
                                                                        message.MessageId, destinationQueueUriValue));
                                      }
                                      else
                                      {
                                          Log.Warning(string.Format(MessageResources.CouldNotRemoveMessage,
                                                                    message.MessageId, sourceQueueUriValue));
                                      }

                                      scope.Complete();
                                  }
                              }
                          })
                ;

            RefreshQueue();
        }

        public void MessageSelected()
        {
            view.ClearMessageView();

            if (!view.HasSelectedMessages)
            {
                return;
            }

            var transportMessage = view.SelectedTransportMessage();

            QueueTask("MessageSelected",
                      () =>
                          {
                              object message = null;

                              try
                              {
                                  var type = Type.GetType(transportMessage.AssemblyQualifiedName, true, true);

                                  var canDisplayMessage = true;

                                  if (transportMessage.CompressionEnabled())
                                  {
                                      Log.Warning(string.Format(MessageResources.MessageCompressed,
                                                                transportMessage.MessageId));
                                      canDisplayMessage = false;
                                  }

                                  if (transportMessage.EncryptionEnabled())
                                  {
                                      Log.Warning(string.Format(MessageResources.MessageEncrypted,
                                                                transportMessage.MessageId));
                                      canDisplayMessage = false;
                                  }

                                  if (canDisplayMessage)
                                  {
                                      using (var stream = new MemoryStream(transportMessage.Message))
                                      {
                                          message = serializer.Deserialize(type, stream);
                                      }

                                      if (!type.AssemblyQualifiedName.Equals(transportMessage.AssemblyQualifiedName))
                                      {
                                          Log.Warning(string.Format(MessageResources.MessageTypeMismatch,
                                                                    transportMessage.AssemblyQualifiedName,
                                                                    type.AssemblyQualifiedName));
                                      }
                                  }
                              }
                              catch (Exception ex)
                              {
                                  Log.Warning(string.Format(MessageResources.CannotObtainMessageType,
                                                            transportMessage.AssemblyQualifiedName));
                                  Log.Error(ex.Message);
                              }

                              view.ShowMessage(transportMessage, message);
                          }
                );
        }

        public void ReturnToSourceQueue()
        {
            var sourceQueueUriValue = view.SourceQueueUriValue;

            QueueTask("ReturnToSourceQueue",
                      () =>
                          {
                              var source = queueManager.GetQueue(sourceQueueUriValue);

                              foreach (var message in view.SelectedMessages)
                              {
                                  message.StopIgnoring();
                                  message.FailureMessages.Clear();

                                  var destination = queueManager.GetQueue(message.RecipientInboxWorkQueueUri);

                                  using (var scope = new TransactionScope())
                                  {
                                      if (source.Remove(message.MessageId))
                                      {
                                          Log.Information(string.Format(MessageResources.RemovedMessage,
                                                                        message.MessageId, sourceQueueUriValue));

                                          destination.Enqueue(message.MessageId, serializer.Serialize(message));

                                          Log.Information(string.Format(MessageResources.EnqueuedMessage,
                                                                        message.MessageId,
                                                                        message.RecipientInboxWorkQueueUri));
                                      }
                                      else
                                      {
                                          Log.Warning(string.Format(MessageResources.CouldNotRemoveMessage,
                                                                    message.MessageId, sourceQueueUriValue));
                                      }

                                      scope.Complete();
                                  }
                              }
                          });

            RefreshQueue();
        }

        public void CheckAll()
        {
            QueueTask("CheckAll", () => view.CheckAll());
        }

        public void InvertChecks()
        {
            QueueTask("InvertChecks", () => view.InvertChecks());
        }

        public override void OnViewReady()
        {
            view.AddFetchCount(0);
            view.AddFetchCount(50);
            view.AddFetchCount(200);
            view.AddFetchCount(500);
            view.AddFetchCount(1000);

            view.FetchCountValue = 200;

            RefreshQueues();
        }

        public void RefreshQueues()
        {
            QueueTask("RefreshQueues", () => view.PopulateQueues(ManagementConfiguration.QueueRepository().All()));
        }

        public void StopIgnoring()
        {
            var sourceQueueUriValue = view.SourceQueueUriValue;

            QueueTask("StopIgnoring",
                      () =>
                          {
                              var source = queueManager.GetQueue(sourceQueueUriValue);

                              foreach (var message in view.SelectedMessages)
                              {
                                  message.StopIgnoring();

                                  using (var scope = new TransactionScope())
                                  {
                                      if (source.Remove(message.MessageId))
                                      {
                                          source.Enqueue(message.MessageId, serializer.Serialize(message));
                                      }

                                      scope.Complete();
                                  }

                                  Log.Information(string.Format(MessageResources.StoppedIgnoringMessage,
                                                                message.MessageId));
                              }
                          });

            RefreshQueue();
        }
    }
}