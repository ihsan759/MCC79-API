using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_accounts")]
    public class Account : BaseEntity
    {
        [Column("password", TypeName = "nvarchar(255)")]
        public string Password { get; set; }
        [Column("is_deleted", TypeName = "bit")]
        public bool IsDeleted { get; set; }
        [Column("otp")]
        public int Otp { get; set; }
        [Column("is_used", TypeName = "bit")]
        public bool IsUsed { get; set; }
        [Column("expired_time")]
        public DateTime ExpiredTime { get; set; }

        // Cardinality
        public Employee? Employee { get; set; }
        public ICollection<AccountRole>? AccountRole { get; set; }
    }
}
