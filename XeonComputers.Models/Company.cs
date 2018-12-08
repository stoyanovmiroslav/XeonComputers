using System;

namespace XeonComputers.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UniqueIdentifier { get; set; }

        public string Manager { get; set; }

        public string Owner { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual Address Address { get; set; }

        public virtual XeonUser XeonUser { get; set; }
    }
}