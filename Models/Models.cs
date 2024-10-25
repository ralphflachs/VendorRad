namespace VendorRad.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Company { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string ContactType { get; set; } = default!;
    }

    public class Customer : Contact
    {
        public string SalesNotes { get; set; } = default!;
    }

    public class Vendor : Contact
    {
        public MasterVendor? MasterVendor { get; set; }
    }

    public class MasterVendor
    {
        public string CompanyName { get; set; } = default!;
        public string VendorCode { get; set; } = default!;
    }
}
