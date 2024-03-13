using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    [Table("USERS_TBL")]
    public partial class UsersTbl
    {
        public UsersTbl()
        {
            PostFeedbackTbls = new HashSet<PostFeedbackTbl>();
            //SentMessages = new HashSet<UserChatTbl>();
            //ReceivedMessages = new HashSet<UserChatTbl>();
        }

        public UsersTbl(string userName, string userPassword, string userFullName,
            string userEmail, string userNatId, string userGender,
            int userAge, string userInfo, string userAddress, string userAccountType)
        {
            UserName = userName;
            UserPassword = userPassword;
            UserFullName = userFullName;
            UserEmail = userEmail;
            UserNatId = userNatId;
            UserGender = userGender;
            UserAge = userAge;
            UserInfo = userInfo;
            UserAddress = userAddress;
            UserAccountType = userAccountType;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("USER_ID", TypeName = "numeric(18, 0)")]
        public decimal UserId { get; set; }
        [Column("USER_NAME")]
        [StringLength(50)]
        public string UserName { get; set; }
        [Column("USER_PASSWORD")]
        public string UserPassword { get; set; }
        [Column("USER_FULL_NAME")]
        [StringLength(300)]
        public string UserFullName { get; set; }
        [Column("USER_EMAIL")]
        [EmailAddress]
        [StringLength(300)]
        public string UserEmail { get; set; }
        [Column("USER_NAT_ID")]
        [StringLength(50)]
        public string UserNatId { get; set; }
        [Column("USER_GENDER")]
        public string UserGender { get; set; }
        [Column("USER_AGE")]
        public int UserAge { get; set; }
        [Column("USER_INFO")]
        public string UserInfo { get; set; }
        [Column("USER_ADDRESS")]
        public string UserAddress { get; set; }
        [Column("USER_ACCOUNT_TYPE")]
        public string UserAccountType { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<PostFeedbackTbl> PostFeedbackTbls { get; set; }

        // adding propertry of posts that the user liked


        //---------------------------------------------

        //[InverseProperty("Sender")]
        //public virtual ICollection<UserChatTbl> SentMessages { get; set; }

        //[InverseProperty("Receiver")]
        //public virtual ICollection<UserChatTbl> ReceivedMessages { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<NotificationTbl> Notifications { get; set; }

        
        [InverseProperty("User")]
        public virtual ICollection<PostsTbl> Posts { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<PostFaviourateTbl> FavoritePosts { get; set; }
    }
}
