select top 1 SequenceId, MessageId, MessageBody from [dbo].[{0}] where MessageId not in ({1}) order by SequenceId
