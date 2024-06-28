using Domain.Enums;

namespace Domain.Entities
{
    public class FileType : BaseEntity
    {
        public FileStateEnum State { get; set; }
        public string TypeName { get; set; }
    }
}
