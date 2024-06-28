namespace Application.ViewModels.RemunerationVMs
{
    public class SubmitRemunerationReq
    {
        public string RemunerationName { get; set; }
        public string RemunerationLink { get; set; }
        public Guid TopicId { get; set; }
    }
}
