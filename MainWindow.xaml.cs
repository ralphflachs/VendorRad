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

        private void AddMasterVendorButton_Click(object sender, RoutedEventArgs e)
        {
            var companyName = NewMasterVendorCompanyName.Text;
            var vendorCode = NewMasterVendorCode.Text;

            if (!string.IsNullOrEmpty(companyName) && !string.IsNullOrEmpty(vendorCode))
            {
                // Assuming ViewModel method to add new master vendor
                ((MainViewModel)DataContext).AddNewMasterVendor(companyName, vendorCode);
                MessageBox.Show("New master vendor added successfully!");
            }
            else
            {
                MessageBox.Show("Please enter both company name and vendor code.");
            }
        }

        // Event handler for saving vendor contact
        private void SaveVendorButton_Click(object sender, RoutedEventArgs e)
        {
            /*var masterVendor = viewModel.GetMasterVendor(VendorCompany.Text);
            if (masterVendor == null)
            {
                // Manually ask for vendor code, as it needs to be linked with the master vendor list
                string vendorCode = PromptForVendorCode();
                masterVendor = viewModel.AddVendor(VendorCompany.Text, vendorCode);
            }

            var vendor = new Vendor
            {
                Name = VendorName.Text,
                Company = VendorCompany.Text,
                PhoneNumber = VendorPhoneNumber.Text,
                Address = VendorAddress.Text,
                MasterVendor = masterVendor
            };

            viewModel.AddContact(vendor);

            MessageBox.Show("Vendor saved successfully!");
            ClearVendorFields();*/
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
            //VendorCompany.Clear();
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