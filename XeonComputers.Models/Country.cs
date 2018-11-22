using System.Collections.Generic;

namespace XeonComputers.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}