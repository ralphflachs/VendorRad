using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using VendorRad.Models;

namespace VendorRad
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Properties for Customer input fields
        private string? customerName;
        public string? CustomerName
        {
            get => customerName;
            set { customerName = value; OnPropertyChanged(nameof(CustomerName)); }
        }

        private string? customerCompany;
        public string? CustomerCompany
        {
            get => customerCompany;
            set { customerCompany = value; OnPropertyChanged(nameof(CustomerCompany)); }
        }

        private string? customerPhoneNumber;
        public string? CustomerPhoneNumber
        {
            get => customerPhoneNumber;
            set { customerPhoneNumber = value; OnPropertyChanged(nameof(CustomerPhoneNumber)); }
        }

        private string? customerAddress;
        public string? CustomerAddress
        {
            get => customerAddress;
            set { customerAddress = value; OnPropertyChanged(nameof(CustomerAddress)); }
        }

        private string? customerSalesNotes;
        public string? CustomerSalesNotes
        {
            get => customerSalesNotes;
            set { customerSalesNotes = value; OnPropertyChanged(nameof(CustomerSalesNotes)); }
        }

        // Properties for Vendor input fields
        private string? vendorName;
        public string? VendorName
        {
            get => vendorName;
            set { vendorName = value; OnPropertyChanged(nameof(VendorName)); }
        }

        private MasterVendor? selectedMasterVendor;
        public MasterVendor? SelectedMasterVendor
        {
            get => selectedMasterVendor;
            set { selectedMasterVendor = value; OnPropertyChanged(nameof(SelectedMasterVendor)); }
        }

        private string? vendorPhoneNumber;
        public string? VendorPhoneNumber
        {
            get => vendorPhoneNumber;
            set { vendorPhoneNumber = value; OnPropertyChanged(nameof(VendorPhoneNumber)); }
        }

        private string? vendorAddress;
        public string? VendorAddress
        {
            get => vendorAddress;
            set { vendorAddress = value; OnPropertyChanged(nameof(VendorAddress)); }
        }

        // Properties for adding new Master Vendor
        private string? newMasterVendorCompanyName;
        public string? NewMasterVendorCompanyName
        {
            get => newMasterVendorCompanyName;
            set { newMasterVendorCompanyName = value; OnPropertyChanged(nameof(NewMasterVendorCompanyName)); }
        }

        private string? newMasterVendorCode;
        public string? NewMasterVendorCode
        {
            get => newMasterVendorCode;
            set { newMasterVendorCode = value; OnPropertyChanged(nameof(NewMasterVendorCode)); }
        }

        // Contacts and MasterVendors collections
        public ObservableCollection<Contact> Contacts { get; set; }
        public ObservableCollection<MasterVendor> MasterVendors { get; set; }

        // Commands
        public ICommand SaveCustomerCommand { get; }
        public ICommand SaveVendorCommand { get; }
        public ICommand AddMasterVendorCommand { get; }

        // Clock
        private string? currentTime;
        public string? CurrentTime
        {
            get => currentTime;
            set { currentTime = value; OnPropertyChanged(nameof(CurrentTime)); }
        }

        private readonly System.Windows.Threading.DispatcherTimer timer;

        // ContactManager
        private readonly ContactManager contactManager;

        public MainViewModel()
        {
            // Initialize ContactManager
            contactManager = new ContactManager();

            // Load Contacts and MasterVendors
            Contacts = new ObservableCollection<Contact>(contactManager.LoadContacts());
            MasterVendors = new ObservableCollection<MasterVendor>(contactManager.MasterVendors);

            // Initialize Commands
            SaveCustomerCommand = new RelayCommand(_ => SaveCustomer(), _ => CanSaveCustomer());
            SaveVendorCommand = new RelayCommand(_ => SaveVendor(), _ => CanSaveVendor());
            AddMasterVendorCommand = new RelayCommand(_ => AddMasterVendor(), _ => CanAddMasterVendor());

            // Start Clock
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        }

        // Command methods
        private bool CanSaveCustomer()
        {
            // Validate Customer input fields
            return !string.IsNullOrWhiteSpace(CustomerName) &&
                   !string.IsNullOrWhiteSpace(CustomerCompany) &&
                   !string.IsNullOrWhiteSpace(CustomerPhoneNumber) &&
                   !string.IsNullOrWhiteSpace(CustomerAddress) &&
                   !string.IsNullOrWhiteSpace(CustomerSalesNotes);
        }

        private async void SaveCustomer()
        {
            try
            {
                var customer = new Customer
                {
                    Name = CustomerName!,
                    Company = CustomerCompany!,
                    PhoneNumber = CustomerPhoneNumber!,
                    Address = CustomerAddress!,
                    SalesNotes = CustomerSalesNotes!,
                    ContactType = "Customer"
                };

                Contacts.Add(customer);
                await contactManager.SaveContactsAsync(Contacts.ToList());

                // Clear fields
                CustomerName = CustomerCompany = CustomerPhoneNumber = CustomerAddress = CustomerSalesNotes = string.Empty;
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log and show a message to the user)
            }
        }

        private bool CanSaveVendor()
        {
            return SelectedMasterVendor != null &&
                   !string.IsNullOrWhiteSpace(VendorName) &&
                   !string.IsNullOrWhiteSpace(VendorPhoneNumber) &&
                   !string.IsNullOrWhiteSpace(VendorAddress);
        }

        private async void SaveVendor()
        {
            try
            {
                var vendor = new Vendor
                {
                    Name = VendorName!,
                    Company = SelectedMasterVendor!.CompanyName,
                    PhoneNumber = VendorPhoneNumber!,
                    Address = VendorAddress!,
                    MasterVendor = SelectedMasterVendor,
                    ContactType = "Vendor"
                };

                Contacts.Add(vendor);
                await contactManager.SaveContactsAsync(Contacts.ToList());

                // Clear fields
                VendorName = VendorPhoneNumber = VendorAddress = string.Empty;
                SelectedMasterVendor = null;
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        private bool CanAddMasterVendor()
        {
            return !string.IsNullOrWhiteSpace(NewMasterVendorCompanyName) &&
                   !string.IsNullOrWhiteSpace(NewMasterVendorCode);
        }

        private async void AddMasterVendor()
        {
            try
            {
                if (IsMasterVendorExists(NewMasterVendorCompanyName!))
                {
                    // Show message: Master vendor already exists
                    return;
                }
                else if (IsMasterVendorCodeExists(NewMasterVendorCode!))
                {
                    // Show message: Vendor code already exists
                    return;
                }

                var newMasterVendor = new MasterVendor
                {
                    CompanyName = NewMasterVendorCompanyName!,
                    VendorCode = NewMasterVendorCode!
                };

                MasterVendors.Add(newMasterVendor);
                await contactManager.SaveMasterVendorsAsync(MasterVendors.ToList());

                // Clear fields
                NewMasterVendorCompanyName = NewMasterVendorCode = string.Empty;
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        // Helper methods
        public bool IsMasterVendorExists(string companyName) =>
            MasterVendors.Any(mv => mv.CompanyName.Equals(companyName, StringComparison.OrdinalIgnoreCase));

        public bool IsMasterVendorCodeExists(string vendorCode) =>
            MasterVendors.Any(mv => mv.VendorCode.Equals(vendorCode, StringComparison.OrdinalIgnoreCase));
    }
}
