declare @id int

select top 1 @id = SequenceId from [dbo].[{0}]
select MessageId, MessageBody from [dbo].[{0}] where SequenceId = @id
delete from [dbo].[{0}] where SequenceId = @id
