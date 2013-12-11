insert into [dbo].[DeferredMessage]
	(DeferTillDate, MessageBody) 
values 
	(@DeferTillDate, @MessageBody)
