using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class UpdateAccountDto
    {
        [Required]
        public Guid Guid { get; set; }
        [PasswordPolicy]
        public string Password { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public int Otp { get; set; }
        [Required]
        public bool IsUsed { get; set; }
        [Required]
        public DateTime ExpiredTime { get; set; }
    }
}
