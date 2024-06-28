using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ReviewVMs
{
    public class ReviewMeetingInfor
    {
        public Guid ReviewId { get; set; }
        public DateTime MeetingTime { get; set; }
        public string MeetingDetail { get; set; }
        public string ChairmanName { get; set; }
        public string State { get; set; }
    }
}
