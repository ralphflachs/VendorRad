using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace VendorRad.Models
{
    public class ContactManager
    {
        public ObservableCollection<Contact> Contacts { get; private set; } = new ObservableCollection<Contact>();

        private readonly string filePath = "contacts.json";

        public ContactManager()
        {
            LoadContacts();
        }

        // Save contact (update if exists, add if new)
        public void SaveContact(Contact contact)
        {
            // Load contacts from file in case it was updated by another instance
            this.LoadContacts();
            var existingContact = Contacts.FirstOrDefault(c => c.Name == contact.Name || c.PhoneNumber == contact.PhoneNumber);

            if (existingContact != null)
            {
                // Update the existing contact
                existingContact.Update(contact);
            }
            else
            {
                // Add the new contact
                Contacts.Add(contact);
            }

            // Save the updated contacts to file
            SaveContacts();
        }

        // Load contacts from the file and populate the ObservableCollection
        private void LoadContacts()
        {
            lock (filePath)
            {
                if (!File.Exists(filePath))
                {
                    return;
                }

                var json = File.ReadAllText(filePath);
                var contactsFromFile = JsonSerializer.Deserialize<ObservableCollection<Contact>>(json);

                if (contactsFromFile != null)
                {
                    Contacts.Clear();
                    foreach (var contact in contactsFromFile)
                    {
                        Contacts.Add(contact);
                    }
                }
            }
        }

        private void SaveContacts()
        {
            var json = JsonSerializer.Serialize(Contacts, new JsonSerializerOptions { WriteIndented = true });
            lock (filePath)
            {
                File.WriteAllText(filePath, json);
            }
        }
    }
}
