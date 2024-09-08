using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace VendorRad.Models
{
    public class ContactManager
    {
        private readonly string contactsFilePath = "contacts.json";
        private readonly string masterVendorFilePath = "master_vendors.json";

        public List<MasterVendor> MasterVendors { get; private set; }

        public ContactManager()
        {
            LoadMasterVendors();
        }

        // Load or initialize contacts from the file
        public List<Contact> LoadContacts()
        {
            if (File.Exists(contactsFilePath))
            {
                var json = File.ReadAllText(contactsFilePath);
                return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
            }
            else
            {
                // Initialize with default contacts if the file doesn't exist
                var initialContacts = new List<Contact>
                {
                    // Customers
                    new Customer { Name = "Alice Johnson", Company = "AJ Solutions", PhoneNumber = "+1234567890", Address = "1234 Elm St", SalesNotes = "Important client" },
                    new Customer { Name = "Bob Smith", Company = "Smith Consulting", PhoneNumber = "+0987654321", Address = "5678 Oak St", SalesNotes = "Prefers email contact" },
                    new Customer { Name = "Charlie Chaplin", Company = "Chaplin Productions", PhoneNumber = "+1122334455", Address = "910 Pine St", SalesNotes = "Enjoys timely deliveries" },
                    // Vendors
                    new Vendor { Name = "Diana Reeves", Company = "ACME Acids", PhoneNumber = "+12025550101", Address = "2345 Maple St", MasterVendor = new MasterVendor { CompanyName = "ACME Acids", VendorCode = "A001" } },
                    new Vendor { Name = "Evan Wright", Company = "Berenstain Biology", PhoneNumber = "+12025550102", Address = "3456 Birch St", MasterVendor = new MasterVendor { CompanyName = "Berenstain Biology", VendorCode = "A002" } },
                    new Vendor { Name = "Fiona Graham", Company = "Flick’s Fluidics", PhoneNumber = "+12025550103", Address = "4567 Cedar St", MasterVendor = new MasterVendor { CompanyName = "Flick’s Fluidics", VendorCode = "A003" } }
                };

                SaveContacts(initialContacts);
                return initialContacts;
            }
        }

        // Save contacts to the file
        public void SaveContacts(List<Contact> contacts)
        {
            var json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });

            lock (contactsFilePath)
            {
                File.WriteAllText(contactsFilePath, json);
            }
        }

        // Load master vendor list from the file
        private void LoadMasterVendors()
        {
            if (File.Exists(masterVendorFilePath))
            {
                var json = File.ReadAllText(masterVendorFilePath);
                MasterVendors = JsonSerializer.Deserialize<List<MasterVendor>>(json) ?? [];
            }
            else
            {
                // Initialize with the given master list if file doesn't exist
                MasterVendors =
                [
                    new MasterVendor { CompanyName = "ACME Acids", VendorCode = "A001" },
                    new MasterVendor { CompanyName = "Berenstain Biology", VendorCode = "A002" },
                    new MasterVendor { CompanyName = "Flick’s Fluidics", VendorCode = "A003" },
                    new MasterVendor { CompanyName = "Radical Reagents", VendorCode = "D004" },
                    new MasterVendor { CompanyName = "BBST Paper Products", VendorCode = "G065" }
                ];
                SaveMasterVendors(MasterVendors); // Save the initial list to the file
            }
        }

        // Save master vendors to the file
        public void SaveMasterVendors(List<MasterVendor> masterVendors)
        {
            var json = JsonSerializer.Serialize(masterVendors, new JsonSerializerOptions { WriteIndented = true });

            lock (masterVendorFilePath)
            {
                File.WriteAllText(masterVendorFilePath, json);
            }
        }
    }
}
