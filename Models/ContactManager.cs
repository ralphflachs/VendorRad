using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace VendorRad.Models
{
    public class ContactManager
    {
        private readonly string filePath = "contacts.json";

        // Load contacts from the file
        public List<Contact> LoadContacts()
        {
            if (!File.Exists(filePath))
            {
                return new List<Contact>();
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
        }

        // Save contacts to the file
        public void SaveContacts(List<Contact> contacts)
        {
            var json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });

            lock (filePath)
            {
                File.WriteAllText(filePath, json);
            }
        }
    }
}
