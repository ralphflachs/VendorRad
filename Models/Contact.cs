using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorRad.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Company { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
    }
}
