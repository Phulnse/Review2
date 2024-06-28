namespace Application.ViewModels.NotifyVMs
{
    public class GetNotifyRes
    {
        public int UnreadNotificationsNumber { get; set; }
        public List<NotifyVM> Notifies { get; set; }
    }
}
