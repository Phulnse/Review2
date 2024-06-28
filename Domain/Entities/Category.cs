namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }

        public ICollection<Topic> Topics { get; set; }
    }
}
