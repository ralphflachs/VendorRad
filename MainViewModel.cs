using System.Collections.ObjectModel;
using VendorRad.Models;

namespace VendorRad.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Contact> Contacts { get; set; }
        public ObservableCollection<MasterVendor> MasterVendors { get; set; }

        private readonly ContactManager contactManager;

        public MainViewModel()
        {
            contactManager = new ContactManager();

            // Load contacts and master vendors from the ContactManager
            var contactsFromManager = contactManager.LoadContacts();
            var masterVendorsFromManager = contactManager.MasterVendors;

            Contacts = new ObservableCollection<Contact>(contactsFromManager);
            MasterVendors = new ObservableCollection<MasterVendor>(masterVendorsFromManager);
        }

        // Add contact in the ObservableCollection and save it to the file
        public void AddContact(Contact contact)
        {
            Contacts.Add(contact);
            contactManager.SaveContacts([.. Contacts]);
        }

        // Get the master vendor from the master vendor list, null if not found
        public MasterVendor? GetMasterVendor(string companyName) => MasterVendors.FirstOrDefault(mv => mv.CompanyName == companyName);

        // Add or update a vendor in the ObservableCollection and master vendor list
        public MasterVendor AddVendor(string companyName, string vendorCode)
        {
            // Check if the company exists in the master vendor list
            var masterVendor = MasterVendors.FirstOrDefault(mv => mv.CompanyName == companyName);

            if (masterVendor == null)
            {
                // If the company doesn't exist, add it to the master vendor list
                masterVendor = new MasterVendor { CompanyName = companyName, VendorCode = vendorCode };
                MasterVendors.Add(masterVendor);
                contactManager.SaveMasterVendors([.. MasterVendors]);
            }

            return masterVendor;
        }

        public void AddNewMasterVendor(string companyName, string vendorCode)
        {
            var newMasterVendor = new MasterVendor { CompanyName = companyName, VendorCode = vendorCode };
            MasterVendors.Add(newMasterVendor);
            contactManager.SaveMasterVendors([.. MasterVendors]);
        }
    }
}
