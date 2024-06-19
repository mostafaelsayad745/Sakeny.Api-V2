using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    //[Keyless]
    [Table("POST_FEATURES_TBL")]
    [Index("FeaturesId", "PostId", Name = "POST_FEATURES_INDEX", IsUnique = true)]
    public partial class PostFeaturesTbl
    {
        [Key]
        [Column("FEATURES_ID", TypeName = "numeric(18, 0)")]
        public decimal? FeaturesId { get; set; }

        [Key]
        [Column("POST_ID", TypeName = "numeric(18, 0)")]
        public decimal? PostId { get; set; }

        [ForeignKey("FeaturesId")]
        public virtual FeaturesTbl? Features { get; set; }
        [ForeignKey("PostId")]
        public virtual PostsTbl? Post { get; set; }
    }
}
