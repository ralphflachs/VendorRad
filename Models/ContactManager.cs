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
        private readonly string filePath = "contacts.json";

        // Save contact with file locking
        public void SaveContact(Contact contact)
        {
            lock (filePath)
            {
                var contacts = LoadContacts();
                contacts.Add(contact);
                SaveContacts(contacts);
            }
        }

        private List<Contact> LoadContacts()
        {
            lock (filePath)
            {
                if (!File.Exists(filePath))
                {
                    return new List<Contact>();
                }

                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Contact>>(json);
            }
        }

        private void SaveContacts(List<Contact> contacts)
        {
            var json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
            lock (filePath)
            {
                File.WriteAllText(filePath, json);
            }
        }
    }

}
