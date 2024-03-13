using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    [Table("USER_BAN_TBL")]
    public partial class UserBanTbl
    {
        [Key]
        [Column("USER_BAN_ID", TypeName = "numeric(18, 0)")]
        public decimal UserBanId { get; set; }

        [Column("USER_BAN_NAT_ID")]
        [StringLength(50)]
        public string? UserBanNatId { get; set; } // return type must be int
    }
}
