declare @id int

select top 1 @id = SequenceId from [dbo].[DeferredMessage]
update [dbo].[DeferredMessage] set SequenceId = SequenceId where SequenceId = @id
select TransportMessage from [dbo].[DeferredMessage] where SequenceId = @id
delete from [dbo].[DeferredMessage] where SequenceId = @id
