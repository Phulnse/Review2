namespace Domain.Enums
{
    public enum TopicProgressEnum
    {
        WaitingForDean = 0,
        WaitingForCouncilFormation = 1,
        WaitingForUploadMeetingMinutes = 2,
        WaitingForDocumentEditing = 3,
        WaitingForCouncilDecision = 4,
        WaitingForUploadContract = 5,
        Completed = 6,
        WaitingForConfigureConference = 7,
        WaitingForDocumentSupplementation = 8,
        WaitingForMakeReviewSchedule = 9,
        WaitingForSubmitRemuneration = 10,
        WaitingForCensorshipRemuneration = 11,
        Waiting = 12,
    }
}
