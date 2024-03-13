using sakeny.Entities;

namespace sakeny.Models
{
    public class UserForReturnWithPostFeedbacks
    {
        public string UserName { get; set; } = string.Empty;

        public string UserPassword { get; set; } = string.Empty;

        public string UserFullName { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public string UserNatId { get; set; } = string.Empty;

        public string UserGender { get; set; } = string.Empty;

        public int UserAge { get; set; }

        public string UserInfo { get; set; } = string.Empty;

        public string UserAddress { get; set; } = string.Empty;

        public string UserAccountType { get; set; } = string.Empty;

        public ICollection<PostFeedbackTbl> PostFeedbacks { get; set; } = new List<PostFeedbackTbl>();
    }
}
