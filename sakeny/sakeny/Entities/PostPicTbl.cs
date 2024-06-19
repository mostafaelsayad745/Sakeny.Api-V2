using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    [Table("POST_PIC_TBL")]
    public partial class PostPicTbl
    {
        [Key]
        [Column("POST_PIC_ID", TypeName = "numeric(18, 0)")]
        public decimal PostPicId { get; set; }

        [Column("POST_ID", TypeName = "numeric(18, 0)")]
        public decimal? PostId { get; set; }

        [Column("PICTURE", TypeName = "image")]
        public byte[]? Picture { get; set; }

        [Column("PICTURESTRING", TypeName = "nvarchar(max)")] // Change the type to nvarchar(max)
        public string? PictureString { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("PostPicTbls")]
        public virtual PostsTbl? Post { get; set; }
    }
}
