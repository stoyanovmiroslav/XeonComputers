using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeonComputers.ViewModels.Home
{
    public class IndexProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal ParnersPrice { get; set; }

        public double Raiting { get; set; }

        public int ReviewsCount { get; set; }

        public string ImageUrl { get; set; }
    }
}