namespace sakeny.Models
{
    public class PostFeedbackForUpdateDto
    {
        public DateTime? PostFeedDate { get; set; }

        public TimeSpan? PostFeedTime { get; set; }

        public string? PostFeedText { get; set; }

        //public decimal? PostId { get; set; }

        public decimal? UserId { get; set; }
    }
}