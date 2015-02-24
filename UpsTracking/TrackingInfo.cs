using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpsTracking
{
    public class TrackingInfo
    {
        public string UpsTrackingNumber { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string CurrentLocation { get; set; }

        //We currently don't care about the rest of the info. If you want it, you'll have to add it. 

    }
}
