if not exists(select null from [dbo].[{0}] where MessageId = @MessageId)
	insert into [dbo].[{0}] (MessageId, MessageBody) values (@MessageId, @MessageBody)