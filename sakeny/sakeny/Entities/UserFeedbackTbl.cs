using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    [Table("USER_FEEDBACK_TBL")]
    public partial class UserFeedbackTbl
    {
        [Key]
        [Column("FEEDBACK_ID", TypeName = "numeric(18, 0)")]
        public decimal FeedbackId { get; set; }

        [Column("FEEDBACK_TEXT")]
        public string? FeedbackText { get; set; }

        [Column("FEEDBACK_DATE", TypeName = "date")]
        public DateTime? FeedbackDate { get; set; }

        [Column("FEEDBACK_TIME")]
        public TimeSpan? FeedbackTime { get; set; }

        // modification : here is the return vlaue must be the user table (UsersTbl)
        // modification : in this class must be a forign key to the (UsersTbl)

        [Column("FEEDBACK_FROM")]
        public string? FeedbackFrom { get; set; }

        [Column("FEEDBACK_TO")]
        public string? FeedbackTo { get; set; }
    }
}
