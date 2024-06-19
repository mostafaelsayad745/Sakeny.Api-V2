using sakeny.Entities;

namespace sakeny.Services.NewPostRepo
{
    public class PostDto
    {
        public decimal? PostId { get; set; }
        public DateTime? PostDate { get; set; }
        public TimeSpan? PostTime { get; set; }
        public string? PostCategory { get; set; }
        public string? PostTitle { get; set; }
        public string? PostBody { get; set; }
        public decimal? PostArea { get; set; }
        public int? PostKitchens { get; set; }
        public int? PostBedrooms { get; set; }
        public int? PostBathrooms { get; set; }
        public bool? PostLookSea { get; set; }
        public bool? PostPetsAllow { get; set; }
        public string? PostCurrency { get; set; }
        public decimal? PostPriceAi { get; set; }
        public decimal? PostPriceDisplay { get; set; }
        public string? PostPriceType { get; set; }
        public string? PostAddress { get; set; }
        public string? PostCity { get; set; }
        public string? PostState { get; set; }
        public int? PostFloor { get; set; }
        public string? PostLatitude { get; set; }
        public string? PostLongitude { get; set; }
        public bool? PostStatue { get; set; }
        public decimal PostUserId { get; set; }
        public bool IsFavourited { get; set; } = false;
        public string? PostOwnerName { get; set; }
        //public List<PostFeedbackTblDto> PostFeedbackTbls { get; set; }
        public List<PostPicTblDto>? PostPicTbls { get; set; }
        public List<FeaturesTblDto>? Features { get; set; }
    }
}
