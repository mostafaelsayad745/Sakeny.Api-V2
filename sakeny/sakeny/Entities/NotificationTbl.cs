using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sakeny.Entities
{
    public class NotificationTbl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("NOTIFICATION_ID", TypeName = "numeric(18, 0)")]
        public decimal NotificationId { get; set; }

        [Column("USER_ID", TypeName = "numeric(18, 0)")]
        public decimal UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual UsersTbl User { get; set; }

        [Column("MESSAGE")]
        public string Message { get; set; }

        [Column("TIMESTAMP")]
        public DateTime Timestamp { get; set; }
    }
}
