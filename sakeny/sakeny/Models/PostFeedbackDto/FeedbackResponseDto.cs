namespace sakeny.Models.PostFeedbackDto
{
    public class FeedbackResponseDto
    {
        public DateTime? TimeCreated { get; set; }
        public string? FeedbackText { get; set; }
        public DateTime? DateCreated { get; set; }
        public UserForReturnDto User { get; set; }
    }

}
