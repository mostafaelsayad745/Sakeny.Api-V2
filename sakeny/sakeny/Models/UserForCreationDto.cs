using System.ComponentModel.DataAnnotations;

namespace sakeny.Models
{
    public class UserForCreationDto
    {
        [Required (ErrorMessage = "User Name is required")]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required (ErrorMessage = "User Password is required")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; } = string.Empty;

        [Required (ErrorMessage = "User Full Name is required")]
        [MaxLength(200)]
        public string UserFullName { get; set; } = string.Empty;

        [Required (ErrorMessage = "User Email is required")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; } = string.Empty;

        [Required (ErrorMessage = "User National ID is required")]
        [MaxLength(50)]
        public string UserNatId { get; set; } = string.Empty;

        [Required (ErrorMessage ="User Gender is required")]
        public string UserGender { get; set; } = string.Empty;

        [Required (ErrorMessage = "User Age is required")]
        public int UserAge { get; set; }

        [Required (ErrorMessage = "User Info is required")]
        [DataType(DataType.MultilineText)]
        public string UserInfo { get; set; } = string.Empty;

        [Required (ErrorMessage = "User Address is required")]
        [DataType(DataType.MultilineText)]
        public string UserAddress { get; set; } = string.Empty;

        [Required (ErrorMessage = "User Account Type is required")]
        public string UserAccountType { get; set; } = string.Empty;

        //public string? UserName { get; set; }
        //public string? UserEmail { get; set; }
        //public string? UserPassword { get; set; }
        //public string? UserPhone { get; set; }
        //public string? UserGender { get; set; }
        //public string? UserAddress { get; set; }
        //public string? UserCity { get; set; }
        //public string? UserState { get; set; }
        //public string? UserCountry { get; set; }
        //public string? UserZip { get; set; }
        //public string? UserImage { get; set; }
        //public string? UserStatus { get; set; }
        //public string? UserAbout { get; set; }
        //public string? UserToken { get; set; }
        //public string? UserTokenExpiry { get; set; }
        //public string? UserCreatedDate { get; set; }
        //public string? UserCreatedTime { get; set; }
        //public string? UserUpdatedDate { get; set; }
        //public string? UserUpdatedTime { get; set; }
        //public string? UserLastLoginDate { get; set; }
        //public string? UserLastLoginTime { get; set; }
        //public string? UserLastLogoutDate { get; set; }
        //public string? UserLastLogoutTime { get; set; }
        //public string? UserLastLoginIp { get; set; }
        //public string? UserLastLogoutIp { get; set; }
        //public string? UserLastLoginDevice { get; set; }
        //public string? UserLastLogoutDevice { get; set; }
        //public string? UserLastLoginBrowser { get; set; }
        //public string? UserLastLogoutBrowser { get; set; }
        //public string? UserLastLoginOs { get; set; }
        //public string? UserLastLogoutOs { get; set; }
        //public string? UserLastLoginLocation { get; set; }
        //public string? UserLastLogoutLocation { get; set; }
        //public string? UserLastLoginLatitude { get; set; }
        //public string? UserLastLogoutLatitude { get; set; }
        //public string? UserLastLoginLongitude { get; set; }
        //public string? UserLastLogoutLongitude { get; set; }
        //public string? UserLastLoginCountry { get; set; }   
    }
}
