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

        // Load contacts from the file
        public List<Contact> LoadContacts()
        {
            if (!File.Exists(contactsFilePath))
            {
                return new List<Contact>();
            }

            var json = File.ReadAllText(contactsFilePath);
            return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
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
