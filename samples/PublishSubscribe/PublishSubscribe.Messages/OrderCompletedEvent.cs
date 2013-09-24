using System;

namespace PublishSubscribe.Messages
{
    public class OrderCompletedEvent 
    {
        public Guid OrderId { get; set; }
    }
}