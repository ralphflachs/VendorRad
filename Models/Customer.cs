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

        // Override the Update method to handle customer-specific fields
        public override void Update(Contact newContact)
        {
            base.Update(newContact); // Call the base method to update common fields

            // Cast newContact to Customer and update customer-specific fields
            if (newContact is Customer newCustomer)
            {
                SalesNotes = newCustomer.SalesNotes;
            }
        }
    }
}
