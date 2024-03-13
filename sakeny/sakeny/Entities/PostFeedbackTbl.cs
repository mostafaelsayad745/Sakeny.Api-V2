using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    [Table("POST_FEEDBACK_TBL")]
    [Index("UserId", "PostId", Name = "POST_FEED_INDEX", IsUnique = true)]
    public partial class PostFeedbackTbl
    {
        [Key]
        [Column("POST_FEED_ID", TypeName = "numeric(18, 0)")]
        public decimal PostFeedId { get; set; }

        [Column("POST_FEED_DATE", TypeName = "date")]
        public DateTime? PostFeedDate { get; set; }

        [Column("POST_FEED_TIME")]
        public TimeSpan? PostFeedTime { get; set; }

        [Column("POST_FEED_TEXT")]
        public string? PostFeedText { get; set; }

        [Column("POST_ID", TypeName = "numeric(18, 0)")]
        public decimal? PostId { get; set; }

        [Column("USER_ID", TypeName = "numeric(18, 0)")]
        public decimal? UserId { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("PostFeedbackTbls")]
        public virtual PostsTbl? Post { get; set; }


        // who is made the feedback or the 
        
        [ForeignKey("UserId")]
        [InverseProperty("PostFeedbackTbls")]
        public virtual UsersTbl? User { get; set; } 
    }
}
