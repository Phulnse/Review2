namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public int Sex { get; set; }
        public string HomeTown { get; set; }
        public string NationName { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime Issue { get; set; }
        public string PlaceOfIssue { get; set; }
        public string AccountEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string PermanentAddress { get; set; }
        public string CurrentResidence { get; set; }
        public string Unit { get; set; }
        public Guid DepartmentId { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
        public string AcademicRank { get; set; }
        public string TaxCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string Bank { get; set; }
        public bool IsDean { get; set; }
        public string BirthPlace { get; set; }

        public Account Account { get; set; }
        public Department Department { get; set; }
        public ICollection<Topic>? DecidedTopics { get; set; }
        public ICollection<Topic> Topics { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<MemberReview> MemberReviews { get; set; }
        public ICollection<Council> Councils { get; set; }
    }
}
