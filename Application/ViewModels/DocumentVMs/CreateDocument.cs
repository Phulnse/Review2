namespace Application.ViewModels.DocumentVMs
{
    public abstract class CreateDocument
    {
        public Guid TopicId { get; set; }
        public NewFile NewFile { get; set; }
    }
}
