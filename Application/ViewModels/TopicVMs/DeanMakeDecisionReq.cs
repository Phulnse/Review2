namespace Application.ViewModels.TopicVMs
{
    public class DeanMakeDecisionReq
    {
        public Guid DiciderId { get; set; }
        public Guid TopicId { get; set; }
        public bool DeanDecision { get; set; }
        public string? ReasonOfDecision { get; set; }
    }
}
