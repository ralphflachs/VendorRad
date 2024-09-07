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
        public void LoadMasterVendors()
        {
            if (!File.Exists(masterVendorFilePath))
            {
                MasterVendors = new List<MasterVendor>();
                return;
            }

            var json = File.ReadAllText(masterVendorFilePath);
            MasterVendors = JsonSerializer.Deserialize<List<MasterVendor>>(json) ?? new List<MasterVendor>();
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
