using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorRad.Models
{
    public class Customer : Contact
    {
        public required string SalesNotes { get; set; }
    }
}
