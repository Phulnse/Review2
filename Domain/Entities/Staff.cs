namespace Domain.Entities
{
    public class Staff : BaseEntity
    {
        public string StaffName { get; set; }
        public string AccountEmail { get; set; }

        public Account Account { get; set; }
    }
}
