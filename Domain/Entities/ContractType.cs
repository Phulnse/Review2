using Domain.Enums;

namespace Domain.Entities
{
    public class ContractType : BaseEntity
    {
        public string TypeName { get; set; }
        public ContractStateEnum State { get; set; }
    }
}
