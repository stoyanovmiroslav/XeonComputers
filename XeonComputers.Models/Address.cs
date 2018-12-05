using System.Collections.Generic;

namespace XeonComputers.Models
{
    public class Address
    {
        public Address()
        {
            this.Addresses = new HashSet<Order>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }

        public string XeonUserId { get; set; }
        public virtual XeonUser XeonUser { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public ICollection<Order> Addresses { get; set; }
    }
}