using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace VendorRad.Models
{
    public class ContactManager
    {
        private List<Contact> contacts;
        private readonly string filePath = "contacts.json";

        public ContactManager()
        {
            // Load contacts from file if exists, otherwise initialize an empty list
            contacts = File.Exists(filePath) ? LoadContacts() : new List<Contact>();            
        }

        // Save a new contact (either Customer or Vendor)
        public void SaveContact(Contact contact)
        {
            contacts.Add(contact);
            SaveContacts();
        }

        // Retrieve all contacts
        public List<Contact> GetAllContacts()
        {
            return contacts;
        }

        // Save the contacts list to a JSON file
        private void SaveContacts()
        {
            var json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Load contacts from the JSON file
        private List<Contact> LoadContacts()
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Contact>>(json);
        }
    }
}
