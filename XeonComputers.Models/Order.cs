using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int MyProperty { get; set; }

        public Status Status { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
