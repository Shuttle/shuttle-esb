declare @id int

select top 1 @id = SequenceId from [dbo].[DeferredMessage]
update [dbo].[DeferredMessage] set [DeferTillDate] = [DeferTillDate] where SequenceId = @id
select MessageBody from [dbo].[DeferredMessage] where SequenceId = @id
delete from [dbo].[DeferredMessage] where SequenceId = @id
