namespace sakeny.Models.ChatDtos
{
    public class MessageForReturnDto
    {
        public string Message { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public DateTime MessageSent { get; set; }
    }
}
