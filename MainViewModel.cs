using System.Collections.ObjectModel;
using VendorRad.Models;

namespace VendorRad.ViewModels
{
    public class MainViewModel
    {
        // Expose ObservableCollection for binding
        public ObservableCollection<Contact> Contacts { get; set; }

        private readonly ContactManager contactManager;

        public MainViewModel()
        {            
            contactManager = new ContactManager();         
            var contactsFromManager = contactManager.LoadContacts();
            Contacts = new ObservableCollection<Contact>(contactsFromManager);
        }

        // Add or update contact in the ObservableCollection
        public void AddOrUpdateContact(Contact contact)
        {
            var existingContact = Contacts.FirstOrDefault(c => c.Name == contact.Name || c.PhoneNumber == contact.PhoneNumber);
            if (existingContact != null)
            {
                existingContact.Update(contact);
            }
            else
            {
                Contacts.Add(contact);
            }

            // Save updated contacts via the ContactManager
            contactManager.SaveContacts([.. Contacts]);
        }
    }
}
