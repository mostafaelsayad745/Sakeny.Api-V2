using System.ComponentModel.DataAnnotations;

namespace sakeny.Models
{
    public class UserForUpdateDto
    {
        public decimal UserId { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Password is required")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Full Name is required")]
        [MaxLength(200)]
        public string UserFullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Email is required")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "User National ID is required")]
        [MaxLength(50)]
        public string UserNatId { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Gender is required")]
        public string UserGender { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Age is required")]
        public int UserAge { get; set; }

        [Required(ErrorMessage = "User Info is required")]
        [DataType(DataType.MultilineText)]
        public string UserInfo { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Address is required")]
        [DataType(DataType.MultilineText)]
        public string UserAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Account Type is required")]
        public string UserAccountType { get; set; } = string.Empty;
    }
}