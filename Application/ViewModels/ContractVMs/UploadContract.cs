namespace Application.ViewModels.ContractVMs
{
    public abstract class UploadContract
    {
        public Guid TopicId { get; set; }
        public string ContractName { get; set; }
        public string ContractLink { get; set; }
    }
}
