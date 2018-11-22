namespace XeonComputers.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string Country { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public string Description { get; set; }
    }
}