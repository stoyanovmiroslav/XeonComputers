using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Models
{
    public class PartnerRequest
    {
        public int Id { get; set; }

        public string XeonUserId { get; set; }
        public XeonUser XeonUser { get; set; }
    }
}