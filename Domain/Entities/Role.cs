namespace Domain.Entities
{
    public class Role
    {
        public string RoleName { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
