using NuGet.Protocol.Core.Types;
using sakeny.Entities;

namespace sakeny.Models.UserFeedbackDtos
{
    public class UserFeedbacktoReturnDto
    {
        public UserFeedbackTbl UserFeedback { get; set; }
        public UserForReturnDto UserWhoMadeTheFeedback { get; set; }
    }
}
