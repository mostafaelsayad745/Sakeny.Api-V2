using sakeny.Entities;

namespace sakeny.Services.PostRepo
{
    public class PostModel
    {
        public PostsTbl Post { get; set; }
        public List<PostPicTbl> Pictures { get; set; }
        public List<FeaturesTbl> Features { get; set; } 
        public List<PostFeedbackTbl> Feedbacks { get; set; }
    }


}
