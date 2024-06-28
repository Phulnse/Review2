using Domain.Enums;

namespace Domain.Entities
{
    public class Contract : BaseEntity
    {
        public string ContractName { get; set; }
        public string ContractLink { get; set; }
        public ContractStateEnum State { get; set; }
        public Guid TopicId { get; set; }

        public Topic Topic { get; set; }
    }
}
