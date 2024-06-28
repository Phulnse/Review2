namespace Application.ViewModels.TopicVMs
{
    public class ResubmitProcess
    {
        public string WaitingForDocumentEditing { get; set; }
        public DateTime UploadDocumentTime { get; set; }
        public string WaitingForCouncilDecision { get; set; }
        public DateTime? UploadFeedbackTime { get; set; }
        public int NumberOfResubmmit { get; set; }
    }
}
