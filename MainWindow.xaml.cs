using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using VendorRad.Models;
using VendorRad.ViewModels;

namespace VendorRad
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            timer = new DispatcherTimer();
            DataContext = viewModel; // Set the DataContext to the MainViewModel

            StartClock();
        }

        // Handler for saving customer information
        private void SaveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CustomerName.Text) ||
                string.IsNullOrEmpty(CustomerCompany.Text) ||
                string.IsNullOrEmpty(CustomerPhoneNumber.Text) ||
                string.IsNullOrEmpty(CustomerAddress.Text) ||
                string.IsNullOrEmpty(CustomerSalesNotes.Text))
            {
                MessageBox.Show("All fields must be filled out.");
                return;
            }

            var customer = new Customer
            {
                Name = CustomerName.Text,
                Company = CustomerCompany.Text,
                PhoneNumber = CustomerPhoneNumber.Text,
                Address = CustomerAddress.Text,
                SalesNotes = CustomerSalesNotes.Text,
                ContactType = "Customer"
            };

            viewModel.AddContact(customer);
            MessageBox.Show("Customer saved successfully!");
            ClearCustomerFields();
        }

        // Handler for saving vendor information
        private void SaveVendorButton_Click(object sender, RoutedEventArgs e)
        {
            if (VendorCompanyDropdown.SelectedItem == null ||
                string.IsNullOrEmpty(VendorName.Text) ||
                string.IsNullOrEmpty(VendorPhoneNumber.Text) ||
                string.IsNullOrEmpty(VendorAddress.Text))
            {
                MessageBox.Show("Please fill out all fields and select a company.");
                return;
            }

            var masterVendor = VendorCompanyDropdown.SelectedItem as MasterVendor;

            var vendor = new Vendor
            {
                Name = VendorName.Text,
                Company = masterVendor.CompanyName,
                PhoneNumber = VendorPhoneNumber.Text,
                Address = VendorAddress.Text,
                MasterVendor = masterVendor,
                ContactType = "Vendor"
            };

            viewModel.AddContact(vendor);
            MessageBox.Show("Vendor saved successfully!");
            ClearVendorFields();
        }

        // Handler for saving vendor company information
        private void AddMasterVendorButton_Click(object sender, RoutedEventArgs e)
        {
            var companyName = NewMasterVendorCompanyName.Text;
            var vendorCode = NewMasterVendorCode.Text;

            if (string.IsNullOrEmpty(companyName) || string.IsNullOrEmpty(vendorCode))
            {
                MessageBox.Show("Both company name and vendor code must be entered.");
                return;
            }
            else if (viewModel.IsMasterVendorExists(companyName))
            {
                MessageBox.Show("Master vendor already exists.");
                return;
            }
            else if (viewModel.IsMasterVendorCodeExists(vendorCode))
            {
                MessageBox.Show("Vendor code already exists.");
                return;
            }

            viewModel.AddNewMasterVendor(companyName, vendorCode);
            MessageBox.Show("New master vendor added successfully!");
        }

        // Clear the customer fields
        private void ClearCustomerFields()
        {
            CustomerName.Clear();
            CustomerCompany.Clear();
            CustomerPhoneNumber.Clear();
            CustomerAddress.Clear();
            CustomerSalesNotes.Clear();
        }

        // Clear the vendor fields
        private void ClearVendorFields()
        {
            VendorName.Clear();
            VendorCompanyDropdown.SelectedIndex = -1;
            VendorPhoneNumber.Clear();
            VendorAddress.Clear();
        }

        // Start the clock display
        private void StartClock()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // Update the clock display
        private void Timer_Tick(object? sender, EventArgs e)
        {
            ClockDisplay.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}