using System.Text;
using System.Windows;
using System.Windows.Controls;
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

        // Event handler for saving customer contact
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

        // Event handler for saving vendor contact
        private void SaveVendorButton_Click(object sender, RoutedEventArgs e)
        {
            var vendor = new Vendor
            {
                Name = VendorName.Text,
                Company = VendorCompany.Text,
                PhoneNumber = VendorPhoneNumber.Text,
                Address = VendorAddress.Text
            };

            // Manually ask for vendor code, as it needs to be linked with the master vendor list
            string vendorCode = PromptForVendorCode();

            bool success = viewModel.AddVendor(vendor, vendor.Company, vendorCode);
            if (success)
            {
                MessageBox.Show("Vendor saved successfully!");
                ClearVendorFields();
            }
            else
            {
                MessageBox.Show("Failed to save vendor. The vendor may already exist.");
            }
        }

        // Method to simulate asking for a vendor code (could be done with a dialog in a real app)
        private string PromptForVendorCode()
        {
            // For simplicity, we'll just return a placeholder code
            return VendorCodeInputDialog(); // You can create a pop-up input dialog or keep it simple.
        }

        private string VendorCodeInputDialog()
        {
            // Simulate vendor code input for now
            return "V001"; // Placeholder, this can be customized to get actual input.
        }

        // Clear customer input fields
        private void ClearCustomerFields()
        {
            CustomerName.Clear();
            CustomerCompany.Clear();
            CustomerPhoneNumber.Clear();
            CustomerAddress.Clear();
            CustomerSalesNotes.Clear();
        }

        // Clear vendor input fields
        private void ClearVendorFields()
        {
            VendorName.Clear();
            VendorCompany.Clear();
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