using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in new UpsTracking.UpsTrackingService("blah", "blah", "blah").GetTrackingInfo(new[] {
           "tracking numbers here"}))
            {
                Console.WriteLine(string.Format("{2}: {0} in {3} on {1}", item.DeliveryStatus, item.DeliveryDate.ToString("MM/dd/yyyy"), item.UpsTrackingNumber, item.CurrentLocation));
            }
            Console.ReadKey();
        }
    }
}
