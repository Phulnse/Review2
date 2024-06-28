namespace Application.ViewModels.CouncilVMs
{
    public class CouncilInforRes
    {
        public Guid CouncilId { get; set; }
        public bool IsChairman { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
