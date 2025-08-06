using System.Collections.Generic;

namespace CallTaxi.Subscriber.Models
{
    public class VehicleNotificationDto
    {
        public string BrandName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<string> AdminEmails { get; set; } = new List<string>();
    }
}