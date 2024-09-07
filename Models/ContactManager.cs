using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace VendorRad.Models
{
    public class ContactManager
    {
        private readonly string contactsFilePath = "contacts.json";
        private readonly string masterVendorsFilePath = "master_vendors.json";

        public ObservableCollection<Contact> Contacts { get; private set; }
        public List<MasterVendor> MasterVendors { get; private set; }

        public ContactManager()
        {
            Contacts = new ObservableCollection<Contact>();
            MasterVendors = new List<MasterVendor>();
            LoadContacts();
            LoadMasterVendors();
        }

        // Load contacts from the file
        public void LoadContacts()
        {
            if (!File.Exists(contactsFilePath))
            {
                Contacts = new ObservableCollection<Contact>();
                return;
            }

            var json = File.ReadAllText(contactsFilePath);
            var contactsFromFile = JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
            Contacts = new ObservableCollection<Contact>(contactsFromFile);
        }

        // Load master vendors from the file
        public void LoadMasterVendors()
        {
            if (!File.Exists(masterVendorsFilePath))
            {
                MasterVendors = new List<MasterVendor>();
                return;
            }

            var json = File.ReadAllText(masterVendorsFilePath);
            MasterVendors = JsonSerializer.Deserialize<List<MasterVendor>>(json) ?? new List<MasterVendor>();
        }

        public bool AddVendor(Vendor vendor, string companyName, string vendorCode)
        {
            // Check if the company exists in the master list
            var masterVendor = MasterVendors.FirstOrDefault(mv => mv.CompanyName == companyName);

            if (masterVendor == null)
            {
                // If the company doesn't exist, create a new MasterVendor and add it to the list
                masterVendor = new MasterVendor { CompanyName = companyName, VendorCode = vendorCode };
                MasterVendors.Add(masterVendor);
                SaveMasterVendors();
            }

            // Now link the vendor contact to the corresponding MasterVendor
            vendor.MasterVendor = masterVendor;

            // Save the vendor contact
            Contacts.Add(vendor);
            SaveContacts();

            return true;
        }

        // Save contacts to the file
        public void SaveContacts()
        {
            var json = JsonSerializer.Serialize(Contacts.ToList(), new JsonSerializerOptions { WriteIndented = true });

            lock (contactsFilePath)
            {
                File.WriteAllText(contactsFilePath, json);
            }
        }

        // Save master vendors to the file
        public void SaveMasterVendors()
        {
            var json = JsonSerializer.Serialize(MasterVendors, new JsonSerializerOptions { WriteIndented = true });

            lock (masterVendorsFilePath)
            {
                File.WriteAllText(masterVendorsFilePath, json);
            }
        }
    }
}
