namespace Domain.Entities
{
    public class Council : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ReviewId { get; set; }
        public bool IsChairman { get; set; }

        public User User { get; set; }
        public Review Review { get; set; }
    }
}
