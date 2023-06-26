namespace API.DTOs.Accounts
{
    public class GetAccountDto
    {
        public Guid Guid { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUsed { get; set; }
    }
}
