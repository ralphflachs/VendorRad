using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace VendorRad.Models
{
    public class ContactManager
    {
        private readonly string filePath = "contacts.json";

        // Save contact with file locking (update if exists, add if new)
        public void SaveContact(Contact contact)
        {
            lock (filePath)
            {
                var contacts = LoadContacts();

                // Try to find if the contact already exists based on unique criteria (e.g., Name or PhoneNumber)
                var existingContact = contacts.FirstOrDefault(c => c.Name == contact.Name || c.PhoneNumber == contact.PhoneNumber);

                if (existingContact != null)
                {
                    // Update existing contact details
                    existingContact.Company = contact.Company;
                    existingContact.Address = contact.Address;
                    existingContact.PhoneNumber = contact.PhoneNumber;

                    if (existingContact is Customer existingCustomer && contact is Customer newCustomer)
                    {
                        existingCustomer.SalesNotes = newCustomer.SalesNotes; // Update Customer-specific field
                    }
                }
                else
                {
                    // If contact doesn't exist, add it as a new contact
                    contacts.Add(contact);
                }

                // Save updated contact list
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
