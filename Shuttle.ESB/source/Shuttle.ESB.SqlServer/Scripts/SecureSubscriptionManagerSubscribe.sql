if 
	not exists (select null from [dbo].[SubscriberMessageTypeRequest] where InboxWorkQueueUri = @InboxWorkQueueUri and MessageType = @MessageType)
	and
	not exists (select null from [dbo].[SubscriberMessageType] where InboxWorkQueueUri = @InboxWorkQueueUri and MessageType = @MessageType)
		insert into [dbo].[SubscriberMessageTypeRequest]
			(InboxWorkQueueUri, MessageType) 
		values 
			(@InboxWorkQueueUri, @MessageType)
