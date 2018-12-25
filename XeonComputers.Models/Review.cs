using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int Raiting { get; set; }

        public string Comment { get; set; }

        public Product Product { get; set; }
    }
}