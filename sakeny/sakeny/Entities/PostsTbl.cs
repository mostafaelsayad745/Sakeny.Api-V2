using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    [Table("POSTS_TBL")]
    public partial class PostsTbl
    {
        public PostsTbl()
        {
            PostFeedbackTbls = new HashSet<PostFeedbackTbl>();
            PostPicTbls = new HashSet<PostPicTbl>();
        }

        [Key]
        [Column("POST_ID", TypeName = "numeric(18, 0)")]
        public decimal PostId { get; set; }
        [Column("POST_DATE", TypeName = "date")]
        public DateTime? PostDate { get; set; }
        [Column("POST_TIME")]
        public TimeSpan? PostTime { get; set; }
        [Column("POST_CATEGORY")]
        public string? PostCategory { get; set; }
        [Column("POST_TITLE")]
        public string? PostTitle { get; set; }
        [Column("POST_BODY")]
        public string? PostBody { get; set; }
        [Column("POST_AREA", TypeName = "decimal(18, 2)")]
        public decimal? PostArea { get; set; }
        [Column("POST_KITCHENS")]
        public int? PostKitchens { get; set; }
        [Column("POST_BEDROOMS")]
        public int? PostBedrooms { get; set; }
        [Column("POST_BATHROOMS")]
        public int? PostBathrooms { get; set; }
        [Column("POST_LOOK_SEA")]
        public bool? PostLookSea { get; set; }
        [Column("POST_PETS_ALLOW")]
        public bool? PostPetsAllow { get; set; }
        [Column("POST_CURRENCY")]
        public string? PostCurrency { get; set; }
        [Column("POST_PRICE_AI", TypeName = "decimal(18, 2)")]
        public decimal? PostPriceAi { get; set; }
        [Column("POST_PRICE_DISPLAY", TypeName = "decimal(18, 2)")]
        public decimal? PostPriceDisplay { get; set; }
        [Column("POST_PRICE_TYPE")]
        public string? PostPriceType { get; set; }
        [Column("POST_ADDRESS")]
        public string? PostAddress { get; set; }
        [Column("POST_CITY")]
        public string? PostCity { get; set; }
        [Column("POST_STATE")]
        public string? PostState { get; set; }
        [Column("POST_FLOOR")]
        public int? PostFloor { get; set; }
        [Column("POST_LATITUDE")]
        public string? PostLatitude { get; set; }
        [Column("POST_LONGITUDE")]
        public string? PostLongitude { get; set; }
        [Column("POST_STATUE")]
        public bool? PostStatue { get; set; } 

        [InverseProperty("Post")]
        public virtual ICollection<PostFeedbackTbl>? PostFeedbackTbls { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostPicTbl> PostPicTbls { get; set; }

        [InverseProperty("Post")]
        public virtual ICollection<PostFeaturesTbl>? PostFeaturesTbls { get; set; }

        [InverseProperty("Post")]
        public virtual ICollection<FeaturesTbl> Features { get; set; }
    

    //new modification
    //[Column("POST_USER_ID", TypeName = "numeric(18, 0)")]
    public decimal PostUserId { get; set; }

        [ForeignKey("PostUserId")]
        public virtual UsersTbl? User { get; set; }

    }
}
