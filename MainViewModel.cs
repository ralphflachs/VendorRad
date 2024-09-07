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

        // Add contact in the ObservableCollection
        public void AddContact(Contact contact)
        {
            Contacts.Add(contact);
            contactManager.SaveContacts([.. Contacts]);
        }

        // Add or update a vendor in the ObservableCollection and master vendor list
        public bool AddVendor(Vendor vendor, string companyName, string vendorCode)
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

            // Link the vendor contact to the corresponding MasterVendor
            vendor.MasterVendor = masterVendor;

            // Add new vendor
            Contacts.Add(vendor);
            contactManager.SaveContacts([.. Contacts]);

            return true;
        }
    }
}
