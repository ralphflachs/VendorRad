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
            DataContext = viewModel;
            timer = new DispatcherTimer();
            StartClock();
        }

        // Method for saving customer information
        private void SaveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer
            {
                Name = CustomerName.Text,
                Company = CustomerCompany.Text,
                PhoneNumber = CustomerPhoneNumber.Text,
                Address = CustomerAddress.Text,
                SalesNotes = CustomerSalesNotes.Text
            };

            viewModel.AddContact(customer);
            MessageBox.Show("Customer saved successfully!");
            ClearCustomerFields();
        }

        // Method for saving vendor information
        private void SaveVendorButton_Click(object sender, RoutedEventArgs e)
        {
            var masterVendor = VendorCompanyDropdown.SelectedItem as MasterVendor;

            if (masterVendor == null)
            {
                MessageBox.Show("Please select a master vendor.");
                return; // Exit the method if no vendor is selected
            }

            var vendor = new Vendor
            {
                Name = VendorName.Text,
                Company = masterVendor.CompanyName,
                PhoneNumber = VendorPhoneNumber.Text,
                Address = VendorAddress.Text,
                MasterVendor = masterVendor
            };

            viewModel.AddContact(vendor);
            MessageBox.Show("Vendor saved successfully!");
            ClearVendorFields();
        }

        private void AddMasterVendorButton_Click(object sender, RoutedEventArgs e)
        {
            var companyName = NewMasterVendorCompanyName.Text;
            var vendorCode = NewMasterVendorCode.Text;

            if (!string.IsNullOrEmpty(companyName) && !string.IsNullOrEmpty(vendorCode))
            {
                if (!viewModel.IsMasterVendorExists(companyName))
                {
                    viewModel.AddNewMasterVendor(companyName, vendorCode);
                    MessageBox.Show("New master vendor added successfully!");
                }
                else
                {
                    MessageBox.Show("Master vendor already exists.");
                }
            }
            else
            {
                MessageBox.Show("Please enter both company name and vendor code.");
            }
        }

        private void ClearCustomerFields()
        {
            CustomerName.Clear();
            CustomerCompany.Clear();
            CustomerPhoneNumber.Clear();
            CustomerAddress.Clear();
            CustomerSalesNotes.Clear();
        }

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

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ClockDisplay.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}