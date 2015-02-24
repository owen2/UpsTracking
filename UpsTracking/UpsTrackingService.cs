using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UpsTracking.TrackWS;

namespace UpsTracking
{
    public class UpsTrackingService
    {

        protected UPSSecurity SecurityToken { get; set; }
        protected TrackPortTypeClient Client { get; set; }
        /// <summary>
        /// Determines what kind of trackable object to look for. Look in the UPS docs.
        /// </summary>
        public string TrackingOption { get; set; }
        /// <summary>
        /// Determines what kind of activity to get. Look in the UPS docs.
        /// </summary>
        public string RequestOption { get; set; }

        /// <summary>
        /// Wraps the Ups Tracking Web Service
        /// </summary>
        /// <param name="UpsUserName">A UPS username (like the one you log into ups.com with)</param>
        /// <param name="UpsPassword">A UPS password</param>
        /// <param name="UpsApiKey">The Access License Number provided by ups. Get one here https://www.ups.com/upsdeveloperkit/requestaccesskey </param>
        public UpsTrackingService(string upsUserName, string upsPassword, string upsApiKey)
        {
            //TODO: Don't Hardcode

            SecurityToken = new TrackWS.UPSSecurity { ServiceAccessToken = new UPSSecurityServiceAccessToken { AccessLicenseNumber = upsApiKey }, UsernameToken = new UPSSecurityUsernameToken { Username = upsUserName, Password = upsPassword } };
            TrackingOption = "02";
            RequestOption = "1";
            Client = new TrackPortTypeClient("TrackPort");
        }

        DateTime parseUpsDateString(string upsDate)
        {
            return new DateTime(Int32.Parse(string.Concat(upsDate.Take(4))), Int32.Parse(string.Concat(upsDate.Skip(4).Take(2))), Int32.Parse(string.Concat(upsDate.Skip(6).Take(2))));
        }

        public IEnumerable<TrackingInfo> GetTrackingInfo(params string[] trackingNumbers)
        {
            return trackingNumbers.Select(
                number =>
                {
                    try
                    {
                        var data = Client.ProcessTrack(SecurityToken, new TrackRequest
                        {
                            InquiryNumber = number,
                            TrackingOption = TrackingOption,
                            Request = new RequestType { RequestOption = new[] { RequestOption } }
                        });
                        var package = data.Shipment.First().Package.First();
                        var location = package.Activity.First().ActivityLocation.Address;

                        var locationString = string.Format("{0}, {1}, {2}", location.City, location.StateProvinceCode, location.CountryCode);
                        var status = package.Activity.First().Status.Description;
                        var date = parseUpsDateString(package.Activity.First().Date);
                        return new TrackingInfo { DeliveryStatus = status, UpsTrackingNumber = number, DeliveryDate = date, CurrentLocation = locationString };
                    }
                    catch
                    {
                        return new TrackingInfo { DeliveryStatus = "Not Found", UpsTrackingNumber = number };
                    }
                });
        }
    }
}
