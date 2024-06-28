namespace Domain.Entities
{
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
