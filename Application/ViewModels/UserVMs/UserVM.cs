namespace Application.ViewModels.UserVMs
{
    public class UserVM
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
        public string Title { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
        public string AcademicRank { get; set; }
        public string TaxCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string Bank { get; set; }
        public string BirthPlace { get; set; }
        public Guid DepartmentId { get; set; }
    }

    public class CreateUserReq
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
        public string Title { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
        public string AcademicRank { get; set; }
        public string TaxCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string Bank { get; set; }
        public string BirthPlace { get; set; }
        public Guid DepartmentId { get; set; }
    }

    public class UpdateUserReq
    {
        public Guid UserId { get; set; }
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
        public string Title { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
        public string AcademicRank { get; set; }
        public string TaxCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string Bank { get; set; }
        public string BirthPlace { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
