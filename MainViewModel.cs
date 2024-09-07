using System.Collections.ObjectModel;
using VendorRad.Models;

namespace VendorRad.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Contact> Contacts { get; set; }

        private readonly ContactManager contactManager;

        public MainViewModel()
        {
            contactManager = new ContactManager();
            var contactsFromManager = contactManager.LoadContacts();
            Contacts = new ObservableCollection<Contact>(contactsFromManager);
        }

        // Add contact in the ObservableCollection
        public void AddContact(Contact contact)
        {
            Contacts.Add(contact);
            contactManager.SaveContacts([.. Contacts]);
        }
    }
}
