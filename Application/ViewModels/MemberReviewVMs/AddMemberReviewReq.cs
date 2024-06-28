namespace Application.ViewModels.MemberReviewVMs
{
    public class AddMemberReviewReq
    {
        public Guid TopicId { get; set; }
        public List<Guid> MemberReviewIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
