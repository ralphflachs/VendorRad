using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorRad.Models
{
    public class Vendor : Contact
    {
        // If there are vendor-specific fields, you can override Update here
        public override void Update(Contact newContact)
        {
            base.Update(newContact); // Call the base method to update common fields            
        }
    }
}
