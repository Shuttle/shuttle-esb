insert into [dbo].[IdempotenceDeferredMessage]
	(MessageId, MessageBody) 
values 
	(@MessageId, @MessageBody)
