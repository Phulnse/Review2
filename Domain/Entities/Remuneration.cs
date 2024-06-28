namespace Domain.Entities
{
    public class Remuneration : BaseEntity
    {
        public string RemunerationName { get; set; }
        public string RemunerationLink { get; set; }
        public Guid TopicId { get; set; }
        public bool? IsAccepted { get; set; }

        public Topic Topic { get; set; }
    }
}
